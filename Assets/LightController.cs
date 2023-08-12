using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxIterations = 5;
    public float maxLength = 10f;
    public Vector3 glassNormal = new Vector3(0.0f, 0.0f, 1.0f);
    public float refractiveIndex = 1.5f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        Vector3 incidentDirection = transform.forward;
        Vector3 reflectionDirection = Vector3.Reflect(incidentDirection, glassNormal).normalized;
        Vector3 refractionDirection = Refract(incidentDirection, glassNormal, refractiveIndex).normalized;

        Vector3 rayStart = transform.position;
        Vector3 rayEnd = rayStart + reflectionDirection * maxLength;

        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        for (int i = 0; i < maxIterations; i++)
        {
            rayStart = rayEnd;
            incidentDirection = reflectionDirection;

            reflectionDirection = Vector3.Reflect(incidentDirection, glassNormal).normalized;
            rayEnd = rayStart + refractionDirection * maxLength;

            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(i + 2, rayEnd);

            // 如果光线不再发生折射，则退出循环
            if (refractionDirection == Vector3.zero)
                break;
        }
    }

    // 自定义的折射方法
    private Vector3 Refract(Vector3 incident, Vector3 normal, float refractiveIndex)
    {
        float cosTheta1 = -Vector3.Dot(incident, normal);
        float eta = 1f / refractiveIndex;

        Vector3 refracted = eta * incident + (eta * cosTheta1 - Mathf.Sqrt(1f - eta * eta * (1f - cosTheta1 * cosTheta1))) * normal;

        return refracted;
    }
}
