using UnityEditor;

namespace POLYGONWARE.Common.Editor
{
    [CustomEditor(typeof(PathManager))]
    public class PathManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var pathManager = target as PathManager;
            
            if(!pathManager)
                return;
            
            pathManager.Refresh();
        }
    }
}