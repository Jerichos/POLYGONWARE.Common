using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Pooling
{
    [Serializable]
    public struct PoolData
    {
        public PooledObject Prefab;
        public int MinCount;
        public int MaxCount;
        private int Count;
        
        public Queue<PooledObject> Instances;

        public void Initialize()
        {
            Instances = new Queue<PooledObject>();
            Count = 0;

            for (int i = 0; i < MinCount; i++)
            {
                EnqueueNewInstance();
            }
        }

        private void EnqueueNewInstance()
        {
            var instance = Object.Instantiate(Prefab);
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