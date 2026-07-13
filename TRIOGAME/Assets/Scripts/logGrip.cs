using UnityEngine;

public class logGrip : MonoBehaviour
{
    public Rigidbody rigidbody;
    public bool ShuldAddForce = false;
    public void OnPlayerHoldingTree(float Grabforce, Vector3 world_TargetPosition, Vector3 GrabPostition)
    {
        Vector3 CurentPos = (world_TargetPosition - GrabPostition) * Grabforce;

        rigidbody.AddForceAtPosition(CurentPos, GrabPostition);
    }
}