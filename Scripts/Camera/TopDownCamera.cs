using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class TopDownCamera : PlayerCamera
    {
        [SerializeField] private Vector3 _offset;
        private Transform _transform;

        protected override void Awake()
        {
            base.Awake();
            _transform = transform;
        }

        protected override void FollowTarget()
        {
            Vector3 targetPosition = _target.position + _offset;
            _transform.position = Vector3.Lerp(_transform.position, targetPosition, 10 * Time.deltaTime);
        }
    }
}