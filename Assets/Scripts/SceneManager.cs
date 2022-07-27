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
            if (spaceShip.activeSelf)
            {
                pathFinder.ResetCourse();
                toggleSpaceShipButtonText.text = Constants.UITexts.ToggleSpaceShipButtonDeactivatedText;
            }
            else
            {
                toggleSpaceShipButtonText.text = Constants.UITexts.ToggleSpaceShipButtonActiveText;
            }
            spaceShip.SetActive(!spaceShip.activeSelf);
        }
    }
}
