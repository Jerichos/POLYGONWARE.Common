using System;
using UnityEngine;

namespace Common
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private PooledObject _pooledPrefab;
        
        [Tooltip("Spawn periodically in seconds. 0 means do NOT spawn repeatedly.")] 
        [SerializeField] private float _spawnCooldown = 0;
        
        public void Spawn()
        {
            if (_pooledPrefab == null)
            {
                Debug.LogWarning(gameObject.name + ": Nothing to spawn...", gameObject);
                return;
            }
            
            var instance = PoolSystem.GetInstance(_pooledPrefab.PrefabID);
            instance.transform.position = transform.position;
            instance.gameObject.SetActive(true);
            
            if(_spawnCooldown > 0)
                Invoke(nameof(Spawn), _spawnCooldown);
        }

#if UNITY_EDITOR
        private bool _autoName = true;
        private void OnValidate()
        {
            if(!_autoName)
                return;

            if (_pooledPrefab)
            {
                gameObject.name = "Spawn: " + _pooledPrefab.name;
                if (_spawnCooldown > 0)
                    gameObject.name += " e" + _spawnCooldown + "s";
            }
        }
#endif
    }
}