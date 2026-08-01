using UnityEngine;
using UnityEngine.InputSystem;

public class fish : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 5f;
    private bool isOnGround = true;

    [System.Obsolete]
    void FixedUpdate()
    {
        if (isOnGround && Random.Range(0f, 1f) < 0.01f) // 1% chance to jump each frame
        {
            rb.AddForce(Vector3.up * jumpForce);
            isOnGround = false;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WaterBottom"))
        {
            isOnGround = true;
        }
    }
}
