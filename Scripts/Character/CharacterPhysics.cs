using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CharacterPhysics : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _stepHeight = 0.25f;

        private float _stepRayLength;
        
        private Vector3 _direction;
        private Vector3 _velocity;
        private Vector3 _velocityDelta;
        private Vector3 _targetVelocity;
        
        private Quaternion _targetDirection;

        private Transform _transform;

        public Vector3 Velocity => _velocity;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _stepRayLength = _stepHeight * 2;
        }

        private void Update()
        {
            // calculate velocity
            _velocity = Vector3.SmoothDamp(_velocity, _targetVelocity, ref _velocity, _smoothTime);
  
            // Cast a ray downward to check for the ground
            Ray ray = new Ray(_transform.position + Vector3.up * _stepHeight + _velocity * Time.deltaTime, Vector3.down);
            RaycastHit hit;
            
            if (UnityEngine.Physics.Raycast(ray, out hit, _stepRayLength, _groundMask)) 
            {
                // Snap the character to the ground
                _transform.position = Vector3.Lerp(_transform.position, hit.point, 15 * Time.deltaTime);
                Debug.DrawRay(ray.origin, ray.direction * _stepRayLength, Color.red);
                _transform.position += _velocity * Time.deltaTime;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * _stepRayLength, Color.green);
            }
            
            // Move the character based on the velocity

            // Smoothly rotate the character to the movement direction
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetDirection, Time.deltaTime * _rotationSpeed);
        }

        public void Move(Vector2 vector)
        {
            _targetVelocity = vector * _walkSpeed;
            _targetVelocity.z = _targetVelocity.y;
            _targetVelocity.y = 0;
            
            if(vector != Vector2.zero)
                _targetDirection = Quaternion.LookRotation(_targetVelocity);
            // Debug.Log("TargetVelocity: " + _targetVelocity);
        }

        public void StopMove()
        {
            _targetVelocity = Vector2.zero;
        }
    }
}