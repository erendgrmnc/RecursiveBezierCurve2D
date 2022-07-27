using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class GameObjectNames
    {
        public static string BezierNodes = "BezierNodes";
        public static string LinesBetweenBezierNodes = "LinesBetweenBezierNodes";
    }

    public static class Tags
    {
        public static string PathFinder = "PathFinder";
        public static string BezierNode = "BezierCurveNode";
        public static string BezierManager = "BezierManager";
        public static string Line = "Line";
        public static string SceneManager = "SceneManager";
        public static string CurveLineRenderer = "CurveLine";
    }

    public static class UITexts
    {
        public static string ToggleSpaceShipButtonActiveText = "Disable Space Ship";
        public static string ToggleSpaceShipButtonDeactivatedText = "Spawn Space Ship";
        public static string ToggleLineBetweenNodesButtonActiveText = "Hide Lines Between Nodes";
        public static string ToggleLineBetweenNodesButtonDeactivatedText = "Show Lines Between Nodes";
    }
}
