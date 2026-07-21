using UnityEngine;
using System.Collections.Generic;

public class NewMultiTargetCamera : MonoBehaviour
{
    public List<Transform> Targets;
    public Vector3 offset;

    private Vector3 velocity;

    public float minZoom;
    public float maxZoom;
    public float zoomLimiter = 50f;
    public float camSmothnes = .5f;

    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Move();

        float newZoom = Mathf.Lerp(minZoom, maxZoom, getGreatestDistans() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }
    void Move()
    {
        Vector3 centerPoint = getCenterPoint();
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, camSmothnes);
    }

    float getGreatestDistans()
    {
        var bounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].position);
        }

        return bounds.size.x;
    }

    Vector3 getCenterPoint()
    {
        if (Targets.Count == 1)
        {
            return Targets[0].position;
        }

        var bounds = new Bounds(Targets[0].position, Vector3.zero);

        for (int i = 0; i < Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].position);
        }
        return bounds.center;
    }
}
