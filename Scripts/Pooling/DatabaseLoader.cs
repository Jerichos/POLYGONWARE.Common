using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class DatabaseLoader
    {
        public static readonly Dictionary<uint, PooledObject> Prefabs = new();
        
        private const string PREFABS_PATH = "prefabs/";

        [RuntimeInitializeOnLoadMethod]
        private static void LoadAllPrefabs()
        {
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