using System;
using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct Path
    {
        public List<Node> Nodes;

        public Node FirstNode => Nodes[0];
        public Node LastNode => Nodes[^1];

        public void Clear()
        {
            Nodes.Clear();
        }

        public void Add(Vector3 position)
        {
            Nodes.Add(new Node(position));
        }

    }
    
    public static class PathExtensions
    {
        public static Vector3 GetPosition(this Path path, int node)
        {
            return path.Nodes[node].Vector3;
        }
    }
}