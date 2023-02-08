using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class PathManager : MonoBehaviour
    {
        public Path Path = new();

        private void OnDrawGizmos()
        {
            if(Path.Nodes.Length <= 1)
                return;

            Gizmos.DrawIcon(Path.FirstNode, "Light", false);
            Gizmos.DrawIcon(Path.LastNode, "Light", false);

            Gizmos.color = Color.red;
            for (int i = 1; i < Path.Nodes.Count(); i++)
            {
                Gizmos.DrawLine(Path.Nodes[i-1], Path.Nodes[i]);
            }
        }

        private void OnValidate()
        {
            if(Path.Nodes.Length <= 1)
                Debug.LogWarning("Path must have at least 2 nodes. " + Path.Nodes.Length);
            
            #if UNITY_EDITOR
            Refresh();
            #endif
        }
        
        
        
#if UNITY_EDITOR
        public void Refresh()
        {
            Path.Nodes = new Vector3[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                Path.Nodes[i] = transform.GetChild(i).position;
            }
        }
#endif
        public bool EndOfPath(int nextPoint)
        {
            if (nextPoint >= Path.Nodes.Length)
                return true;

            return false;
        }
    }
    
    public static class PathManagerExtensions
    {
        public static Vector3 GetPosition(this PathManager pathManager, int node)
        {
            return pathManager.Path.Nodes[node];
        }
    }
}