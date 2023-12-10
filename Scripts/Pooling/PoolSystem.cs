using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common.Pooling
{
    [DefaultExecutionOrder(-50)]
    public class PoolSystem : Singleton<PoolSystem>
    {
        [SerializeField] PooledObjectsSO _pooledObjectsSO;
        
        private List<PoolData> PooledPrefabs => _pooledObjectsSO.PooledPrefabs;

        private static readonly Dictionary<uint, InstancedPool> PooledInstances = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (PoolData poolData in PooledPrefabs)
            {
                RegisterPrefab(poolData);
            }
        }

        public static void RegisterPrefab(PoolData poolData)
        {
            Debug.Log("registering prefab " + poolData.Prefab.name + " with id " + poolData.Prefab.PrefabID);
            if (PooledInstances.ContainsKey(poolData.Prefab.PrefabID))
            {
                Debug.LogWarning("Prefab with id " +  poolData.Prefab.PrefabID + " is already pooled.");
                return;
            }
            
            PooledInstances.Add(poolData.Prefab.PrefabID, new InstancedPool(poolData, Instance.transform));
        }

        public static PooledObject GetInstance(uint prefabID)
        {
            return PooledInstances[prefabID].GetInstance();
        }
        
        public static T GetInstance<T>(T poledPrefab) where T: PooledObject
        {
            return (T)PooledInstances[poledPrefab.PrefabID].GetInstance();
        }

        public static void ReturnInstance(PooledObject instance)
        {
            PooledInstances[instance.PrefabID].ReturnInstance(instance);
        }
        
        #if UNITY_EDITOR
        public void AddPoolData(PoolData poolData)
        {
            for (int i = 0; i < PooledPrefabs.Count; i++)
            {
                if (PooledPrefabs[i].Prefab.PrefabID == poolData.Prefab.PrefabID)
                {
                    Debug.LogError("There is already prefab with same ID in the PooledPrefabs. " + " ID: " + poolData.Prefab.PrefabID);
                    return;
                }
            }
            
            PooledPrefabs.Add(poolData);
            Debug.Log("Added prefab to the pooling list. " + poolData.Prefab.name + " ID: " + poolData.Prefab.PrefabID);
        }
        #endif
    }
}