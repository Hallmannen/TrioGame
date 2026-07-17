using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] TreePrefab;
    public GameObject[] GrassPrefabs;
    public GameObject[] StonePrefabs;
    public BoxCollider SpawnArea;
    public int Treeamount = 1;
    public int Grassamount = 1;
    public int Stoneamount = 1;
    public LayerMask Ground;
    void Start()
    {
        SpawnTrees();
        SpawnGrass();
    }
    void SpawnTrees()
    {
        for (int i = 0; i < Treeamount; i++)
        {
            Vector3 LocalPosition = new Vector3(
                Random.Range(-SpawnArea.size.x / 2f, SpawnArea.size.x / 2f),
                0, // ignore the y axis
                Random.Range(-SpawnArea.size.z / 2f, SpawnArea.size.z / 2f)
            );

            Vector3 WorldPosition = SpawnArea.transform.TransformPoint(LocalPosition + SpawnArea.center);

            int TreeVerition = Random.Range(0, TreePrefab.Length);

            SpawnSinglAssetatposition(WorldPosition, TreePrefab[TreeVerition]);
        }
    }
    void SpawnGrass()
    {
        for (int i = 0; i < Grassamount; i++)
        {
            Vector3 LocalPosition = new Vector3(
                Random.Range(-SpawnArea.size.x / 2f, SpawnArea.size.x / 2f),
                0, // ignore the y axis
                Random.Range(-SpawnArea.size.z / 2f, SpawnArea.size.z / 2f)
            );

            Vector3 WorldPosition = SpawnArea.transform.TransformPoint(LocalPosition + SpawnArea.center);

            int TreeVerition = Random.Range(0, TreePrefab.Length);

            SpawnSinglAssetatposition(WorldPosition, GrassPrefabs[TreeVerition]);
        }
    }
    public bool SpawnSinglAssetatposition(Vector3 SpwanLocatation, GameObject Asset)
    {
        Vector3 rayStart = SpwanLocatation + Vector3.up * 50f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, Ground))
        {
            Instantiate(Asset, hit.point, Asset.transform.rotation);
            return true;
        }
        else return false;
    }
}
