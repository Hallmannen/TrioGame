using UnityEditor.Timeline;
using UnityEngine;

public class PalyerArmScritp : MonoBehaviour
{
    public float armLength = 1.2f; // Längden från axel till hand
    [Space]
    public GameObject leftArm;
    public Vector3 leftArmGrabingpointoffset;
    public Vector3 leftArmOffset;
    private Vector3 directionleftarm;
    [Space]
    [Space]
    public GameObject rigthtArm;
    public Vector3 rigthArmGrabingpointoffset;
    public Vector3 rigthArmOffset;
    private Vector3 directionrigtharm;
    [Space]
    [Space]
    public PlayerGraber playerGraber;
    void Update()
    {
        Vector3 leftshoulderPos = transform.TransformPoint(leftArmOffset);
        Vector3 rightShoulderPos = transform.TransformPoint(rigthArmOffset);

        if (!playerGraber.isGrabbing)
        {
            leftArm.transform.SetPositionAndRotation(leftshoulderPos, Quaternion.LookRotation(transform.forward));
            rigthtArm.transform.SetPositionAndRotation(rightShoulderPos, Quaternion.LookRotation(transform.forward));
        }
        else
        {
            Vector3 handPos = playerGraber.worldGrabPoint;
            directionleftarm = (handPos - leftshoulderPos).normalized;
            directionrigtharm = (handPos - rightShoulderPos).normalized;

            leftArm.transform.rotation = Quaternion.LookRotation(directionleftarm);
            rigthtArm.transform.rotation = Quaternion.LookRotation(directionrigtharm);

            // Flytta armen så att handen hamnar på grabbpunkten
            leftArm.transform.position = handPos - directionleftarm * armLength;
            rigthtArm.transform.position = handPos - directionrigtharm * armLength;
        }
    }
}