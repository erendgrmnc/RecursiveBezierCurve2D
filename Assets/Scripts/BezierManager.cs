using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class BezierManager : MonoBehaviour
{
    [SerializeField]
    private int maxNode;

    [SerializeField]
    private int lineDensity;

    private int lastPlanetIndex;

    [SerializeField]
    private GameObject bezierNode;
    [SerializeField]
    private GameObject linePrefab;

    public GameObject curveLinePrefab;

    private GameObject bezierNodes;
    private GameObject lastAddedNode;
    private GameObject linesBetweenNodes;
    private List<GameObject> bezierNodesOnScene;

    private LineRenderer curveLineRenderer;

    private SceneManager sceneManager;

    private const int minNodesToDrawLines = 2;
    private Vector3[] bezierCurvePointPositions;

    [SerializeField]
    private Sprite[] planetSprites;

    [SerializeField]
    private Vector3[] qubicBezierCurveInitNodes;

    void Awake()
    {
        bezierNodesOnScene = new List<GameObject>();
        bezierCurvePointPositions = new Vector3[lineDensity];
        InitBezierNodesParent();
        InitLinesBetweenNodeObject();
        InitSceneManager();
    }

    void Start()
    {
        CreateCurveLineRenderer();
    }

    void Update()
    {
        HandleBezierCurve();
    }

    public int GetSpawnedBezierNode()
    {
        return bezierNodesOnScene.Count;
    }

    public int GetMinBezierNodesToDrawCurve()
    {
        return minNodesToDrawLines;
    }

    public bool IsNodeSpawnable()
    {
        return bezierNodesOnScene.Count < maxNode;
    }

    public bool IsLinesBetweenNodesShowing()
    {
        return linesBetweenNodes.activeSelf;
    }

    public bool CanCurveDrawable()
    {
        int spawnedBezierNodes = GetSpawnedBezierNode();
        int minNodesToDrawCurve = GetMinBezierNodesToDrawCurve();

        return spawnedBezierNodes >= minNodesToDrawCurve;
    }

    Sprite GetRandomPlanetSprite()
    {
        int randomIndex = 0;
        do
        {
            randomIndex = new Random().Next(0, planetSprites.Length);

        }
        while (randomIndex == lastPlanetIndex);

        lastPlanetIndex = randomIndex;
        return planetSprites[randomIndex];
    }

    void InitBezierNodesParent()
    {
        bezierNodes = new GameObject(Constants.GameObjectNames.BezierNodes);
    }

    void InitLinesBetweenNodeObject()
    {
        linesBetweenNodes = new GameObject(Constants.GameObjectNames.LinesBetweenBezierNodes);
    }

    void InitSceneManager()
    {
        var sceneManagerObj = GameObject.FindGameObjectWithTag(Constants.Tags.SceneManager);
        if (sceneManagerObj != null)
        {
            sceneManager = sceneManagerObj.GetComponent<SceneManager>();
        }
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

        sceneManager.ToggleSpawnSpaceShipButton(CanCurveDrawable());
        sceneManager.ToggleLineBetweenNodesButtonInteractibility(bezierNodesOnScene.Count > 2);
    }

    public void HandleBezierCurve()
    {
        int nodeCount = bezierNodesOnScene.Count;

        if (nodeCount >= 2)
        {
            DrawBezierCurve();
        }
    }
    public void SpawnBezierNode(Vector3 spawnPosition)
    {
        var newBezierNode = Instantiate(bezierNode, spawnPosition, gameObject.transform.rotation);
        newBezierNode.transform.parent = bezierNodes.transform;
        var bezierNodeSpriteRenderer = newBezierNode.GetComponent<SpriteRenderer>();
        var spriteSize = bezierNodeSpriteRenderer.size;
        bezierNodeSpriteRenderer.sprite = GetRandomPlanetSprite();
        bezierNodeSpriteRenderer.size = spriteSize;
        AddNode(newBezierNode);
    }

    void DrawLinesBetweenNodes(Transform firstNodeTransform, Transform secondNodeTransform)
    {
        Transform[] points = new Transform[2];
        points[0] = firstNodeTransform;
        points[1] = secondNodeTransform;

        var newLine = Instantiate(linePrefab, gameObject.transform.position, gameObject.transform.rotation);
        newLine.transform.parent = linesBetweenNodes.transform;
        newLine.SetActive(false);
        newLine.GetComponent<LineController>().SetUpLine(points);
        newLine.SetActive(true);
    }

    void DrawBezierCurve()
    {
        var controlPoints = new Vector3[bezierNodesOnScene.Count];

        for (int i = 0; i < bezierNodesOnScene.Count; i++)
        {
            if (bezierNodesOnScene[i] != null)
            {
                controlPoints[i] = bezierNodesOnScene[i].transform.position;
            }
        }

        var controlPointsSpan = new Span<Vector3>(controlPoints);

        for (int i = 0; i < lineDensity; i++)
        {
            float tValue = (i + 1) / (float)lineDensity;
            bezierCurvePointPositions[i] = CalculateRecursiveBezierPoint(controlPoints, tValue);
        }

        HandleLineRenderer();
    }

    Vector3 CalculateRecursiveBezierPoint(ReadOnlySpan<Vector3> bezierControlPoints, float tValue)
    {
        if (bezierControlPoints.Length == 1)
            return bezierControlPoints[0];

        var firstBezierNode = CalculateRecursiveBezierPoint(bezierControlPoints[0..^1], tValue);
        var secondBezierNode = CalculateRecursiveBezierPoint(bezierControlPoints[1..], tValue);
        var oneMinusT = 1 - tValue;
        return new Vector3(oneMinusT * firstBezierNode.x + tValue * secondBezierNode.x, oneMinusT * firstBezierNode.y + tValue * secondBezierNode.y, firstBezierNode.z);
    }

    void HandleLineRenderer()
    {
        if (bezierCurvePointPositions != null)
        {
            curveLineRenderer.SetPositions(bezierCurvePointPositions);

            var pathFinder = GameObject.FindGameObjectWithTag(Constants.Tags.PathFinder);
            if (pathFinder != null)
            {
                pathFinder.GetComponent<PathFinder>().SetCourseForPathFinder(bezierCurvePointPositions);
            }
        }
    }

    public void ToggleVisibiltyOfLinesBetweenNodes()
    {
        linesBetweenNodes.SetActive(!linesBetweenNodes.activeSelf);
    }

    public Vector3[] GetBezierCurvePoints()
    {
        return bezierCurvePointPositions;
    }

    public void CreateQubicBezierCurve()
    {
        if (linesBetweenNodes.transform.GetComponentsInChildren<Transform>().Length > 0)
        {
            DestroyLines();
        }

        if (bezierNodesOnScene.Count > 0)
        {
            DestroyNodes();
        }
        
        DestroyCurveLineRenderer();
        CreateCurveLineRenderer();

        sceneManager.DisableShipAndShipButton();

        if (qubicBezierCurveInitNodes.Length == 4)
        {
            foreach (var node in qubicBezierCurveInitNodes)
            {
                SpawnBezierNode(node);
            }
        }
    }

    private void DestroyNodes()
    {
        foreach (var bezierNode in bezierNodesOnScene)
        {
            Destroy(bezierNode.gameObject);
        }

        bezierNodesOnScene.Clear();
    }

    private void DestroyLines()
    {
        foreach (Transform child in linesBetweenNodes.transform)
        {
            if (child.tag == Constants.Tags.Line)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void DestroyCurveLineRenderer()
    {
        var currentCurveLineRendererObj = GameObject.FindGameObjectWithTag(Constants.Tags.CurveLineRenderer);
        if (currentCurveLineRendererObj != null)
        {
            Destroy(currentCurveLineRendererObj);
        }
    }

    private void CreateCurveLineRenderer()
    {
        var curveLineObject = Instantiate(curveLinePrefab, curveLinePrefab.transform.position, curveLinePrefab.transform.rotation);
        curveLineRenderer = curveLineObject.GetComponent<LineRenderer>();
        InitCurveLineRenderer();

        var pathFinder = GameObject.FindGameObjectWithTag(Constants.Tags.PathFinder);
        if (pathFinder)
        {
            pathFinder.GetComponent<PathFinder>().SetLineRenderer(curveLineRenderer);
        }
    }

    void InitCurveLineRenderer()
    {
        curveLineRenderer.positionCount = lineDensity;
    }
}
