using System;
using UnityEngine;

namespace Common
{
    public class PlayerCamera : Singleton<PlayerCamera>
    {
        private Transform _target;

        private void Update()
        {
            var position = _target.position;
            position.z = -10;
            transform.position = position;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public bool IsOnScreen(Vector2 position, Vector2 offset)
        {
            return true;
        }
    }
}