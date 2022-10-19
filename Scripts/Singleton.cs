﻿using UnityEngine;

namespace Common
{
    public class Singleton<T> : MonoBehaviour where T: Component
    {
        [SerializeField] private bool _dontDestroyOnLoad;
        
        private static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();
            Debug.Log(name + " Singleton Initialized");
            
            if(_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}