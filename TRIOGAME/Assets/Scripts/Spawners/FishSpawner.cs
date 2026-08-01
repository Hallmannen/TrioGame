using UnityEngine;
using UnityEngine.InputSystem;
public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fisches;
    public BoxCollider SpawnArea;
    public float shorlineindent = 5f;
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            SpawnFish();
        }
    }
    void SpawnFish()
    {
        GameObject Spawnfish = Fisches[Random.Range(0, Fisches.Length)];

        float randomX = Random.Range(SpawnArea.bounds.min.x + shorlineindent, SpawnArea.bounds.max.x - shorlineindent);
        float randomZ = Random.Range(SpawnArea.bounds.min.z + shorlineindent, SpawnArea.bounds.max.z - shorlineindent);

        Vector3 position = new Vector3(randomX, 1.24f, randomZ);

        Instantiate(Spawnfish, position, Spawnfish.transform.rotation);
    }
}