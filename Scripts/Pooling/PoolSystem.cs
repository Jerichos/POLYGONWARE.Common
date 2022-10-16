using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class PoolSystem : Singleton<PoolSystem>
    {
        [SerializeField] private List<PoolData> _pooledPrefabs;

        private static readonly Dictionary<uint, PoolData> PooledInstances = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (PoolData poolData in _pooledPrefabs)
            {
                RegisterPrefab(poolData);
            }
        }

        public static void RegisterPrefab(PoolData poolData)
        {
            if (PooledInstances.ContainsKey(poolData.Prefab.PrefabID))
            {
                Debug.LogWarning("Prefab with id " +  poolData.Prefab.PrefabID + " is already pooled.");
                return;
            }
            
            poolData.Initialize(Instance.transform);
            PooledInstances.Add(poolData.Prefab.PrefabID, poolData);
        }

        public static PooledObject GetInstance(uint prefabID)
        {
            return PooledInstances[prefabID].GetInstance();
        }

        public static void ReturnInstance(PooledObject instance)
        {
            PooledInstances[instance.PrefabID].ReturnInstance(instance);
        }
        
        #if UNITY_EDITOR
        public void AddPoolData(PoolData poolData)
        {
            for (int i = 0; i < _pooledPrefabs.Count; i++)
            {
                if (_pooledPrefabs[i].Prefab.PrefabID == poolData.Prefab.PrefabID)
                {
                    Debug.LogError("There is already prefab with same ID in the PooledPrefabs. " + " ID: " + poolData.Prefab.PrefabID);
                    return;
                }
            }
            
            _pooledPrefabs.Add(poolData);
            Debug.Log("Added prefab to the pooling list. " + poolData.Prefab.name + " ID: " + poolData.Prefab.PrefabID);
        }
        #endif
    }
}