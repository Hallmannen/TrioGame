using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Movement : MonoBehaviour
{
    public bool PlayingWithControler = false;
    [Space]
    [Header("Movment")]
    public float maxSpeed = 6f;
    private float newmaxSpeed;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float rotationSpeed = 10f;
    public CharacterController controller;
    private Vector3 PlayerPosition;
    public PlayerGraber playerGraber;
    private Vector3 targetVelocity;
    private float dashTime = 1.5f;
    public float DashSpeed = 10f;
    private bool IsSprinting = false;
    public bool loockAtObject = false;
    [Space]
    public Animator Ani;
    [Space]
    [Header("water stuff")]
    [SerializeField] private LayerMask waterMask;
    public bool inWater = true;
    public GameObject waterSplash;
    public ParticleSystem SwimingParticle;
    private float ChangeingDasTimer;
    [Space]
    public GameObject StunedPartical;
    private float StunedTime = 0;
    private bool isStunned = false;
    [Header("standing/swim Offsets")]

    public float centerStanding, hightStanding;
    public float centerSwiming, hightSwiming;
    [Space]
    private PalyerArmScritp playerArmScript;
    void Start()
    {
        newmaxSpeed = maxSpeed;
        playerArmScript = GetComponent<PalyerArmScritp>();
    }
    void FixedUpdate()
    {
        if (!isStunned)
        {
            MoveHandeler(); // MovmentHandeler is in Region Handel_Movnent
        }
    }
    void MoveHandeler()
    {
        Vector2 input = Vector2.zero;

        // Keyboard input
        if (Keyboard.current != null && !PlayingWithControler)
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

        if (IsSprinting)
        {
            ChangeingDasTimer = dashTime;

            if (ChangeingDasTimer <= 0)
            {
                newmaxSpeed = maxSpeed;
                IsSprinting = false;
            }
            else
            {
                ChangeingDasTimer -= 1 * Time.deltaTime;
            }
        }

        targetVelocity = Quaternion.Euler(0, 45, 0) * new Vector3(input.x, 0, input.y); // this makes the player move in the rigth direction!

        // This doesn't make the player move faster diagonally
        if (targetVelocity.magnitude > 1) targetVelocity.Normalize();

        targetVelocity *= newmaxSpeed;

        float rate = targetVelocity.magnitude > 0 ? acceleration : deceleration;

        PlayerPosition = Vector3.MoveTowards(PlayerPosition, targetVelocity, rate * Time.deltaTime);

        //if the player is holding a log then it shuld look towards the log
        if (playerGraber.isGrabbing && playerGraber.Interactebole != null && playerGraber.worldGrabPoint != Vector3.zero && loockAtObject)
        {
            Vector3 direction = playerGraber.worldGrabPoint - transform.position;
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

        controller.SimpleMove(PlayerPosition * playerGraber.logStuck_moveModifier);
    }
    private void Update()
    {
        #region water stuff
        if (targetVelocity == Vector3.zero)
        {
            Ani.SetBool("Walk", false);

        }
        else
        {
            Ani.SetBool("Walk", true);

        }
        if (inWater)
        {
            Ani.Play("Swim");
            controller.center = new Vector3(0, centerSwiming, 0);
            controller.height = hightSwiming;
            SwimingParticle.enableEmission = true;

        }
        else
        {
            SwimingParticle.enableEmission = false;
            controller.center = new Vector3(0, centerStanding, 0);
            controller.height = hightStanding;
        }

        if (targetVelocity != Vector3.zero)
        {
            // chopp tree animation is called from playerGraber
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit waterRay, 1.5f, waterMask) && waterRay.collider.CompareTag("Water"))
            {
                if (!inWater)
                {
                    Destroy(Instantiate(waterSplash, waterRay.point + transform.forward, transform.rotation), 3);
                }
                inWater = true;
            }
            else
            {
                if (inWater)
                {
                    Destroy(Instantiate(waterSplash, transform.position, transform.rotation), 3);
                    Ani.Play("Idle");
                }
                inWater = false;
            }

        }

        #endregion

        if (StunedTime > 0)
        {
            if (!isStunned)
            {
                Destroy(Instantiate(StunedPartical, transform.position + new Vector3(0, 3, 0), Quaternion.identity), StunedTime);
            }
            isStunned = true;
        }
        else isStunned = false;
        StunedTime -= Time.deltaTime;
    }

    public void stunPlayer(float stunTime)
    {
        StunedTime = stunTime;
    }

}