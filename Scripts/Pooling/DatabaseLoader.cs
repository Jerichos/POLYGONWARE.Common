using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common.Pooling
{
    [DefaultExecutionOrder(-100)]
    public static class DatabaseLoader
    {
        private const bool LOAD_PREFAB_DATABASE = false;

        public static readonly Dictionary<uint, PooledObject> Prefabs = new();
        
        private const string PREFABS_PATH = "prefabs/";

        [RuntimeInitializeOnLoadMethod]
        private static void LoadAllPrefabs()
        {
            if (!LOAD_PREFAB_DATABASE)
            {
                Debug.LogWarning("DatabaseLoader is disabled");
                return;
            }
            
            var prefabs = Resources.LoadAll<PooledObject>(PREFABS_PATH);
            for (uint i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].PrefabID = i;
            }
            Debug.Log("DatabaseLoader: Loaded " + prefabs.Length + " prefabs.");
        }

        #if UNITY_EDITOR
        public static void UpdateAllIDs()
        {
            var prefabs = Resources.LoadAll<PooledObject>(PREFABS_PATH);
            for (uint i = 0; i < prefabs.Length; i++)
            {
                prefabs[i].PrefabID = i;
            }
            
            Debug.Log("DatabaseLoader: Updated IDs for " + prefabs.Length + " prefabs.");
        }
        #endif
    }
}