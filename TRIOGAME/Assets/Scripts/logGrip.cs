using UnityEngine;

public class logGrip : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Vector3 Offsett = new Vector3(0, 0, 0);
    public void OnPlayerHoldingTree(float Grabforce, Vector3 targetPosition)
    {
        Debug.Log("Player is holding the tree: " + gameObject.name);
        transform.position = Vector3.SmoothDamp(transform.position + Offsett, targetPosition, ref velocity, Grabforce);
    }
}
