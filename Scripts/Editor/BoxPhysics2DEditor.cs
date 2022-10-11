using TMPro;
using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    [CustomEditor(typeof(BoxPhysics2D))]
    public class BoxPhysics2DEditor : UnityEditor.Editor
    {
        // private Vector3 _position;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            // EditorGUILayout.LabelField("Editor", EditorStyles.boldLabel);

            // Transform transform = Selection.activeTransform;
            //
            // if(!transform)
            //     return;
            //
            // var script = target as BoxPhysics2D;
            // if (_position != transform.position)
            // {
            //     var delta = transform.position - _position;
            //     transform.position = _position;
            //     
            //     script.Move(delta / Time.deltaTime);
            //     
            //     Debug.Log("Delta " + delta);
            // }
        }
    }
}