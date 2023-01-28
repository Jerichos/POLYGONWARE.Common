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
            if(Path.Nodes.Count <= 1)
                return;

            Gizmos.DrawIcon(Path.FirstNode.Vector3, "Light", false);
            Gizmos.DrawIcon(Path.LastNode.Vector3, "Light", false);

            Gizmos.color = Color.red;
            for (int i = 1; i < Path.Nodes.Count(); i++)
            {
                Gizmos.DrawLine(Path.Nodes[i-1].Vector3, Path.Nodes[i].Vector3);
            }
        }

        private void OnValidate()
        {
            if(Path.Nodes.Count <= 1)
                Debug.LogWarning("Path must have at least 2 nodes. " + Path.Nodes.Count);
            
            Refresh();
        }
        
#if UNITY_EDITOR
        public void Refresh()
        {
            Path.Clear();
            var childTransforms = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransforms.Add(transform.GetChild(i));
                Path.Add(transform.GetChild(i).position);
            }
        }
#endif
        public bool EndOfPath(int nextPoint)
        {
            if (nextPoint >= Path.Nodes.Count)
                return true;

            return false;
        }
    }
    
    public static class PathManagerExtensions
    {
        public static Vector3 GetPosition(this PathManager pathManager, int node)
        {
            return pathManager.Path.Nodes[node].Vector3;
        }
    }
}