using UnityEngine;

public class ForestSpawner : MonoBehaviour
{
    public GameObject TreePrefab;
    public BoxCollider SpawnArea;
    public int AmountOfTrees = 10;
    public LayerMask Ground;
    private readonly float TreeSpawnYoffset = 2.9669368f; // this offset is so the tree is not in the ground when i spawnds a new tree
    void Start()
    {
        SpawnTrees();
    }
    void SpawnTrees()
    {
        Bounds bounds = SpawnArea.bounds;
        Debug.Log(SpawnArea.transform.rotation);

        for (int CurentTree = 0; CurentTree < AmountOfTrees; CurentTree++)
        {
            Vector3 LocalPosition = new Vector3(
                Random.Range(-SpawnArea.size.x / 2f, SpawnArea.size.x / 2f),
                0, // ignore the y axis
                Random.Range(-SpawnArea.size.z / 2f, SpawnArea.size.z / 2f)
            );

            Vector3 WorldPosition = SpawnArea.transform.TransformPoint(LocalPosition + SpawnArea.center);

            SpawnSingleTree(WorldPosition);
        }
    }
    public bool SpawnSingleTree(Vector3 SpwanLocatation)
    {
        Vector3 rayStart = SpwanLocatation + Vector3.up * 50f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, Ground))
        {
            Instantiate(TreePrefab, hit.point + transform.up * TreeSpawnYoffset, TreePrefab.transform.rotation, transform);
            return true;
        }
        else return false;
    }
}
