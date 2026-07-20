//using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class Player_Movement : MonoBehaviour
{
    public bool PlayingWithControler = false;
    [Space]
    public float maxSpeed = 6f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float rotationSpeed = 10f;
    public float Grabforce = 20f;
    public float GrabRange = 1f;
    public Vector3 GrabPositionOffset = new Vector3(1, 1, 0);
    public CharacterController controller;
    public GameObject Log;
    private Vector3 PlayerPosition;
    public bool isGrabbing = false;
    private RaycastHit hit;
    public float GrabSpeedMultiplyer = 0.9f;
    private Vector3 localGrabPoint;
    private Vector3 worldGrabPoint;
    public float minLogStuckRange = 1;
    public float maxLogStuckRange = 5f;
    [Range(1f, 10f)]
    private float logStuck_moveModifier;
    private bool CanGrabBool = false;
    public Vector3 rayOrigin;
    void Start()
    {
        //Application.targetFrameRate = 60;
    }
    void Update()
    {
        PickupHandeler(); // Pickuphandeler is in Handel_Grabing_stuff
    }
    void FixedUpdate()
    {
        MoveHandeler(); // MovmentHandeler is in Region Handel_Movnent
        DrawRayForPlayer(); // DrawRayForPlayer is in Region Handel_Grabing_Stuff
    }
    #region Handel_Grabing_stuff
    void PickupHandeler()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !PlayingWithControler) Interact(); // this need to be in update so it can reliebly se when the player is pressing the e Button

        if (Gamepad.current != null && PlayingWithControler && Gamepad.current.buttonWest.wasPressedThisFrame) Interact();


    }
    void DrawRayForPlayer()
    {
        float angle = transform.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        rayOrigin = transform.position + transform.up * 0.4f;

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

        logStuck_moveModifier = 1;

        if (Log != null && isGrabbing)
        {
            CalculateLogStuckMoveModifier();

            Vector3 targetPosition = transform.position + transform.TransformDirection(GrabPositionOffset); // this and the row below updated the postition so it moves with the player
            worldGrabPoint = Log.transform.TransformPoint(localGrabPoint);

            Debug.DrawLine(rayOrigin, worldGrabPoint, Color.red); // added a debug så we can se where the player has grabd the tree

            Log.GetComponent<logGrip>().OnPlayerHoldingTree(Grabforce, targetPosition, worldGrabPoint); // here i say where the log huld go
        }
    }
    void CalculateLogStuckMoveModifier()
    {
        float distanceToLog = Vector3.Distance(rayOrigin, worldGrabPoint);
        logStuck_moveModifier = minLogStuckRange / distanceToLog + 1 - distanceToLog / maxLogStuckRange;
        logStuck_moveModifier = Mathf.Clamp(logStuck_moveModifier, 0.1f, 1f);
        if (logStuck_moveModifier == 0.1f && CanGrabBool) // to far from log and lossing grip
        {
            Interact();
        }
        CanGrabBool = true;
    }
    void Interact()
    {
        if (!isGrabbing)
        {
            if (Log != null)
            {
                isGrabbing = true;
                CanGrabBool = false;
            }
        }
        else
        {
            isGrabbing = false;
            Log = null;
        }
    }
    #endregion
    #region Handel_Movement
    void MoveHandeler()
    {
        Vector2 input = Vector2.zero;

        // Keyboard input
        if (Keyboard.current != null && PlayingWithControler == false)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        // Controler input
        if (Gamepad.current != null && PlayingWithControler)
        {
            input += Gamepad.current.leftStick.ReadValue();
        }

        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);

        // This doesn't make the player move faster diagonally
        if (targetVelocity.magnitude > 1) targetVelocity.Normalize();

        targetVelocity *= maxSpeed;

        float rate = targetVelocity.magnitude > 0 ? acceleration : deceleration;

        PlayerPosition = Vector3.MoveTowards(PlayerPosition, targetVelocity, rate * Time.deltaTime);

        //if the player is holding a log then it shuld look towards the log
        if (isGrabbing && Log != null)
        {

            Vector3 direction = worldGrabPoint - transform.position;
            direction.y = 0f; // Ignorera höjdskillnad

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        //Add rotation to the player based on the input direction
        else if (PlayerPosition.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(PlayerPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.SimpleMove(PlayerPosition * logStuck_moveModifier);
    }
    #endregion
}