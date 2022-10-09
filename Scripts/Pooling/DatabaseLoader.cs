using System.Collections.Generic;
using UnityEngine;

namespace Common.Pooling
{
    public static class DatabaseLoader
    {
        public static readonly Dictionary<uint, PooledObject> Prefabs = new();
        public const string PREFABS_PATH = "prefabs/";

        
        [RuntimeInitializeOnLoadMethod]
        private static void RegisterAllPrefabs()
        {
            var prefabs = Resources.LoadAll<PooledObject>(PREFABS_PATH);
        }
    }
}