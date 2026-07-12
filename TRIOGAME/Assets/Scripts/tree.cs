using UnityEngine;

public class tree : MonoBehaviour
{

    public float choopRange = 1;
    public Transform treeStump;

    public GameObject treeStumpObj;
    public GameObject falingTreeObj;

    void Start()
    {
    }

    void Update()
    {
        //RaycastHit[] Hits = Physics.SphereCastAll(treeStump.position, choopRange, new Vector3(0, -3, 0));

        /*
        Debug.DrawRay(transform.position, new Vector3(choopRange, 0, 0));
        Debug.DrawRay(transform.position, new Vector3(-choopRange, 0, 0));
        Debug.DrawRay(transform.position, new Vector3(0, 0, choopRange));
        Debug.DrawRay(transform.position, new Vector3(0, 0, -choopRange));
        */

        // sending out 8 raycast and checking for nerby players
        for (int i = 0; i < 8; i++)
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

    private float TreeHp = 100;

    public void choopTree()
    {
        if (TreeHp <= 0)
        {
            Destroy(this.gameObject);
            Debug.Log("Chooping down tree! ");
        }
        else
        {
            TreeHp -= Time.deltaTime * 100f;
        }

    }

    private void OnDestroy()
    {
        Instantiate(treeStumpObj, treeStump.position, Quaternion.Euler(0, 45, 0));
        Instantiate(falingTreeObj, transform.position + transform.up, Quaternion.Euler(0, 45, 0));

    }

}
