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
}

public class InstancedPool
{
    public PooledObject Prefab;
    public Queue<PooledObject> Instances;
    private Transform _parent;
    
    public int MinCount;
    public int MaxCount;
    private int Count;
    
    public InstancedPool(PoolData poolData, Transform parent)
    {
        Prefab = poolData.Prefab;
        MinCount = poolData.MinCount;
        MaxCount = poolData.MaxCount;
        Count = 0;
        _parent = parent;
        Instances = new Queue<PooledObject>();

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
        Debug.Log("enqueued new instance, available instances: " + Instances.Count + " count: " + Count + " max count: " + MaxCount + " min count: " + MinCount + " prefab: " + Prefab.name + " parent: " + _parent.name);
    }
        
    public PooledObject GetInstance()
    {
        //Debug.Log("get instance, available instances: " + Instances.Count);
        if (Instances.Count == 0)
        {
            if (Count >= MaxCount)
            {
                Debug.LogWarning("You reached MaxCount in this pool bro.");
                //return null;
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