using UnityEngine;

public class logGrip : MonoBehaviour
{
    public Rigidbody rigidbody;
    public bool ShuldAddForce = false;
    public void OnPlayerHoldingTree(float Grabforce, Vector3 targetPosition, Vector3 grabPoint)
    {
        if (rigidbody == null)
        {
            Debug.LogError("Ingen Rigidbody!");
            return;
        }
        Vector3 CurentPos = (targetPosition - grabPoint) * Grabforce;

        rigidbody.AddForceAtPosition(CurentPos, grabPoint);


    }
}