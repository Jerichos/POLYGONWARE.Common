using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class CameraOnScreenHandler : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private Vector2 _sizeOffset = Vector2.one;
        [SerializeField] private BoxCollider2D _collider;

        private void Awake()
        {
            UpdateColliderSize();
        }

        private void UpdateColliderSize()
        {
            var aspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
            var orthoSize = _camera.orthographicSize;

            Vector2 size;
            size.x = 2.0f * orthoSize * aspect;
            size.y = 2.0f * _camera.orthographicSize;

            _collider.size = size + _sizeOffset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IOnScreenTrigger trigger = other.GetComponent<IOnScreenTrigger>();
            trigger?.OnScreenEnter();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IOnScreenTrigger trigger = other.GetComponent<IOnScreenTrigger>();
            trigger?.OnScreenExit();
        }

        private void OnValidate()
        {
            UpdateColliderSize();
        }
    }
}