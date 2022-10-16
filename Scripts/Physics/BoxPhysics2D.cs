using System;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public enum PhysicsType
    {
        Static,
        Dynamic,
    }
    
    public class BoxPhysics2D : MonoBehaviour
    {
        [SerializeField] private PhysicsType _type;
        [SerializeField] private LayerMask _collisionLayer;

        public Vector2 _lastVelocity;
        
        [Header("References")]
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Transform _transform;

        private BoxCollisionHandler2D _handler;
        private Vector2 _velocity;

        private bool _grounded;
        private Vector2 _velocityDelta;

        public bool Grounded
        {
            get => _grounded;
            set
            {
                if(_grounded == value)
                    return;

                _grounded = value;
            }
        }

        public PhysicsType Type => _type;
        public int ID => _transform.GetInstanceID();

        public float Gap;

        private void Awake()
        {
            PhysicsWorld2D.Add(transform.GetInstanceID(), this);
            
            if (_type == PhysicsType.Static)
            {
                enabled = false;
                return;
            }
            
            _handler = new BoxCollisionHandler2D(_transform, _collider, _collisionLayer, 1f);
        }

        private void LateUpdate()
        {
            if(_velocity == Vector2.zero)
                return;
            
            _velocityDelta = _velocity * Time.deltaTime;
            _handler.ResetHandler();
            _handler.CheckHorizontalCollision(ref _velocityDelta, true);
            _handler.CheckVerticalCollision(ref _velocityDelta, true);
            
            if(_velocity.x != 0 && _velocity.y != 0)
                _handler.CheckDiagonalCollision(ref _velocityDelta, true);
            
            Debug.Log(name + " FinalVelocity: " + _velocityDelta);
            _transform.Translate(_velocityDelta);
            _velocity = Vector2.zero; // Maximum Drag
        }

        public void AddVelocity(Vector2 velocity)
        {
            _velocity += velocity;
        }

        private void Reset()
        {
            _collider = GetComponent<BoxCollider2D>();
            _transform = transform;
        }

        private void OnEnable()
        {
            if (_type == PhysicsType.Static)
                enabled = false;
        }

        private void OnDrawGizmos()
        {
            if(_type == PhysicsType.Static)
                return;
            
            if(Application.isPlaying == false)
                _handler = new BoxCollisionHandler2D(_transform, _collider, _collisionLayer, 0.1f);
    
            _handler.DrawGizmo();
        }

        public void AddHorizontalVelocity(float velocityX)
        {
            _velocity.x += velocityX;
        }

        public Vector2 PushHorizontallyVirtually(ref Vector2 deltaVelocity)
        {
            if (_type == PhysicsType.Static)
                return Vector2.zero;
            
            _handler.CheckHorizontalCollision(ref deltaVelocity, true);
            // _transform.Translate(Vector2.right * deltaVelocity);
            return deltaVelocity;
        }

        public void PerformPush(float deltaX)
        {
            if (_type == PhysicsType.Static)
                return;
            
            _transform.Translate(deltaX * Vector2.right);
        }
    }
}