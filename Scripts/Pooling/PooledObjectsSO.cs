using System;
using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common.Pooling
{
[CreateAssetMenu(fileName = "NewObjectPool", menuName = "common/New object pool", order = 0)]
public class PooledObjectsSO : ScriptableObject
{
    public List<PoolData> PooledPrefabs = new();

    private void OnValidate()
    {
        Debug.Log("Update pooled prefabs ids");
        uint id = 0;
        foreach (PoolData poolData in PooledPrefabs)
        {
            poolData.Prefab.PrefabID = id;
            id++;
            Debug.Log("registered prefab " + poolData.Prefab.name + " with id " + poolData.Prefab.PrefabID);
        }
    }
}
}   