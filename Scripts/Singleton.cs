using UnityEngine;

namespace Common
{
    public class Singleton<T> : MonoBehaviour where T: Component
    {
        [SerializeField] private bool _dontDestroyOnLoad;
        
        private T _instance;
        public T Instance => _instance;

        protected void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();
            
            if(_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}