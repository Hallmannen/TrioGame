using UnityEngine;
using System.Collections.Generic;

public class FolowCamera : MonoBehaviour
{
    public Transform player; // The camera will follow this transform (usually the player)
    public float Transparecy = 0.5f;
    public Vector3 Transparentrayoffset = new Vector3(0, 0, 0);
    public float smoothTime = 0.3f;
    private List<Renderer> transparentObjects = new List<Renderer>();
    void LateUpdate()
    {
        // set the alpha value of all the trees the rays from the camera to the player hit
        foreach (Renderer rend in transparentObjects)
        {
            if (rend != null) SetAlpha(rend.material, false);
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
                    SetAlpha(rend.material, true);
                    transparentObjects.Add(rend);
                }
            }
        }
    }
    void SetAlpha(Material rendmat, bool isTransparant)
    {
        if (isTransparant)
        {
            rendmat.SetFloat("_Surface", 1);
            rendmat.SetOverrideTag("RenderType", "Transparent");
            rendmat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            rendmat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            rendmat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            rendmat.SetInt("_ZWrite", 0);
            rendmat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            Color color = rendmat.color;
            color.a = Transparecy;
            rendmat.color = color;
        }
        else
        {
            rendmat.SetFloat("_Surface", 0);
            rendmat.SetOverrideTag("RenderType", "Opaque");
            rendmat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            rendmat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            rendmat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            rendmat.SetInt("_ZWrite", 1);
            rendmat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            Color color = rendmat.color;
            color.a = 1f;
            rendmat.color = color;
        }
    }
}
