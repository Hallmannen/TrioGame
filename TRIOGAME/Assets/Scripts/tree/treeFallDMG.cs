using UnityEngine;

public class treeFallDMG : MonoBehaviour
{
    public GameObject groundPartical;
    public GameObject treeHitPartical;
    public GameObject playerHitPartical;


    private CameraShake cameraShake;
    private void OnEnable()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && enabled)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 point = contact.point;
            GameObject newPartical = Instantiate(groundPartical, point, Quaternion.identity);
            Destroy(newPartical, 6);
            StartCoroutine(cameraShake.Shake(.1f, .1f));
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
            StartCoroutine(cameraShake.Shake(.1f, .1f));
        }
        if (collision.collider.CompareTag("Player") && enabled)
        {

        }


    }

}
