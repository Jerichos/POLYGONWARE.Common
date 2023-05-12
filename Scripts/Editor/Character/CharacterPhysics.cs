using UnityEditor;
using UnityEngine;

namespace POLYGONWARE.Common.Editor
{
[CustomEditor(typeof(CharacterPhysics))]
public class CharacterPhysicsEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var physics = target as CharacterPhysics;
        
        GUILayout.Space(5);
        GUILayout.Label($"Velocity: {physics.Velocity} VelocityDelta {physics.VelocityDelta}");
    }
}
}