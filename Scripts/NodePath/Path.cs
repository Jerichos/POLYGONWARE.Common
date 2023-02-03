using System;
using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct Path
    {
        public Vector3[] Nodes;

        public Vector3 FirstNode => Nodes[0];
        public Vector3 LastNode => Nodes[^1];

    }
    
    public static class PathExtensions
    {
        public static Vector3 GetPosition(this Path path, int node)
        {
            return path.Nodes[node];
        }
    }
}