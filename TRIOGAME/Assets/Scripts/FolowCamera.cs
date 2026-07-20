using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class FolowCamera : MonoBehaviour
{
    public Transform player; // The camera will follow this transform (usually the player)
    public Vector3 offset; // Offset from the player's position
    public float Transparecy = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 Transparentrayoffset = new Vector3(0, 0, 0);
    public float smoothTime = 0.3f;
    private List<Renderer> transparentObjects = new List<Renderer>();
    void LateUpdate()
    {
        

        // set the alpha value of all the trees the rays from the camera to the player hit
        foreach (Renderer rend in transparentObjects)
        {
            if (rend != null) SetAlpha(rend, 1f);
        }

        transparentObjects.Clear();

        Vector3 direction = player.position - (transform.position + Transparentrayoffset);
        float rayDistance = Vector3.Distance(player.position, transform.position + Transparentrayoffset);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, rayDistance);
        Debug.DrawRay(transform.position, direction, Color.red);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform != player)
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();

                if (rend != null)
                {
                    SetAlpha(rend, Transparecy);
                    transparentObjects.Add(rend);
                }
            }
        }
    }
    void SetAlpha(Renderer rend, float alpha)
    {
        Color color = rend.material.color;
        color.a = alpha;
        rend.material.color = color;
    }
}
