using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CharacterTopDownCamera : PlayerCamera
    {
        [SerializeField] private Vector3 _offset;
        private Transform _transform;

        private CharacterPhysics _characterPhysics;

        protected override void Awake()
        {
            base.Awake();
            _transform = transform;
        }

        protected override void FollowTarget()
        {
            Vector3 targetPosition = _target.position + _offset;

            if (!_characterPhysics.Grounded)
            {
                targetPosition.y = _transform.position.y;
            }

            _transform.position = Vector3.Lerp(_transform.position, targetPosition, 10 * Time.deltaTime);
        }

        protected override void OnTargetSet(Transform target)
        {
            _characterPhysics = target.GetComponent<CharacterPhysics>();

            if (!_characterPhysics)
                _transform.LogError("target does not have CharacterPhysics component");
        }
    }
}