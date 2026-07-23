using UnityEngine;

public class LookAttCamera : MonoBehaviour
{
    public Camera Camera;
    void LateUpdate()
    {
        transform.rotation = Camera.transform.rotation;
    }
}
