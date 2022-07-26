using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bezierNode;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLeftMouseInput();
    }

    void CheckLeftMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            var spawnPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            OnLeftMouseButtonClick(spawnPosition);
        }
    }

    void OnLeftMouseButtonClick(Vector3 spawnPosition)
    {
        Instantiate(bezierNode, spawnPosition, gameObject.transform.rotation);
    }
}
