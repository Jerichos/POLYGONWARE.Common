using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class TopDownCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _target;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            Vector3 targetPosition = _target.position + _offset;
            _transform.position = Vector3.Lerp(_transform.position, targetPosition, 10 * Time.deltaTime);
        }
    }
}