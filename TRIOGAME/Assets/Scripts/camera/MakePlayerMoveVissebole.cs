using UnityEngine;
using System.Collections.Generic;

public class MakePlayerMoveVissebole : MonoBehaviour
{
    public List<Transform> players;
    public float Transparecy = 0.5f;
    public float TransparentrayYoffset = 2f;
    private readonly List<Renderer> transparentObjects;
    void LateUpdate()
    {
        // set the alpha value of all the trees the rays from the camera to the player hit
        foreach (Renderer rend in transparentObjects)
        {
            if (rend != null) SetAlpha(rend.material, false);
        }

        transparentObjects.Clear();

        foreach (Transform ThisPlayer in players)
        {
            Vector3 rayOrigin = transform.position;
            Vector3 direction = (ThisPlayer.position + new Vector3(0, TransparentrayYoffset, 0) - rayOrigin).normalized;
            float rayDistance = Vector3.Distance(ThisPlayer.position, rayOrigin);

            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, direction, rayDistance);

            Debug.DrawRay(rayOrigin, direction * rayDistance, Color.black);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform != ThisPlayer)
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
}
