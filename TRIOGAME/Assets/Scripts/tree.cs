using UnityEngine;

public class Tree : MonoBehaviour
{
    public float choopRange = 1;
    public int RayAmounts = 8;
    public Transform treeStump;
    public GameObject treeStumpObj;
    public GameObject falingTreeObj;
    private readonly float TreeSpawnYoffset = 2.9669368f; // this offset is so the tree is not in the ground when i spawnds a new tree
    public float TreeHp = 100;
    [HideInInspector]
    void Start()
    {
        transform.position += Vector3.up * TreeSpawnYoffset;
    }
    void Update()
    {
        for (int i = 0; i < RayAmounts; i++)
        {
            float angle = i * 45f * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            Debug.DrawRay(treeStump.position, dir * choopRange, Color.red);

            if (Physics.Raycast(treeStump.position, dir, out RaycastHit hit, choopRange))
            {
                //Debug.Log(hit.collider.name + " Is chooping down a tree!!");
                if (hit.collider.CompareTag("Player"))
                {
                    choopTree();
                }
            }
        }
    }
    public void choopTree()
    {
        if (TreeHp <= 0)
        {
            Destroy(this.gameObject);
            SpawnTreeparts();
            Debug.Log("Chooping down tree! ");
        }
        else
        {
            TreeHp -= Time.deltaTime * 100f;
        }

    }
    void SpawnTreeparts()
    {
        Instantiate(treeStumpObj, treeStump.position, Quaternion.Euler(0, 45, 0));
        Instantiate(falingTreeObj, transform.position + transform.up, Quaternion.Euler(0, 45, 0));
    }
}
