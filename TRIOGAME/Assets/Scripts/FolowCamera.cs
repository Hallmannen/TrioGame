using UnityEngine;

public class FolowCamera : MonoBehaviour
{
    public Transform player; // The camera will follow this transform (usually the player)
    public Vector3 offset; // Offset from the player's position
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
