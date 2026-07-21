using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGraber : MonoBehaviour
{
    public float Grabforce = 20f;
    public float GrabRange = 1f;
    public float SphercastRadius = 1;
    public Vector3 GrabPositionOffset = new Vector3(1, 1, 0);
    public GameObject Log;
    public bool isGrabbing = false;
    private RaycastHit hit;
    private Vector3 localGrabPoint;
    public Vector3 worldGrabPoint;
    public float minLogStuckRange = 1;
    public float maxLogStuckRange = 5f;
    [Range(1f, 10f)]
    public float logStuck_moveModifier;
    private bool CanGrabBool = false;
    public Vector3 rayOrigin;
    public Player_Movement player_Movement;

    void Update()
    {
        PickupHandeler();
    }
    void FixedUpdate()
    {
        DrawRayForPlayer();
    }
    void PickupHandeler()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !player_Movement.PlayingWithControler) Interact(); // this need to be in update so it can reliebly se when the player is pressing the e Button

        if (Gamepad.current != null && player_Movement.PlayingWithControler && Gamepad.current.buttonWest.wasPressedThisFrame) Interact();
    }
    void DrawRayForPlayer()
    {
        float angle = transform.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        rayOrigin = transform.position + transform.up * 0.4f;

        //if (Physics.Raycast(rayOrigin, dir, out hit, GrabRange)) // here i is where the ray is created
        if(Physics.SphereCast(rayOrigin, SphercastRadius, dir, out hit, GrabRange - SphercastRadius))
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
        else if (Log == null && isGrabbing)
        {
            isGrabbing = false;
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
}
