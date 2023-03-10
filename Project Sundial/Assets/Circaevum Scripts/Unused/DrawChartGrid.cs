using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawChartGrid : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Vector3[] coordinates;
    public float radius = 0.001f;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        for (int i = 0; i < 45; i += 3)
        {
            lineRenderer.SetPosition(i, new Vector3(0, i / 3, 0));
            lineRenderer.SetPosition(i + 1, new Vector3(0, i / 3, 15));
            lineRenderer.SetPosition(i + 2, new Vector3(0, i / 3, 0));
        }
        lineRenderer.SetPosition(45, new Vector3(0, 0, 0));
        for (int i = 0; i < 45; i += 3)
        {
            lineRenderer.SetPosition(i+45, new Vector3(0, 0, i / 3));
            lineRenderer.SetPosition(i + 46, new Vector3(0, 15, i / 3));
            lineRenderer.SetPosition(i + 47, new Vector3(0, 0, i / 3));
        }
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }
    void Update()
    {
    }
}
