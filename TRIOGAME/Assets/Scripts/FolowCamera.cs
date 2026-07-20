using UnityEngine;
using System.Collections.Generic;
public class FolowCamera : MonoBehaviour
{
    public Transform player; // The camera will follow this transform (usually the player)
    public Vector3 offset; // Offset from the player's position
    public float transparency = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 Transparentrayoffset = new Vector3(0, 0, 0);
    public float smoothTime = 0.3f;
    private List<MeshRenderer> transparentObjects = new List<MeshRenderer>();
    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // set the alpha value of all the trees the rays from the camera to the player hit
        foreach (MeshRenderer rend in transparentObjects)
        {
            if (rend != null) SetAlpha(rend, false);
        }

        transparentObjects.Clear();

        Vector3 direction = player.position - (transform.position + Transparentrayoffset);
        float rayDistance = Vector3.Distance(player.position, transform.position + Transparentrayoffset);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, rayDistance);
        //Debug.DrawRay(transform.position, direction, Color.red);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform != player)
            {
                MeshRenderer rend = hit.collider.GetComponent<MeshRenderer>();

                if (rend != null)
                {
                    SetAlpha(rend, true);
                    transparentObjects.Add(rend);
                }
            }
        }
    }
    void SetAlpha(MeshRenderer rend, bool isTransparent)
    {
        if (isTransparent)
        {
            rend.material.SetOverrideTag("RenderType", "Transparent");
            rend.material.SetFloat("_Surface", transparency);
        }
        else
        {
            rend.material.SetOverrideTag("RenderType", "Opaque");
            rend.material.SetFloat("_Surface", 0);
        }
    }
}