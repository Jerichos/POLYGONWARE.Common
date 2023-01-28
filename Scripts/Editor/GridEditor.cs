using UnityEditor;
using UnityEngine;

namespace POLYGONWARE.Common.Editor
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update Grid"))
            {
                var grid = target as Grid;
                grid.InitializeGrid();
            }
        }
    }
}