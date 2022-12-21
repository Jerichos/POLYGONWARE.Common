using System.Collections.Generic;
using POLYGONWARE.Common.Physics;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public static class PhysicsWorld2D
    {
        private static readonly Dictionary<int, BoxPhysics2D> AllBoxPhysics = new();

        public static void Add(int id, BoxPhysics2D box)
        {
            AllBoxPhysics.Add(id, box);
        }

        public static BoxPhysics2D Get(int id)
        {
            if (AllBoxPhysics.ContainsKey(id))
                return AllBoxPhysics[id];
            
            Debug.LogWarning("PhysicsWorld2D: Hit object with id " + id + " is not registered as BoxPhysics. Component is not attached to an object probably");
            return null;
        }
    }
}