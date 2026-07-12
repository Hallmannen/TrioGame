using UnityEngine;

public class tree : MonoBehaviour
{
    public Transform[] Players;

    public float choopRange = 1;
    public Transform treeStump;

    public GameObject treeStumpObj;
    public GameObject falingTreeObj;

    void Start()
    {
    }

    void Update()
    {
        RaycastHit[] Hits = Physics.SphereCastAll(treeStump.position, choopRange, Vector3.zero);
        Debug.DrawRay(treeStump.position, new Vector3(choopRange, 0, 0));
        Debug.DrawRay(treeStump.position, new Vector3(-choopRange, 0, 0));
        Debug.DrawRay(treeStump.position, new Vector3(0, 0, choopRange));
        Debug.DrawRay(treeStump.position, new Vector3(0, 0, -choopRange));


        for (int i = 0; i < Hits.Length; i++)
        {
            if (Hits[i].collider.CompareTag("Player"))
            {
                choopTree();
            }
        }

    }

    public void choopTree()
    {
        Debug.Log("Chooping down tree!");
    }
}
