using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace POLYGONWARE.Common.Camera
{
    public class DevCamera : MonoBehaviour
    {
        [SerializeField] private float _normalSpeed = 5;
        [SerializeField] private float _boostSpeed = 15;
        
        private Vector3 _moveDirection;
        private bool _lookCamera;
        private float _speed;
        
        private void Update()
        {
            _speed = _normalSpeed;
            _moveDirection = Vector3.zero;
            _lookCamera = false;
            
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                _moveDirection.z = -1;
            
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                _moveDirection.z = 1;
            
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                _moveDirection.x = 1;
            
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                _moveDirection.x = -1;

            if (Keyboard.current.shiftKey.isPressed)
                _speed = _boostSpeed;
            
            if (Keyboard.current.spaceKey.isPressed)
                _moveDirection.y = 1;
            
            if (Keyboard.current.leftCtrlKey.isPressed)
                _moveDirection.y = -1;

            if (Mouse.current.rightButton.isPressed)
                _lookCamera = true;


            if (_lookCamera)
            {
                Vector3 mouseDelta = Mouse.current.delta.ReadValue() * (Time.smoothDeltaTime * _speed);
                var deltaRotation = Quaternion.Euler(new Vector3(-mouseDelta.y, mouseDelta.x, 0));
                transform.rotation *= deltaRotation;
                var rotation = transform.eulerAngles;
                rotation.z = 0;
                transform.eulerAngles = rotation;
            }
            
            var deltaPosition = _moveDirection.normalized * (_speed * Time.deltaTime);
            transform.position += transform.forward * deltaPosition.x + transform.right * deltaPosition.z;
        }
    }
}