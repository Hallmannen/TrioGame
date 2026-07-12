using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Movement : MonoBehaviour
{
    public float maxSpeed = 6f;
    public float acceleration = 15f;
    public float deceleration = 20f;
    public float rotationSpeed = 10f;
    public float GrabRange = 1f;

    public CharacterController controller;
    private Vector3 velocity;


    void Update()
    {
        MoveHandeler();
        PickupHandeler();
    }
    void PickupHandeler()
    {
        for (int i = -1; i <= 1; i++)
        {
            float angle = (transform.eulerAngles.y + i * 30) * Mathf.Deg2Rad;

            Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));

            Debug.DrawRay(transform.position, dir * GrabRange, Color.red);

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, GrabRange))
            {
                Debug.Log(hit.collider.name + " is in range!");
            }
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

        //Add rotation to the player based on the input direction
        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.SimpleMove(velocity);
    }
}