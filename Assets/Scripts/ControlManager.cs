using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    private BezierManager bezierManager;

    private GameObject selectedNode = null;
    private Vector3 offset = Vector3.zero;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        bezierManager = GameObject.FindGameObjectWithTag("BezierManager").GetComponent<BezierManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLeftMouseInput();
        CheckForRightMouseBtnInput();
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
                bezierManager.SpawnBezierNode(spawnPosition);
            }
        }
    }

    void MoveNode()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedNode.transform.position = mousePosition + offset;
     
        if (Input.GetMouseButtonUp(0) && selectedNode != null)
        {
            selectedNode = null;
        }
    }
}
