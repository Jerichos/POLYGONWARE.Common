using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct Node
    {
        public float X;
        public float Y;
        public float Z;
        
        public Node(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }

        public Vector3 Vector3 => new Vector3(X, Y, Z);
    }
}