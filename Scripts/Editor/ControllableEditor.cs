using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common.Editor
{
    [CustomEditor(typeof(Controllable), true)]
    public class ControllableEditor : UnityEditor.Editor
    {
        [NonSerialized] private SerializedProperty m_ActionsProperty;
        [NonSerialized] private SerializedProperty m_DefaultActionMapProperty;
        [NonSerialized] private GUIContent[] m_ActionMapOptions;
        [NonSerialized] private readonly GUIContent m_DefaultActionMapText = EditorGUIUtility.TrTextContent("Default Map", "Action map to enable by default. If not set, no actions will be enabled by default.");
        [NonSerialized] private int m_SelectedDefaultActionMap;
        [NonSerialized] private GUIContent[] m_ActionNames;
        [NonSerialized] private GUIContent[] m_ActionMapNames;
        [NonSerialized] private int m_SelectedDefaultControlScheme;
        [NonSerialized] private GUIContent[] m_ControlSchemeOptions;
        [NonSerialized] private bool m_ActionAssetInitialized;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Controllable", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();

            // Action config section.
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ActionsProperty);
            var actionsWereChanged = false;
            if (EditorGUI.EndChangeCheck() || !m_ActionAssetInitialized)
            {
                OnActionAssetChange();
                actionsWereChanged = true;
            }
            
            ++EditorGUI.indentLevel;
            if (m_ActionMapOptions != null && m_ActionMapOptions.Length > 0)
            {
                // Default action map picker.

                var selected = EditorGUILayout.Popup(m_DefaultActionMapText, m_SelectedDefaultActionMap,
                    m_ActionMapOptions);
                if (selected != m_SelectedDefaultActionMap)
                {
                    if (selected == 0)
                    {
                        m_DefaultActionMapProperty.stringValue = null;
                    }
                    else
                    {
                        // Use ID rather than name.
                        var asset = (InputActionAsset)m_ActionsProperty.objectReferenceValue;
                        var actionMap = asset.FindActionMap(m_ActionMapOptions[selected].text);
                        if (actionMap != null)
                            m_DefaultActionMapProperty.stringValue = actionMap.id.ToString();
                    }
                    m_SelectedDefaultActionMap = selected;
                }
            }
            --EditorGUI.indentLevel;

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void OnActionAssetChange()
        {
            serializedObject.ApplyModifiedProperties();
            m_ActionAssetInitialized = true;

            var playerInput = FindObjectOfType<PlayerInput>();
            var controllable =(Controllable) target;

            if (!playerInput)
            {
                Debug.LogError("Missing PlayerInput in the scene");
                return;
            }
            
            var asset = (InputActionAsset)m_ActionsProperty.objectReferenceValue;
            if (asset == null)
            {
                m_ControlSchemeOptions = null;
                m_ActionMapOptions = null;
                m_ActionNames = null;
                m_SelectedDefaultActionMap = -1;
                m_SelectedDefaultControlScheme = -1;
                return;
            }

            // Read out control schemes.
            var selectedDefaultControlScheme = playerInput.defaultControlScheme;
            m_SelectedDefaultControlScheme = 0;
            var controlSchemes = asset.controlSchemes;
            m_ControlSchemeOptions = new GUIContent[controlSchemes.Count + 1];
            m_ControlSchemeOptions[0] = new GUIContent(EditorGUIUtility.TrTextContent("<Any>"));
            ////TODO: sort alphabetically
            for (var i = 0; i < controlSchemes.Count; ++i)
            {
                var name = controlSchemes[i].name;
                m_ControlSchemeOptions[i + 1] = new GUIContent(name);

                if (selectedDefaultControlScheme != null && string.Compare(name, selectedDefaultControlScheme,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                    m_SelectedDefaultControlScheme = i + 1;
            }
            if (m_SelectedDefaultControlScheme <= 0)
                playerInput.defaultControlScheme = null;

            // Read out action maps.
            var selectedDefaultActionMap = !string.IsNullOrEmpty(controllable.ActionMap)
                ? asset.FindActionMap(controllable.ActionMap)
                : null;
            m_SelectedDefaultActionMap = asset.actionMaps.Count > 0 ? 1 : 0;
            var actionMaps = asset.actionMaps;
            m_ActionMapOptions = new GUIContent[actionMaps.Count + 1];
            m_ActionMapOptions[0] = new GUIContent(EditorGUIUtility.TrTextContent("<None>"));
            ////TODO: sort alphabetically
            for (var i = 0; i < actionMaps.Count; ++i)
            {
                var actionMap = actionMaps[i];
                m_ActionMapOptions[i + 1] = new GUIContent(actionMap.name);

                if (selectedDefaultActionMap != null && actionMap == selectedDefaultActionMap)
                    m_SelectedDefaultActionMap = i + 1;
            }
            if (m_SelectedDefaultActionMap <= 0)
                controllable.ActionMap = null;
            else
            {
                controllable.ActionMap = m_ActionMapOptions[m_SelectedDefaultActionMap].text;
                Debug.Log("set " + controllable.ActionMap);
            }

            serializedObject.Update();
        }

        private void OnEnable()
        {
            m_ActionsProperty = serializedObject.FindProperty("_actionAsset");
            m_DefaultActionMapProperty = serializedObject.FindProperty("_actionMap");
        }
        
        private void Refresh()
        {
            ////FIXME: doesn't seem like we're picking up the results of the latest import
            m_ActionAssetInitialized = false;
            Repaint();
        }
        
       
    }
}