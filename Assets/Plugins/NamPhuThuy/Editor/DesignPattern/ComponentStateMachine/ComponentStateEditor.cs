using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NamPhuThuy
{
#if UNITY_EDITOR
    [CustomEditor (typeof (ComponentState), true)]
    public class ComponentStateEditor : Editor 
    {
        //Private Variables:
        ComponentState _target;
        
        //Init:
        void OnEnable()
        {
            _target = target as ComponentState;
        }

        //Inspector GUI:
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            if (!Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                DrawSoloButton();
                DrawHideAllButton();
                GUILayout.EndHorizontal();
            }
            else
            {
                DrawChangeStateButton();
            }
        }

        //GUI Draw Methods:
        void DrawChangeStateButton ()
        {
            GUI.color = Color.green;
            if (GUILayout.Button("Change State"))
            {
                _target.ChangeState(_target.gameObject);
            }
        }

        void DrawHideAllButton ()
        {
            GUI.color = Color.red;
            if (GUILayout.Button ("Hide All"))
            {
                Undo.RegisterCompleteObjectUndo (_target.transform.parent.transform, "Hide All");
                foreach (Transform item in _target.transform.parent.transform) 
                {
                    item.gameObject.SetActive (false);
                }
            }
        }

        void DrawSoloButton ()
        {
            GUI.color = Color.green;
            if (GUILayout.Button ("Solo"))
            {
                foreach (Transform item in _target.transform.parent.transform) 
                {
                    if (item != _target.transform) item.gameObject.SetActive (false);
                    Undo.RegisterCompleteObjectUndo (_target, "Solo");
                    _target.gameObject.SetActive (true);
                }
            }
        }
    }
#endif
}