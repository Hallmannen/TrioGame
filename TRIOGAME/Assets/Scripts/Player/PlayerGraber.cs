using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGraber : MonoBehaviour
{
    public Image TreeChoppBar;
    public Gradient gradient;
    public GameObject ChoppPartical;
    [Space]
    public float Grabforce = 20f;
    public float GrabRange = 1f;
    public float SphercastRadius = 1;
    public Vector3 GrabPositionOffset = new Vector3(1, 1, 0);
    public GameObject Interactebole = null;
    public bool isGrabbing = false;
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
    public float TimeAfterLastChopp; // the time it takes for the Chopp  bar to diseper after last Chopp
    private float EndTimer;
    private float LastChoppBarVlue;

    void Start()
    {
        EndTimer = TimeAfterLastChopp;
    }
    void Update()
    {
        rayOrigin = transform.position + transform.up * 0.4f; // update the rayOrigon to the playerposition every frame

        InteractHandeler();
        ChopBarLogic();
    }
    void FixedUpdate()
    {
        TryGrabing();
    }
    void ChopBarLogic()
    {
        LastChoppBarVlue = ChoppBarValue;

        ChoppBarValue = Mathf.Lerp(ChoppBarValue, targetBarValue, Time.deltaTime * 5f);

        if (ChoppBarValue <= LastChoppBarVlue) targetBarValue = 0.0f;

        TreeChoppBar.fillAmount = ChoppBarValue;
        if (ChoppBarValue >= 0.9f)
        {
            if (EndTimer <= 0)
            {
                targetBarValue = 0.0f;
                EndTimer = TimeAfterLastChopp;
            }
            else
            {
                EndTimer -= 1f * Time.deltaTime;
            }
        }
        TreeChoppBar.color = gradient.Evaluate(TreeChoppBar.fillAmount);
    }
    void InteractHandeler() // this need to be in update so it can reliebly se when the player is pressing the e Button
    {
        //keybord
        if (!player_Movement.PlayingWithControler && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                intreact();
                return;
            }
        }
        //Controler
        else if (player_Movement.PlayingWithControler && Gamepad.current != null)
        {
            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                intreact();
                return;
            }
        }
    }
    void OnDrawGizmos()
    {
        if (Interactebole == null) return;

        Gizmos.DrawWireSphere(worldGrabPoint, SphercastRadius);
    }
    void Castray()
    {
        Vector3 dir = transform.forward;

        if (Physics.SphereCast(rayOrigin, SphercastRadius, dir, out RaycastHit hit, GrabRange)) // here i is where the ray is created
        {
            Interactebole = hit.collider.gameObject;

            if (Interactebole != null)
            {
                localGrabPoint = Interactebole.transform.InverseTransformPoint(hit.point);
                return; // here is Intreactebole the thing The player is trying to grab
            }
        }
    }
    bool TryGrabing() // return true if you need to change is grabing when you are trying to grab something
    {
        logStuck_moveModifier = 1;

        if (Interactebole == null)
        {
            isGrabbing = false;
            return false; // cant do anything if Interactebole is null
        }

        worldGrabPoint = Interactebole.transform.TransformPoint(localGrabPoint);
        Vector3 targetPosition = transform.position + transform.TransformDirection(GrabPositionOffset);

        // this ruins every frame while the player is holding a log
        if (Interactebole.CompareTag("FalenTree")) // Grabes the tree log
        {
            if (!CheckIfPlayerLostObject()) return false; // need to check if the player loses grip of what its holding!

            Interactebole.GetComponent<logGrip>().OnPlayerHoldingTree(Grabforce, targetPosition, worldGrabPoint); // here i say where the log huld go

            player_Movement.loockAtObject = true; // make the player look at the log while holding it

            return true;
        }
        // this runns once
        if (Interactebole.CompareTag("Tree")) // chopping down tree
        {
            GameObject newPartical = Instantiate(ChoppPartical, worldGrabPoint + Vector3.up, transform.rotation);
            Destroy(newPartical, 6);

            Tree TreeScript = Interactebole.GetComponent<Tree>();

            targetBarValue = 1f - ((float)(TreeScript.treeHP - 1) / TreeScript.maxTreeHP);
            TreeScript.choopTree();
            player_Movement.Ani.Play("ChoppTree"); // chopp tree animation is called;

            Interactebole = null; // we dont need the Tree gameobject anny more

            return false;
        }
        // this runns once
        if (Interactebole.CompareTag("SwimingFish")) // grabbing fish
        {
            Interactebole.tag = "CaughtFish";
            Interactebole.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);

            Interactebole.GetComponent<fish>().enabled = false; // disable the fish script so it cant jump anymore
            Interactebole.transform.GetChild(0).gameObject.SetActive(false); // disable the fish child gameobject so it cant swim anymore
            worldGrabPoint = Interactebole.transform.position; // set the worldgrabpoint to the fish position so the logstuckmovemodifer is corect

            Interactebole = null;// we dont need the fish gameobject anny more
            return false;
        }
        // this runns every frame while the player is holding a fish
        if (Interactebole.CompareTag("CaughtFish")) // grabbing fish
        {
            worldGrabPoint = Vector3.zero;

            Interactebole.transform.position = targetPosition;

            Interactebole.transform.rotation = transform.rotation;

            player_Movement.loockAtObject = false;
            return true;
        }
        return false;
    }
    bool CheckIfPlayerLostObject() // if the player loses the girip of what is holding
    {
        float distanceToLog = Vector3.Distance(rayOrigin, worldGrabPoint);
        logStuck_moveModifier = minLogStuckRange / distanceToLog + 1 - distanceToLog / maxLogStuckRange;
        logStuck_moveModifier = Mathf.Clamp(logStuck_moveModifier, 0.1f, 1f);
        if (logStuck_moveModifier == 0.1f) // to far from log and lossing grip
        {
            isGrabbing = false;
            Interactebole = null;
            return false;
        }
        CanGrabBool = true;
        return true;
    }
    void ChangeIsGrabbig()
    {
        if (Interactebole == null) return;

        if (!isGrabbing)
        {
            isGrabbing = true;
            CanGrabBool = false;
        }
        else
        {
            isGrabbing = false;
            Interactebole = null;
        }
    }
    void intreact()
    {
        Castray();
        if (TryGrabing()) ChangeIsGrabbig();
    }
}
