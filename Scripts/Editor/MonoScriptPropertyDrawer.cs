using System.Collections.Generic;
using POLYGONWARE.Common.Attributes;
using POLYGONWARE.Common.Player;
using UnityEditor;
using UnityEngine;

namespace POLYGONWARE.Common.Editor
{
    [CustomPropertyDrawer(typeof(MonoScriptAttribute), false)]
    public class MonoScriptPropertyDrawer : PropertyDrawer
    {
        static Dictionary<string, MonoScript> m_ScriptCache;
        static MonoScriptPropertyDrawer()
        {
            _type = null;
        }
        
        bool m_ViewString = false;
        static private string _type;

        private void GetScripts()
        {
            MonoScriptAttribute monoScript = (MonoScriptAttribute) attribute;
            
            m_ScriptCache = new Dictionary<string, MonoScript>();
            
            Debug.Log("Find Objects " + typeof(MonoScript));
            
            var scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            
            for (int i = 0; i < scripts.Length; i++)
            {
                var type = scripts[i].GetClass();
                if (type != null && !m_ScriptCache.ContainsKey(type.FullName) && monoScript.type.IsAssignableFrom(type))
                {
                    Debug.Log("add " + type.FullName);
                    m_ScriptCache.Add(type.FullName, scripts[i]);
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MonoScriptAttribute monoScript = (MonoScriptAttribute) attribute;

            if (_type != nameof(monoScript.type))
            {
                GetScripts();
                _type = nameof(monoScript.type);
            }
            
            if (property.propertyType == SerializedPropertyType.String)
            {
                Rect r = EditorGUI.PrefixLabel(position, label);
                Rect labelRect = position;
                labelRect.xMax = r.xMin;
                position = r;
                m_ViewString = GUI.Toggle(labelRect, m_ViewString, "", "label");
                if (m_ViewString)
                {
                    property.stringValue = EditorGUI.TextField(position, property.stringValue);
                    return;
                }
                MonoScript script = null;
                string typeName = property.stringValue;
                if (!string.IsNullOrEmpty(typeName))
                {
                    m_ScriptCache.TryGetValue(typeName, out script);
                    if (script == null)
                        GUI.color = Color.red;
                }

                script = (MonoScript)EditorGUI.ObjectField(position, script, typeof(IInputHandler), false);
                if (GUI.changed)
                {
                    if (script != null)
                    {
                        var type = script.GetClass();
                        MonoScriptAttribute attr = (MonoScriptAttribute)attribute;
                        if (attr.type != null && !attr.type.IsAssignableFrom(type))
                            type = null;
                        if (type != null)
                            property.stringValue = type.FullName;
                        else
                            Debug.LogWarning("The script file " + script.name + " doesn't contain an assignable class");
                    }
                    else
                        property.stringValue = "";
                }
            }
            else
            {
                GUI.Label(position, "The MonoScript attribute can only be used on string variables");
            }
        }
    }
}