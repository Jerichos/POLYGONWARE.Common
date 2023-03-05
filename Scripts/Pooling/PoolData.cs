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

public struct ObjectPool<T> where T: Component
{
    private T Prefab;
        
    private Queue<T> Instances;
        
    private int Count;
    private Transform _parent;

    private int _prewarmCount;

    public void Initialize(T templateObject, Transform parent, int prewarmCount, bool includeTemplate = false)
    {
        Prefab = templateObject;
        _parent = parent;
        Count = 0;
        Instances = new Queue<T>();

        _prewarmCount = prewarmCount;
        
        if(includeTemplate)
            EnqueueInstance(templateObject);

        while(Count <= _prewarmCount)
        {
            EnqueueInstance();
        }
    }
    
    
    private void EnqueueInstance(T instance)
    {
        instance.gameObject.SetActive(false);
        Instances.Enqueue(instance);
        Count++;
    }

    private void EnqueueInstance()
    {
        var instance = Object.Instantiate(Prefab, _parent);
        instance.gameObject.SetActive(false);
        Instances.Enqueue(instance);
        Count++;
    }
        
    public T GetInstance()
    {
        if (Instances.Count == 0)
        {
            return null;
            // Debug.LogWarning("enqueing new instance");
            // EnqueueInstance();
        }
            
        return Instances.Dequeue();
    }

    public void ReturnInstance(T instance)
    {
        Instances.Enqueue(instance);
    }

    public void DestroyAndClear()
    {
        while (Instances.Count > 0)
        {
            var instance = GetInstance();
            Object.Destroy(instance.gameObject);
        }
        
        Instances.Clear();
    }
}
}