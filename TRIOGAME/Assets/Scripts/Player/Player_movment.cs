using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Movement : MonoBehaviour
{
    public bool PlayingWithControler = false;
    [Space]
    public float maxSpeed = 6f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float rotationSpeed = 10f;
    public CharacterController controller;
    private Vector3 PlayerPosition;
    public PlayerGraber playerGraber;
    private Vector3 targetVelocity;
    [Space]
    public Animator Ani;

    void FixedUpdate()
    {
        MoveHandeler(); // MovmentHandeler is in Region Handel_Movnent
    }
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

        targetVelocity = new Vector3(input.x, 0, input.y);

        // This doesn't make the player move faster diagonally
        if (targetVelocity.magnitude > 1) targetVelocity.Normalize();

        targetVelocity *= maxSpeed;

        float rate = targetVelocity.magnitude > 0 ? acceleration : deceleration;

        PlayerPosition = Vector3.MoveTowards(PlayerPosition, targetVelocity, rate * Time.deltaTime);

        //if the player is holding a log then it shuld look towards the log
        if (playerGraber.isGrabbing && playerGraber.Interactebole != null && playerGraber.worldGrabPoint != Vector3.zero)
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
        if(targetVelocity == Vector3.zero)
        {
            Ani.SetBool("Walk", false);

        }
        else
        {
            Ani.SetBool("Walk", true);

        }

        // chopp tree animation is called from playerGraber
    }

}