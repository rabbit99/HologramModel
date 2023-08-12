using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastReflection : MonoBehaviour
{
    private Transform goTransform;
    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 inDirection;
    public int nReflections = 2;
    public float h = 2;
    private int nPoints;

    void Awake()
    {
        goTransform = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(goTransform.position, goTransform.forward);
        Debug.DrawRay(goTransform.position, goTransform.forward * 100, Color.red);
        nPoints = nReflections;
        lineRenderer.SetVertexCount(nPoints);
        lineRenderer.SetPosition(0, goTransform.position);
        for (int i = 0; i <= nReflections; i++)
        {
            if (i == 0)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000))
                {
                    inDirection = Vector3.Reflect(hit.point, hit.normal);
                    ray = new Ray(hit.point, inDirection);
                    Debug.DrawRay(hit.point, hit.normal, Color.green);
                    Debug.DrawRay(hit.point, inDirection, Color.magenta);

                    Vector3 refactionDirection = Refarct(hit.point - goTransform.position, hit.normal, h);
                    Debug.DrawRay(hit.point, refactionDirection, Color.blue);
                    ray = new Ray(hit.point, refactionDirection);
                    //  ray = new Ray(hit.point, refactionDirection);

                    Debug.Log("object name = " + hit.transform.name);
                    if (nReflections == 1)
                    {
                        lineRenderer.SetVertexCount(++nPoints);
                    }
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
            }
            else
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))
                {
                    inDirection = Vector3.Reflect(inDirection, hit.normal);
                    ray = new Ray(hit.point, inDirection);
                    lineRenderer.SetVertexCount(++nPoints);
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
            }

        }
    }

    private Vector3 Refarct(Vector3 I, Vector3 N, float h)
    {
        I = I.normalized;
        N = N.normalized;
        float A = Vector3.Dot(I, N);
        float k = 1.0f - h * h * (1.0f - A * A);
        Vector3 T = h * I - (h * A + Mathf.Sqrt(k)) * N;
        if (k > 0) return T;
        else return Vector3.zero;
    }
}
