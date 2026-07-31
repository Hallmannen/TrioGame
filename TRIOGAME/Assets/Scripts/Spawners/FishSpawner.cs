using System.Collections.Generic;
using UnityEngine;
public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fisches;
    public BoxCollider SpawnArea;
    public float shorlineindent = 5f;

    void Start()
    {
        GameObject Spawnfish = Fisches[Random.Range(0, Fisches.Length)];

        Vector3 position = new Vector3(SpawnArea.size.x, 1.24f, SpawnArea.size.y);

        Instantiate(Spawnfish, position, Spawnfish.transform.rotation);
    }
}