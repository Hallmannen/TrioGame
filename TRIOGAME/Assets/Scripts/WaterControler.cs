using UnityEngine;

public class WaterControler : MonoBehaviour
{
    public float waterForce = 20;
    private Rigidbody logRB;
    private Transform logTransform;

    private Vector3 targetPosition;
    public float targetXOffset;
    private Vector3 forceDir;

    void Awake()
    {
        logRB = GetComponentInParent<Rigidbody>();
        logTransform = logRB.transform;
    }



    void Update()
    {
        targetPosition = new Vector3(logTransform.position.x + targetXOffset, logTransform.position.y, logTransform.position.z);

        forceDir = targetPosition - transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log(this.gameObject.name + " is toching water");

            logRB.AddForceAtPosition(forceDir.normalized * waterForce, transform.position); //ForceMode.VelocityChange;
        }
    }
}
