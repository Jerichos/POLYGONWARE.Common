using UnityEngine;

namespace POLYGONWARE.Common.Camera
{
    public class PlayerCamera : Singleton<PlayerCamera>
    {
        private Transform _target;

        private void Update()
        {
            if (!_target)
                enabled = false;
            
            var position = _target.position;
            position.z = -10;
            transform.position = position;
        }

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
                enabled = false;
        }
    }
}