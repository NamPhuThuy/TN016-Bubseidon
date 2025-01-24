#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

//Use backtracking to show the path of the current clicked UIElement
namespace NamPhuThuy
{
    public class UIClickDetector : MonoBehaviour
    {
        private void Start()
        {
            string[] args = Environment.GetCommandLineArgs();
            string ans = "";
            for (int i = 0; i < args.Length; i++)
            {
                Debug.Log($"args[{i}]: {args[i]}");
                ans += $"{args[i]} ";
            }

            Debug.Log(ans);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count > 0)
                {
                    GameObject clickedObject = results[0].gameObject;
                    string path = GetHierarchyPath(clickedObject.transform);
                    Debug.Log($"TNam - Path: {path}");
                }
            }
        }

        private string GetHierarchyPath(Transform transform)
        {
            if (transform.parent == null)
            {
                return transform.name + " -> ";
            }

            return GetHierarchyPath(transform.parent) + transform.name;
        }
    }

    [CustomEditor(typeof(UIClickDetector))]
    public class ClickDetectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal();
            GUILayout.Space(40); // Left margin
            // Display the description
            GUILayout.Label("Enable this component if you want to Debug the 'location on the Hierarchy tree' of the game object you just clicked",  new GUIStyle() { wordWrap = true, normal = {textColor = Color.black}} );
            GUILayout.Space(40); // Right margin
            GUILayout.EndHorizontal();
        }
    }
}
#endif