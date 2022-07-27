using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    public float speed;

    private Vector3[] positions = new Vector3[397];
    private Vector3[] curvePoints;
    private int nextPoint = 0;
    private bool isGoingBack = false;

    void Start()
    {
        curvePoints = GetLinePointsInWorldSpace();
        gameObject.transform.position = curvePoints[nextPoint];
    }

    public void SetPoints(Vector3[] points)
    {
        curvePoints = points;
    }

    Vector3[] GetLinePointsInWorldSpace()
    {
        lineRenderer.GetPositions(positions);

        return positions;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
            curvePoints[nextPoint],
            speed * Time.deltaTime);

        if (gameObject.transform.position == curvePoints[nextPoint])
        {
            if (isGoingBack)
            {
                nextPoint--;
            }
            else
            {
                nextPoint++;
            }
        }

        if (nextPoint == curvePoints.Length && !isGoingBack)
        {
            nextPoint = curvePoints.Length - 1;
            isGoingBack = true;
        }
        else if (nextPoint == 0 && isGoingBack)
        {
            nextPoint = 1;
            isGoingBack = false;
        }
    }
}
