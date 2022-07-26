using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierManager : MonoBehaviour
{
    [SerializeField]
    private int maxNode;

    [SerializeField]
    private int lineDensity;

    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private LineRenderer curveLinePrefab;

    private List<GameObject> bezierNodesOnScene;
    private GameObject lastAddedNode;

    private const int minNodesToDrawLines = 2;
    private const int minQuadraticBezierNode = 4;
    private Vector3[] bezierCurvePointPositions;

    void Awake()
    {
        bezierNodesOnScene = new List<GameObject>();
        bezierCurvePointPositions = new Vector3[lineDensity];
    }

    // Start is called before the first frame update
    void Start()
    {
        curveLinePrefab.positionCount = lineDensity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNode(GameObject node)
    {
        bezierNodesOnScene.Add(node);
        var nodeCount = bezierNodesOnScene.Count;

        if (nodeCount >= minNodesToDrawLines)
        {
            DrawLinesBetweenNodes(lastAddedNode.transform, node.transform);
        }

        HandleBezierCurve();

        lastAddedNode = node;
    }

    public void HandleBezierCurve()
    {
        int nodeCount = bezierNodesOnScene.Count;

        if (nodeCount == 2)
        {
            DrawLineerBezierCurve();
        }

        if (nodeCount == minQuadraticBezierNode)
        {
            DrawQuadraticBezierCurve();
        }
    }

    void DrawLinesBetweenNodes(Transform firstNodeTransform, Transform secondNodeTransform)
    {
        Transform[] points = new Transform[2];
        points[0] = firstNodeTransform;
        points[1] = secondNodeTransform;

        var newLine = Instantiate(linePrefab, gameObject.transform.position, gameObject.transform.rotation);
        newLine.SetActive(false);
        newLine.GetComponent<LineController>().SetUpLine(points);
        newLine.SetActive(true);
    }

    public bool IsNodeSpawnable()
    {
        return bezierNodesOnScene.Count < maxNode;
    }

    void DrawLineerBezierCurve()
    {
        for (int i = 0; i < lineDensity; i++)
        {
            float tValue = (i + 1) / (float)lineDensity;
            bezierCurvePointPositions[i] = CalculateLineerBezierPoint(tValue, bezierNodesOnScene[0].transform.position, bezierNodesOnScene[1].transform.position);
        }

        HandleLineRenderer();
    }

    void DrawQuadraticBezierCurve()
    {
        for (int i = 0; i < lineDensity; i++)
        {
            float tValue = (i + 1) / (float)lineDensity;
            bezierCurvePointPositions[i] = CalculateQuadraticLineerBezierPoint(tValue, bezierNodesOnScene[0].transform.position, bezierNodesOnScene[1].transform.position, bezierNodesOnScene[2].transform.position, bezierNodesOnScene[3].transform.position);
        }

        HandleLineRenderer();
    }
    Vector3 CalculateLineerBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    Vector3 CalculateQuadraticLineerBezierPoint(float t, Vector3 startPoint, Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 endPoint)
    {
        float oneMinusT = 1 - t;
        return Mathf.Pow(oneMinusT, 3) * startPoint +
               3 * t * Mathf.Pow(oneMinusT, 2) * firstControlPoint +
               3 * Mathf.Pow(t, 2) * oneMinusT * secondControlPoint +
               Mathf.Pow(t, 3) * endPoint;
    }

    void HandleLineRenderer()
    {
        if (bezierCurvePointPositions != null)
        {
            curveLinePrefab.SetPositions(bezierCurvePointPositions);
        }
    }
}
