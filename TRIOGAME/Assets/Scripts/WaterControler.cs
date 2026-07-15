using UnityEngine;

public class WaterControler : MonoBehaviour
{
    public float waterForce = 20;
    private Rigidbody logRB;
    private Transform logTransform;

    private Vector3 targetPosition;
    private Vector3 forceDir;

    void Awake()
    {
        logRB = GetComponentInParent<Rigidbody>();
        logTransform = logRB.transform;
    }
    void Update()
    {
        if (Water == null) return;

        Vector3 LocalLogPosition = Water.InverseTransformPoint(logTransform.position);

        LocalLogPosition.x += 1;
        LocalLogPosition.z = 0f;

        targetPosition = Water.TransformPoint(LocalLogPosition);

        forceDir = waterForce * (targetPosition - transform.position).normalized;
    }

    private Transform Water;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Water = other.gameObject.transform;

            logRB.AddForceAtPosition(forceDir, transform.position); //ForceMode.VelocityChange;
        }
    }
}
