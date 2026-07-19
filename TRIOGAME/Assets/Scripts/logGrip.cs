using UnityEngine;

public class logGrip : MonoBehaviour
{
    public GameObject Player = null;
    public Rigidbody rigidbody;
    public float PushForce = 10;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        Vector3 direction = transform.position - Player.transform.position;
        direction.y = 0f; // ignore the y so the tree does not fly uppwoard att al!
        direction.Normalize();

        Vector3 TopOfLog = transform.position + transform.up * (transform.localScale.y * 0.5f);

        rigidbody.AddForceAtPosition(direction * PushForce, TopOfLog, ForceMode.Impulse);
    }
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