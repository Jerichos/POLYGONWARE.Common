using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CharacterPhysics : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _collisionLayer;
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _stepHeight = 0.25f;

        private float _stepRayLength;
        private float _jumpT;
        private float _maxJumpHeight = 2;
        
        private Vector3 _direction;
        private Vector3 _velocity;
        private Vector3 _velocityDelta;
        private Vector3 _targetVelocity;
        
        private Quaternion _targetDirection;

        private Transform _transform;

        public Vector3 Velocity => _velocity;
        public Vector3 VelocityDelta => _velocityDelta;

        private bool _isJumping; // means acceleration on y axis
        private bool _grounded;
        private bool _isHorizontalHit;

        private RaycastHit _verticalHit;
        private RaycastHit _horizontalHit;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _stepRayLength = _stepHeight * 2;
        }

        private void Update()
        {
            // calculate velocity
            _velocity = Vector3.SmoothDamp(_velocity, _targetVelocity, ref _velocity, _smoothTime);
            _velocity.y = -9; // gravity
            _velocityDelta = _velocity * Time.deltaTime;
            _grounded = false;
            
            Debug.Log($"velocityDelta {_velocityDelta.y}");
  
            // Cast a ray downward to check for the ground
            // Ray ray = new Ray(_transform.position + Vector3.up * _stepHeight + _velocityDelta, Vector3.down);
            // RaycastHit hit;
            
            if (Physics.BoxCast(_transform.position + Vector3.up * 0.5f, new Vector3(0.6f, 0.1f, 0.6f), Vector3.down,
                    out _verticalHit, Quaternion.identity, 0.5f + Mathf.Abs(_velocityDelta.y), _collisionLayer) && !_isJumping)
            {
                Debug.Log("Grounded true");
                OnGrounded(_verticalHit.point);
            }
            else
            {
                Debug.Log("Grounded false");
                _grounded = false;
            }
            
            // if (UnityEngine.Physics.Raycast(ray, out hit, _stepRayLength, _groundMask) && !_isJumping) 
            // {
            //     Debug.DrawRay(ray.origin, ray.direction * _stepRayLength, Color.red);
            //     OnGrounded(hit.point);
            // }
            // else
            // {
            //     _grounded = false;
            //     Debug.DrawRay(ray.origin, ray.direction * _stepRayLength, Color.green);
            // }
            
            // handle vertical collisions
            if (_isJumping)
            {
                // jumpT == 1 is highest possible jump
                var jumpT = _jumpT + Time.deltaTime;
                var jumpDelta = jumpT - _jumpT;
                _jumpT = jumpT;

                _velocityDelta.y = jumpDelta * _maxJumpHeight;
                Debug.Log($"velocityY {_velocityDelta.y}");
            }
            
            // check horizontal collisions
            if (_velocityDelta.x != 0 || _velocityDelta.z != 0)
            {
                var horizontalVelocityDelta = _velocityDelta;
                horizontalVelocityDelta.y = 0;
                
                _isHorizontalHit = Physics.BoxCast(_transform.position + Vector3.up * 0.5f, Vector3.one * 0.5f, horizontalVelocityDelta.normalized, 
                    out _horizontalHit, Quaternion.identity, horizontalVelocityDelta.magnitude, _collisionLayer);

                if(_isHorizontalHit)
                {
                    // handle movement along the normal:
                    var dot = Vector3.Dot(_horizontalHit.normal, _velocityDelta.normalized);
                    Debug.Log($"horizontal collision {_horizontalHit.normal} dot: {dot}");

                    var horizontalVelocity = _velocity;
                    horizontalVelocity.y = 0;
                    
                    var newDirection = Vector3.ProjectOnPlane(horizontalVelocityDelta.normalized, _horizontalHit.normal);

                    horizontalVelocity = newDirection * horizontalVelocity.magnitude;
                    horizontalVelocityDelta = newDirection * horizontalVelocityDelta.magnitude;

                    _velocity.x = horizontalVelocity.x;
                    _velocity.z = horizontalVelocity.z;
                    _velocityDelta.x = horizontalVelocityDelta.x;
                    _velocityDelta.z = horizontalVelocityDelta.z;
                }
            }
            
            // Move the character based on the velocity
            Debug.Log($"apply velocity: {_velocityDelta}");
            _transform.position += _velocityDelta;

            // Smoothly rotate the character to the movement direction
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetDirection, Time.deltaTime * _rotationSpeed);
        }

        private void OnGrounded(Vector3 groundPosition)
        {
            // Snap the character to the ground
            Debug.Log($"grounded {groundPosition}");
            var snapPosition = Mathf.Lerp(_transform.position.y, groundPosition.y, 100 * Time.deltaTime);
            var position = _transform.position;
            position.y = snapPosition;
            _transform.position = position;
            _velocity.y = 0;
            _velocityDelta.y = 0;
            _grounded = true;
        }

        public void JumpStart()
        {
            if(_isJumping || !_grounded)
                return;

            Debug.Log("JumpStart");
            _isJumping = true;
            _jumpT = 0;
        }

        public void JumpStop()
        {
            Debug.Log("JumpStop");
            if(!_isJumping)
                return;
            
            _jumpT = 0;
            _isJumping = false;
            _velocityDelta.y = 0;
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
            _targetVelocity = direction * _walkSpeed;
            _targetVelocity.z = _targetVelocity.y;
            _targetVelocity.y = 0;
            
            Debug.Log($"direction {_direction} targetVelocity {_targetVelocity}");
            
            if(direction != Vector2.zero)
                _targetDirection = Quaternion.LookRotation(_targetVelocity);
            // Debug.Log("TargetVelocity: " + _targetVelocity);
        }

        public void StopMove()
        {
            _targetVelocity = Vector2.zero;
            _direction = Vector3.zero;
            Debug.Log($"direction {_direction} targetVelocity {_targetVelocity}");
        }
        
        //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            //Check if there has been a hit yet
            if (_isHorizontalHit)
            {
                Gizmos.color = Color.red;
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * _horizontalHit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f + transform.forward * _horizontalHit.distance, transform.localScale);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * _velocityDelta.magnitude);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f + transform.forward * _velocityDelta.magnitude, transform.localScale);
            }
        }
    }
}