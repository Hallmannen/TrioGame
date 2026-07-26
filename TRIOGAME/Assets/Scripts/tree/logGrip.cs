using UnityEngine;
using System.Collections.Generic;

public class logGrip : MonoBehaviour
{
    public List<GameObject> players;
    public Rigidbody rigidbody;
    public float PushForce;
    public PlayerManager playerManager;
    void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        foreach (GameObject player in playerManager.Players)
        {
            if (player == null) continue;
            players.Add(player);
        }

        GameObject nerestplayer = GetClosestObject(transform.position, players);

        if (nerestplayer != null)
        {
            Vector3 direction = transform.position - nerestplayer.transform.position;
            direction.y = 0f; // ignore the y so the tree does not fly uppwoard att al!
            direction.Normalize();

            Vector3 TopOfLog = transform.position + transform.up * (transform.localScale.y * 0.5f);

            rigidbody.AddForceAtPosition(direction * PushForce, TopOfLog, ForceMode.Impulse);
        }
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
    GameObject GetClosestObject(Vector3 position, List<GameObject> objects)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            float distance = Vector3.SqrMagnitude(obj.transform.position - position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }
        return closest;
    }
}