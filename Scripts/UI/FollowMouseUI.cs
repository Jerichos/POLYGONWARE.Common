using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class FollowMouseUI : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        
        private UnityEngine.Camera _camera;
        private RectTransform _rect;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            Vector3 position = Input.mousePosition + _offset;

            // clamp to screen view
            position.x = Mathf.Clamp(position.x, _offset.x, Screen.width - _offset.x);
            position.y = Mathf.Clamp(position.y, _offset.y, Screen.height - _offset.y);
            
            transform.position = position;
        }
    }
}