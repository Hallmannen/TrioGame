using UnityEngine;

public class treeFallDMG : MonoBehaviour
{
    public GameObject groundPartical;
    public GameObject treeHitPartical;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground") && this.enabled)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            GameObject newPartical = Instantiate(groundPartical, point, Quaternion.identity);
            Destroy(newPartical, 6);
            this.enabled = false;
        }
        if (collision.collider.CompareTag("Tree") && this.enabled)
        {
            Tree T = collision.collider.GetComponent<Tree>();
            T.TreeHp = 0; T.choopTree();
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            GameObject newPartical = Instantiate(treeHitPartical, point, Quaternion.identity);
            Destroy(newPartical, 6);
        }

    }

}
