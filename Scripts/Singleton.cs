using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
[DefaultExecutionOrder(-100)]
public class Singleton<T> : MonoBehaviour where T: Component
{
    [SerializeField] private bool _dontDestroyOnLoad;
    
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                throw new Exception("Singleton was not initialized!");
            }
            
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = GetComponent<T>();
        transform.Log(typeof(T) + " Singleton Initialized");
        
        if(_dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    
    protected void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
}