using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private int nextPoint = 0;
    private const int minSpeed = 1;
    private const int maxSpeed = 50;
    
    private bool isGoingBack = false;

    private LineRenderer lineRenderer;

    private Vector3[] positions = new Vector3[397];
    private Vector3[] curvePoints;

    void FixedUpdate()
    {
        if (curvePoints.Length > 0)
        {
            Move();
        }
    }

    public void SetPoints(Vector3[] points)
    {
        curvePoints = points;
    }

    public void SetLineRenderer(LineRenderer lineRenderer)
    {
        this.lineRenderer = lineRenderer;
        curvePoints = GetLinePointsInWorldSpace();
    }

    Vector3[] GetLinePointsInWorldSpace()
    {
        lineRenderer.GetPositions(positions);
        return positions;
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

            if (nextPoint >= 0 && nextPoint < curvePoints.Length)
            {
                RotateToNextPoint(curvePoints[nextPoint]);
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

    public void SetCourseForPathFinder(Vector3[] path)
    {
        var pathFinder = GameObject.FindGameObjectWithTag(Constants.Tags.PathFinder);
        if (pathFinder != null)
        {
            pathFinder.GetComponent<PathFinder>().SetPoints(path);
        }
    }

    void RotateToNextPoint(Vector3 target)
    {
        var offset = 90f;
        Vector2 direction = target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.back * (angle + offset));
    }

    public void ResetCourse()
    {
        nextPoint = 0;
        isGoingBack = false;
        var curvePoints = GameObject.FindGameObjectWithTag(Constants.Tags.BezierManager).GetComponent<BezierManager>()
            .GetBezierCurvePoints();

        SetCourseForPathFinder(curvePoints);

        if (curvePoints.Length > 0)
        {
            gameObject.transform.position = curvePoints[nextPoint];
        }
    }

    public void IncreaseSpeed()
    {
        if (speed < maxSpeed)
        {
            
        }
        speed++;
    }

    public void DecreaseSpeed()
    {
        if (speed > minSpeed)
        {
            speed--;
        }
    }
}
