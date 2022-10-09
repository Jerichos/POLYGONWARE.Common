using System.Collections.Generic;
using UnityEngine;

namespace Common.Pooling
{
    public class PoolSystem : Singleton<PoolSystem>
    {
        [SerializeField] private PoolData[] _pooledPrefabs;

        private readonly Dictionary<uint, PoolData> PooledInstances = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (PoolData poolData in _pooledPrefabs)
            {
                RegisterPrefab(poolData);
            }
        }

        public void RegisterPrefab(PoolData poolData)
        {
            if (PooledInstances.ContainsKey(poolData.Prefab.PrefabID))
            {
                Debug.LogWarning("Prefab with id " +  poolData.Prefab.PrefabID + " is already pooled.");
                return;
            }
            
            poolData.Initialize();
            PooledInstances.Add(poolData.Prefab.PrefabID, poolData);
        }

        public PooledObject GetInstance(uint prefabID)
        {
            return PooledInstances[prefabID].GetInstance();
        }

        public void ReturnInstance(PooledObject instance)
        {
            PooledInstances[instance.PrefabID].ReturnInstance(instance);
        }
    }
}