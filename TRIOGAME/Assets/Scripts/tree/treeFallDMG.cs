using UnityEngine;

public class treeFallDMG : MonoBehaviour
{
    public GameObject groundPartical;
    public GameObject treeHitPartical;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && enabled)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            GameObject newPartical = Instantiate(groundPartical, point, Quaternion.identity);
            Destroy(newPartical, 6);
            enabled = false;
        }
        if (collision.collider.CompareTag("Tree") && enabled)
        {
            Tree tree = collision.collider.GetComponent<Tree>();
            //tree.TreeHp = 0; tree.choopTree();
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            GameObject newPartical = Instantiate(treeHitPartical, point, Quaternion.identity);
            Destroy(newPartical, 6);
        }

    }

}
