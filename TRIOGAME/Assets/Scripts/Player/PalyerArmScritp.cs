using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public class PalyerArmScritp : MonoBehaviour
{
    public float armLength = 1.2f; // Längden från axel till hand
    [Space]
    [Header("Left Arm")]
    [Space]
    public GameObject leftArm;
    public Vector3 leftArmOffset;
    public Vector3 leftArmOffset_inWater;
    private Vector3 leftArmVelocity;
    private Vector3 directionleftarm;
    private bool leftHandLocked = false;
    [Space]
    [Header("Right Arm")]
    [Space]
    public GameObject rigthtArm;
    public Vector3 rigthArmOffset;
    public Vector3 rigthArmOffset_inWater;
    private Vector3 rightArmVelocity;
    private Vector3 directionrigtharm;
    private bool rightHandLocked = false;
    [Space]
    public PlayerGraber playerGraber;
    [Space]
    [Header("Smooth")]
    [Space]
    public float positionSmoothTime = 0.08f;
    public float rotationSpeed = 12f;
    public float lockDistance = 0.02f; // 2 cm
    private bool wasGrabbing = false;
    [Space]
    public Player_Movement playerMovment;
    void Update()
    {
        Vector3 trueLeftArmOffset;
        Vector3 trueRigthArmOffset;

        if (playerMovment.inWater)
        {
            trueLeftArmOffset = leftArmOffset_inWater;
            trueRigthArmOffset = rigthArmOffset_inWater;
        }
        else
        {
            trueLeftArmOffset = leftArmOffset;
            trueRigthArmOffset = rigthArmOffset;
        }

        Vector3 leftshoulderPos = transform.TransformPoint(trueLeftArmOffset);
        Vector3 rightShoulderPos = transform.TransformPoint(trueRigthArmOffset);

        if (playerGraber.isGrabbing && !wasGrabbing)
        {
            leftHandLocked = false;
            rightHandLocked = false;
        }

        wasGrabbing = playerGraber.isGrabbing;

        if (!playerGraber.isGrabbing)
        {
            Quaternion idleRot = Quaternion.LookRotation(transform.forward);
            // Left arm
            leftArm.transform.position = Vector3.SmoothDamp(leftArm.transform.position, leftshoulderPos, ref leftArmVelocity, positionSmoothTime * Time.deltaTime);

            leftArm.transform.rotation = Quaternion.Slerp(leftArm.transform.rotation, idleRot, rotationSpeed * Time.deltaTime);

            // Rigth arm
            rigthtArm.transform.position = Vector3.SmoothDamp(rigthtArm.transform.position, rightShoulderPos, ref rightArmVelocity, positionSmoothTime * Time.deltaTime);

            rigthtArm.transform.rotation = Quaternion.Slerp(rigthtArm.transform.rotation, idleRot, rotationSpeed * Time.deltaTime);
        }
        else // this is if the player is grabing
        {
            Vector3 handPos = playerGraber.worldGrabPoint;

            directionleftarm = (handPos - leftshoulderPos).normalized;
            directionrigtharm = (handPos - rightShoulderPos).normalized;

            // TargetRotation
            Quaternion leftTargetRot = Quaternion.LookRotation(directionleftarm);
            Quaternion rightTargetRot = Quaternion.LookRotation(directionrigtharm);

            // TargetPosition
            Vector3 leftTargetPos = handPos - directionleftarm * armLength;
            Vector3 rightTargetPos = handPos - directionrigtharm * armLength;

            Debug.DrawLine(leftTargetPos, leftshoulderPos, Color.red);
            Debug.DrawLine(rightTargetPos, rightShoulderPos, Color.red);

            // Left arm
            if (!leftHandLocked)
            {
                leftArm.transform.position = Vector3.SmoothDamp(leftArm.transform.position, leftTargetPos, ref leftArmVelocity, positionSmoothTime * Time.deltaTime);

                if (Vector3.Distance(leftArm.transform.position, leftTargetPos) < lockDistance)
                {
                    leftHandLocked = true;
                }
            }
            else
            {
                leftArm.transform.position = leftTargetPos;
            }

            leftArm.transform.rotation = Quaternion.Slerp(leftArm.transform.rotation, leftTargetRot, rotationSpeed * Time.deltaTime);

            // Right arm
            if (!rightHandLocked)
            {
                rigthtArm.transform.position = Vector3.SmoothDamp(rigthtArm.transform.position, rightTargetPos, ref rightArmVelocity, positionSmoothTime * Time.deltaTime);

                if (Vector3.Distance(rigthtArm.transform.position, rightTargetPos) < lockDistance)
                {
                    rightHandLocked = true;
                }
            }
            else
            {
                rigthtArm.transform.position = rightTargetPos;
            }

            rigthtArm.transform.rotation = Quaternion.Slerp(rigthtArm.transform.rotation, rightTargetRot, rotationSpeed * Time.deltaTime);
        }
    }
}