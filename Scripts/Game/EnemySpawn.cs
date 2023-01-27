#if URP
using POLYGONWARE.Common.Pooling;
using UnityEngine;

namespace POLYGONWARE.Common.Game
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private PooledObject _pooledPrefab;
        
        [Tooltip("Spawn periodically in seconds. 0 means do NOT spawn repeatedly.")] 
        [SerializeField] private float _spawnCooldown = 0;
        [SerializeField] private bool _disableInstanceOffScreen = false;
        [SerializeField] private Vector2 _disableOffset = Vector2.one * 2;
        
        public void Spawn()
        {
            if (_pooledPrefab == null)
            {
                Debug.LogWarning(gameObject.name + ": Nothing to spawn...", gameObject);
                return;
            }
            
            var instance = PoolSystem.GetInstance(_pooledPrefab.PrefabID);
            instance.transform.position = transform.position;

            if (_disableInstanceOffScreen)
            {
                var disableScript = instance.transform.GetComponent<DisableOffScreen>();
                if (!disableScript)
                {
                    disableScript = instance.gameObject.AddComponent<DisableOffScreen>();
                }
                disableScript.Offset = _disableOffset;
            }
            
            instance.gameObject.SetActive(true);
            
            if(_spawnCooldown > 0)
                Invoke(nameof(Spawn), _spawnCooldown);
        }

        public void DelayedSpawn(float seconds)
        {
            Invoke(nameof(Spawn), seconds);
        }

        public void Stop()
        {
            CancelInvoke();
        }

#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField] private bool _autoName = true;
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
            else
            {
                gameObject.name = "Enemy Spawn";
            }
        }
#endif
    }
}
#endif