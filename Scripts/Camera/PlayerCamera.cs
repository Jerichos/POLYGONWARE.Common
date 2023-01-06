using UnityEngine;

namespace POLYGONWARE.Common
{
    public abstract class PlayerCamera : Singleton<PlayerCamera>
    {
        [SerializeField] protected Transform _target;

        private void Update()
        {
            if (!_target)
                enabled = false;
            
            FollowTarget();
        }

        protected abstract void FollowTarget();

        public void SetTarget(Transform target)
        {
            _target = target;
            enabled = true;
        }
        
        public bool IsOnScreen(Vector2 position, Vector2 offset)
        {
            return true;
        }

        private void OnEnable()
        {
            if (!_target)
            {
                Debug.LogWarning("PlayerCamera has no target... disabling.");
                enabled = false;
            }
        }
    }
}