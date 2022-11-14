using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class FloatingPanelUI : MonoBehaviour
    {
        // TODO: Clamp to screen
        [SerializeField] private bool _clampToScreen;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _transform;

        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            var position = _camera.WorldToScreenPoint(_target.position);
            position += _offset;
            _transform.position = position;
        }

        private void OnEnable()
        {
            if (!_camera)
            {
                Debug.LogError(name + " _camera is not assigned.");
                enabled = false;
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            if (target)
                enabled = true;
            else
                enabled = false;
        }
    }
}