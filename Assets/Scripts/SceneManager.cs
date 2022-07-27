using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private PathFinder pathFinder;
    [SerializeField]
    private GameObject spaceShip;
    private BezierManager bezierManager;

    [SerializeField]
    private Button toggleSpaceShipButton;
    [SerializeField]
    private TMPro.TMP_Text toggleSpaceShipButtonText;

    [SerializeField]
    private Button toggleLineBetweenNodesButton;
    [SerializeField]
    private TMPro.TMP_Text toggleLineBetweenNodesText;

    [SerializeField] 
    private GameObject toggleSpaceShipSpeedPanel;


    void Start()
    {
        InitSpaceShip();
        InitBezierManager();
    }

    void InitSpaceShip()
    {
        if (spaceShip)
        {
            pathFinder = spaceShip.GetComponent<PathFinder>();
            spaceShip.SetActive(false);
        }
    }

    void InitBezierManager()
    {
        var bezierManagerObj = GameObject.FindGameObjectWithTag(Constants.Tags.BezierManager);
        if (bezierManagerObj)
        {
            bezierManager = bezierManagerObj.GetComponent<BezierManager>();
        }
    }

    public void ToggleSpawnSpaceShipButton(bool isInteractable)
    {
        toggleSpaceShipButton.interactable = isInteractable;
    }

    public void ToggleSpaceShip()
    {
        if (spaceShip && bezierManager.CanCurveDrawable())
        {
            bool prevState = spaceShip.activeSelf;
            spaceShip.SetActive(!spaceShip.activeSelf);
            if (prevState)
            {
                toggleSpaceShipButtonText.text = Constants.UITexts.ToggleSpaceShipButtonDeactivatedText;
            }
            else
            {
                toggleSpaceShipButtonText.text = Constants.UITexts.ToggleSpaceShipButtonActiveText;
            }
            ToggleSpaceShipSpeedPanel(!prevState);
            pathFinder.ResetCourse();
            
        }
    }

    public void ToggleLineBetweenNodesButtonInteractibility(bool isInteractable)
    {
        toggleLineBetweenNodesButton.interactable = isInteractable;
    }

    public void ToggleLineBetweenNodesButton()
    {
        bezierManager.ToggleVisibiltyOfLinesBetweenNodes();
        if (!bezierManager.IsLinesBetweenNodesShowing())
        {
            toggleLineBetweenNodesText.text = Constants.UITexts.ToggleLineBetweenNodesButtonDeactivatedText;
        }
        else
        {
            toggleLineBetweenNodesText.text = Constants.UITexts.ToggleLineBetweenNodesButtonActiveText;
        }
    }

    void ToggleSpaceShipSpeedPanel(bool isActivable)
    {
        toggleSpaceShipSpeedPanel.SetActive(isActivable);
    }
}
