using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NamPhuThuy
{
    public class ColorGenerator : SimpleSingleton<ColorGenerator>
    {
        [SerializeField]
        private int numberOfColors;

        [SerializeField]
        private List<Color> colors = new List<Color>();

        public List<Color> Colors => colors;

        public void GenerateColors()
        {
            colors.Clear();
            for (int i = 0; i < numberOfColors; i++)
            {
                Color color = new Color
                {
                    r = Random.Range(0.3f, 1f),
                    g = Random.Range(0.3f, 1f),
                    b = Random.Range(0.3f, 1f),
                    a = 1,
                };
                colors.Add(color);
            }
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(ColorGenerator))]
    public class ColorGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ColorGenerator colorGenerator = (ColorGenerator)target;
            if (GUILayout.Button("Generate Colors"))
            {
                colorGenerator.GenerateColors();
            }
        }
    }
#endif
}


