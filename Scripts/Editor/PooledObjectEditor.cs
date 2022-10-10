using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    [CustomEditor(typeof(PooledObject), true)]
    public class PooledObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("PooledObject", EditorStyles.boldLabel);
            GUILayout.Label("PrefabID: " + ((PooledObject)target).PrefabID);

            if (PrefabUtility.GetPrefabInstanceStatus(Selection.activeGameObject) !=
                PrefabInstanceStatus.NotAPrefab) return;
            
            if (GUILayout.Button("Add to the PoolingSystem"))
            {
                FindObjectOfType<PoolSystem>().AddPoolData(new PoolData()
                {
                    Prefab = (PooledObject)target,
                    MinCount = 10,
                    MaxCount = 100,
                });
            }

            if (GUILayout.Button("Update All IDs"))
            {
                DatabaseLoader.UpdateAllIDs();
            }
        }
    }
}