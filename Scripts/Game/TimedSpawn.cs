using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Common
{
    public class TimedSpawn : MonoBehaviour
    {
        [SerializeField] private PooledObject _spawnPfb;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private float _startDelay = 0;

        [SerializeField] private bool _killInstanceOffScreen;
        
        private void OnEnable()
        {
            InvokeRepeating(nameof(SpawnPrefab), _startDelay, _cooldown);
        }

        private void SpawnPrefab()
        {
            var instance = PoolSystem.GetInstance(_spawnPfb.PrefabID);
            instance.transform.position = transform.position;

            if (_killInstanceOffScreen)
                instance.AddComponent<DisableOffScreen>();
            
            instance.SetActive(true);
        }
    }
}