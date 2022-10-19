using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace POLYGONWARE.Common.Pooling
{
    [Serializable]
    public struct PoolData
    {
        public PooledObject Prefab;
        public int MinCount;
        public int MaxCount;
        
        public Queue<PooledObject> Instances;
        
        private int Count;
        private Transform _parent;

        public void Initialize(Transform parent)
        {
            Instances = new Queue<PooledObject>();
            Count = 0;
            _parent = parent;

            for (int i = 0; i < MinCount; i++)
            {
                EnqueueNewInstance();
            }
        }

        private void EnqueueNewInstance()
        {
            var instance = Object.Instantiate(Prefab, _parent);
            instance.gameObject.SetActive(false);
            Instances.Enqueue(instance);
            Count++;
        }
        
        public PooledObject GetInstance()
        {
            if (Instances.Count == 0)
            {
                if (Count >= MaxCount)
                {
                    Debug.LogWarning("You reached MaxCount in this pool bro.");
                    return null;
                }
                
                EnqueueNewInstance();
            }
            
            return Instances.Dequeue();
        }

        public void ReturnInstance(PooledObject instance)
        {
            Instances.Enqueue(instance);
        }
    }
}