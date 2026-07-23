using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGraber : MonoBehaviour
{
    public Image TreeChoppBar;
    public float Grabforce = 20f;
    public float GrabRange = 1f;
    public float SphercastRadius = 1;
    public Vector3 GrabPositionOffset = new Vector3(1, 1, 0);
    public GameObject Interactebole;
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
    private float ChoppBarValue;
    private float targetBarValue;
    void Update()
    {
        PickupHandeler();

        ChoppBarValue = Mathf.Lerp(ChoppBarValue, targetBarValue, Time.deltaTime * 10f);

        TreeChoppBar.fillAmount = ChoppBarValue;
        if (ChoppBarValue >= 0.9f) targetBarValue = 0.0f;
    }
    void FixedUpdate()
    {
        DrawRayForPlayer();
    }
    void PickupHandeler()
    {
        if (Keyboard.current != null && !player_Movement.PlayingWithControler && Keyboard.current.eKey.wasPressedThisFrame) // this need to be in update so it can reliebly se when the player is pressing the e Button
        {
            Interact();
        }

        if (Gamepad.current != null && player_Movement.PlayingWithControler && Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            Interact();
        }
    }
    void OnDrawGizmos()
    {
        if (worldGrabPoint == Vector3.zero) return;
        Gizmos.DrawWireSphere(worldGrabPoint, SphercastRadius);
    }
    void Castray()
    {
        float angle = transform.eulerAngles.y * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));

        if (Physics.SphereCast(rayOrigin, SphercastRadius, dir, out hit, GrabRange)) // here i is where the ray is created
        {
            if (!isGrabbing && hit.collider.CompareTag("FalenTree"))
            {
                Interactebole = hit.collider.gameObject;
                if (Interactebole != null)
                {
                    localGrabPoint = Interactebole.transform.InverseTransformPoint(hit.point);
                }
            }
            if (!isGrabbing && hit.collider.CompareTag("Tree"))
            {
                Interactebole = hit.collider.gameObject;
                if (Interactebole != null)
                {
                    Tree TreeScript = Interactebole.GetComponent<Tree>();

                    targetBarValue = 1f - ((float)(TreeScript.treeHP - 1) / TreeScript.maxTreeHP);

                    TreeScript.choopTree();

                    Interactebole = null; // we dont need the Tree gameobject anny more
                }
            }
        }
    }
    void DrawRayForPlayer()
    {
        rayOrigin = transform.position + transform.up * 0.4f;

        logStuck_moveModifier = 1;

        if (Interactebole != null && Interactebole.CompareTag("FalenTree") && isGrabbing)
        {
            Vector3 targetPosition = transform.position + transform.TransformDirection(GrabPositionOffset); // this and the row below updated the postition so it moves with the player

            worldGrabPoint = Interactebole.transform.TransformPoint(localGrabPoint);

            CalculateLogStuckMoveModifier();

            Debug.DrawLine(rayOrigin, worldGrabPoint, Color.red); // added a debug så we can se where the player has grabd the tree

            Interactebole.GetComponent<logGrip>().OnPlayerHoldingTree(Grabforce, targetPosition, worldGrabPoint); // here i say where the log huld go
        }
        else if (Interactebole == null && isGrabbing)
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
    void ChangeIsGrabbig()
    {
        if (!isGrabbing)
        {
            if (Interactebole != null)
            {
                isGrabbing = true;
                CanGrabBool = false;
            }
        }
        else
        {
            isGrabbing = false;
            Interactebole = null;
        }
    }
    void Interact()
    {
        Castray();
        ChangeIsGrabbig();
    }
}
