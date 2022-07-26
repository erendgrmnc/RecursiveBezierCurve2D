using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bezierNode;

    private BezierManager bezierManager;

    private GameObject bezierNodes;

    private GameObject selectedNode = null;
    private Vector3 offset = Vector3.zero;
    private Vector3 mousePosition;

    void Awake()
    {
        if (!bezierNode)
        {
            var errorMessage = "Can't found bezier node.";

            Debug.LogError(errorMessage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBezierNodesParent();
        bezierManager = GameObject.FindGameObjectWithTag("BezierManager").GetComponent<BezierManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLeftMouseInput();
        CheckForRightMouseBtnInput();
    }

    void InitBezierNodesParent()
    {
        bezierNodes = new GameObject("BezierNodes");
    }

    void CheckLeftMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject && targetObject.tag == "BezierCurveNode")
            {
                selectedNode = targetObject.transform.gameObject;
                offset = selectedNode.transform.position - mousePosition;
            }
        }

        if (selectedNode != null)
        {
            MoveNode();
        }

    }

    void CheckForRightMouseBtnInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var isNewNodeSpawnable = bezierManager.IsNodeSpawnable();

            if (isNewNodeSpawnable)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = Camera.main.nearClipPlane;
                var spawnPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                SpawnBezierNode(spawnPosition);
            }
        }
    }

    //TODO: Move to BezierManger (Violation Of Class Responsibility)
    void SpawnBezierNode(Vector3 spawnPosition)
    {
        var newBezierNode = Instantiate(bezierNode, spawnPosition, gameObject.transform.rotation);
        newBezierNode.transform.parent = bezierNodes.transform;
        bezierManager.AddNode(newBezierNode);
    }

    void MoveNode()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedNode.transform.position = mousePosition + offset;
     
        if (Input.GetMouseButtonUp(0) && selectedNode)
        {
            selectedNode = null;
        }
    }
}
