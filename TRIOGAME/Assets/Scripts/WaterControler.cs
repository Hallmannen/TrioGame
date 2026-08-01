using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class WaterControler : MonoBehaviour
{
    public float waterForce = 20;
    private Rigidbody rb;
    private Transform logTransform;

    private Vector3 targetPosition;
    private Vector3 forceDir;

    public ParticleSystem waterParticle;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        logTransform = rb.transform;
        waterParticle.GetComponent<ParticleSystem>();
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

            rb.AddForceAtPosition(forceDir, transform.position); //ForceMode.VelocityChange;

            waterParticle.enableEmission = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterParticle.enableEmission = false;
        }
    }
}
