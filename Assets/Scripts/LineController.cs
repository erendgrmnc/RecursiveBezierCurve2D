using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] points;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            AlignLinePosition();
        }
    }

    public void SetUpLine(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;
        this.points = points;
    }

    public void AlignLinePosition()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (lineRenderer != null && points[i] != null)
            {
                lineRenderer.SetPosition(i, points[i].transform.position);
            }
        }
    }
}
