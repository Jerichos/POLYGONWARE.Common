using System;
using UnityEngine;

namespace POLYGONWARE.Common
{

public enum PhysicsState
{
    Ground,
    Water
}
    public class CharacterPhysics : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _collisionLayer;
        [SerializeField] private LayerMask _waterLayer;
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _stepHeight = 0.25f;
        
        [Header("Jump")]
        [SerializeField] private float _maxJumpHeight = 2;
        [SerializeField] private float _maxJumpInSeconds = 0.5f;
        [SerializeField] private AnimationCurve _jumpCurve;

        [Header("Gravity")] 
        [SerializeField] private float _maxGravity = -10;
        [SerializeField] private AnimationCurve _gravityCurve;

        [Header("Physics")] 
        [SerializeField] private BoxCollider _boxCollider;

        [Header("Water")] 
        [SerializeField] private ProxyTrigger _shallowWaterTrigger;
        [SerializeField] private ProxyTrigger _deepWaterTrigger;

        private float _jumpT;
        private float _gravityT;
        
        private Vector3 _direction;
        private Vector3 _velocity;
        private Vector3 _velocityDelta;
        private Vector3 _targetVelocity;
        
        private Quaternion _targetDirection;

        private Transform _transform;
        private PhysicsState _state;

        public Vector3 Velocity => _velocity;
        public Vector3 VelocityDelta => _velocityDelta;

        private bool _isJumping; // means acceleration on y axis
        private bool _grounded;
        private bool _isHorizontalHit;

        private RaycastHit _verticalHit;
        private RaycastHit _horizontalHit;

        public bool Grounded => _grounded;
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            // TODO: separate each handler into separate classes
            UpdateGroundState();
        }

        private void UpdateGroundState()
        {
            // calculate velocity
            _velocity = Vector3.SmoothDamp(_velocity, _targetVelocity, ref _velocity, _smoothTime);
            _velocityDelta = _velocity * Time.deltaTime;
            
            // Cast a ray downward to check for the ground
            // Ray ray = new Ray(_transform.position + Vector3.up * _stepHeight + _velocityDelta, Vector3.down);
            // RaycastHit hit;
            var colliderHalf = _boxCollider.size / 2;
            if (Physics.BoxCast(_transform.position + Vector3.up * (_stepHeight + 0.1f), new Vector3(colliderHalf.x, 0.1f, colliderHalf.z), Vector3.down,
                    out _verticalHit, Quaternion.identity, 0.5f + Mathf.Abs(_velocityDelta.y), _collisionLayer) && !_isJumping)
            {
                Debug.Log("grounded");
                OnGrounded(_verticalHit.point);
            }
            else
            {
                _grounded = false;
            }
            
            // if not grounded, apply gravity
            if (!_grounded)
            {
                _gravityT += Time.deltaTime;
                _velocityDelta.y = _gravityCurve.Evaluate(_gravityT) * Time.deltaTime * _maxGravity;
            }

            if (_state == PhysicsState.Water)
            {
                _velocityDelta.y = 0;
            }
            
            // TODO: Better handle slope angles
            
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
                var jumpT = _jumpT + (Time.deltaTime / _maxJumpInSeconds);
                
                var t = _jumpCurve.Evaluate(jumpT);
                var prevT = _jumpCurve.Evaluate(_jumpT);
                
                var jumpDelta = t - prevT;
                _jumpT = jumpT;

                if (jumpT >= 1)
                {
                    JumpStop();
                }

                _velocityDelta.y = jumpDelta * _maxJumpHeight;
            }
            
            // check horizontal collisions
            if (_velocityDelta.x != 0 || _velocityDelta.z != 0)
            {
                var horizontalVelocityDelta = _velocityDelta;
                horizontalVelocityDelta.y = 0;

                var colliderExtents = _boxCollider.size / 2;
                colliderExtents.y -= _stepHeight /2;
                
                _isHorizontalHit = Physics.BoxCast(_transform.position + Vector3.up * (colliderExtents.y + _stepHeight), new Vector3(colliderExtents.x, colliderExtents.y, colliderExtents.z), horizontalVelocityDelta.normalized, 
                    out _horizontalHit, Quaternion.identity, horizontalVelocityDelta.magnitude, _collisionLayer);

                if(_isHorizontalHit)
                {
                    // handle movement along the normal:
                    var dot = Vector3.Dot(_horizontalHit.normal, _velocityDelta.normalized);

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
            _transform.position += _velocityDelta;

            // Smoothly rotate the character to the movement direction
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetDirection, Time.deltaTime * _rotationSpeed);
        }

        private void OnGrounded(Vector3 groundPosition)
        {
            // Snap the character to the ground
            var snapPosition = Mathf.Lerp(_transform.position.y, groundPosition.y, 20 * Time.deltaTime);
            var position = _transform.position;
            position.y = snapPosition;
            _transform.position = position;
            
            _velocity.y = 0;
            _velocityDelta.y = 0;
            _gravityT = 0;
            
            _grounded = true;
        }

        public void JumpStart()
        {
            if(_isJumping || (!_grounded && _state == PhysicsState.Ground))
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
            
            if(direction != Vector2.zero)
                _targetDirection = Quaternion.LookRotation(_targetVelocity);
            // Debug.Log("TargetVelocity: " + _targetVelocity);
        }

        public void StopMove()
        {
            _targetVelocity = Vector2.zero;
            _direction = Vector3.zero;
        }
        
        private void OnDeepWaterExit(Collider value)
        {
            _state = PhysicsState.Ground;
        }

        private void OnDeepWaterEnter(Collider value)
        {
            _state = PhysicsState.Water;
        }

        private void OnEnable()
        {
            _deepWaterTrigger.OnEnter += OnDeepWaterEnter;
            _deepWaterTrigger.OnExit += OnDeepWaterExit;
        }

        private void OnDisable()
        {
            _deepWaterTrigger.OnEnter -= OnDeepWaterEnter;
            _deepWaterTrigger.OnExit -= OnDeepWaterExit;
        }

        //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            //Check if there has been a hit yet
            if (_isHorizontalHit)
            {
                var colliderExtents = _boxCollider.size / 2;
                colliderExtents.y -= _stepHeight / 2;
                
                Gizmos.color = Color.red;
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(_transform.position + Vector3.up * (colliderExtents.y + _stepHeight), transform.forward * _horizontalHit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(_transform.position + Vector3.up * (colliderExtents.y + _stepHeight) + transform.forward * _horizontalHit.distance, _boxCollider.size);
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