//using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Movement : MonoBehaviour
{
    public float maxSpeed = 6f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float rotationSpeed = 10f;
    public float Grabforce = 20f;
    public float GrabRange = 1f;
    public Vector3 GrabPositionOffset = new Vector3(1, 1, 0);
    public CharacterController controller;
    public GameObject Log;
    private Vector3 velocity;
    public bool isGrabbing = false;
    private RaycastHit hit;
    private Vector3 localGrabPoint;
    private Vector3 worldGrabPoint;
    public float GrabSpedSlowMultiplayer;

    public float minLogStuckRange = 1;
    public float maxLogStuckRange = 5f;
    [Range(1f, 10f)]
    private float logStuck_moveModifier;
    private bool NotImportantBool = false; // its not important but do not remove <----

    void Update()
    {
        MoveHandeler();
        PickupHandeler();
    }
    void PickupHandeler()
    {
        DrawRayForPlayer();
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }

    }
    void DrawRayForPlayer()
    {
        float angle = transform.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        Vector3 rayOrigin = transform.position + transform.up * -0.4f;

        if (Physics.Raycast(rayOrigin, dir, out hit, GrabRange)) // here i is where the ray is created
        {
            if (!isGrabbing && hit.collider.CompareTag("FalenTree"))
            {
                Log = hit.collider.gameObject;
                if (Log != null) // checs so i aculy have a log to do transform on
                {
                    localGrabPoint = Log.transform.InverseTransformPoint(hit.point);
                }
            }
        }

        #region logStuck_moveModifier
        logStuck_moveModifier = 1;

        if (isGrabbing)
        {
            float distanceToLog = Vector3.Distance(rayOrigin, worldGrabPoint);
            logStuck_moveModifier = (minLogStuckRange / distanceToLog + 1) - distanceToLog / maxLogStuckRange;
            logStuck_moveModifier = Mathf.Clamp(logStuck_moveModifier, 0.1f, 1f);
            if(logStuck_moveModifier == 0.1f && NotImportantBool) // to far from log and lossing grip
            {
                Interact();
            }
            NotImportantBool = true; // its not important but do not remove <----
        }
        #endregion

        if (Log != null && isGrabbing)
        {
            Vector3 targetPosition = transform.position + transform.TransformDirection(GrabPositionOffset); // this and the row below updated the postition so it moves with the player
            worldGrabPoint = Log.transform.TransformPoint(localGrabPoint);

            Debug.DrawLine(rayOrigin, worldGrabPoint, Color.red); // added a debug så we can se where the player has grabd the tree

            Log.GetComponent<logGrip>().OnPlayerHoldingTree(Grabforce, targetPosition, worldGrabPoint); // here i say where the log huld go
        }
    }
    void MoveHandeler()
    {
        Vector2 input = Vector2.zero;

        // Keyboard input
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        // Controler input
        if (Gamepad.current != null)
        {
            input += Gamepad.current.leftStick.ReadValue();
        }

        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);

        // This doesn't make the player move faster diagonally
        if (targetVelocity.magnitude > 1) targetVelocity.Normalize();

        targetVelocity *= maxSpeed;

        float rate = targetVelocity.magnitude > 0 ? acceleration : deceleration;

        velocity = Vector3.MoveTowards(velocity, targetVelocity, rate * Time.deltaTime);

        //if the player is holding a log then it shuld look towards the log
        if (isGrabbing && Log != null)
        {
            velocity *= GrabSpedSlowMultiplayer;
            Vector3 direction = worldGrabPoint - transform.position;
            direction.y = 0f; // Ignorera höjdskillnad

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        //Add rotation to the player based on the input direction
        else if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        controller.SimpleMove(velocity * logStuck_moveModifier);
    }

    void Interact()
    {
        NotImportantBool = false;
        isGrabbing = !isGrabbing;

    }
}