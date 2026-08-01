using UnityEngine;
using UnityEngine.InputSystem;

public class fish : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 5f;
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
}
