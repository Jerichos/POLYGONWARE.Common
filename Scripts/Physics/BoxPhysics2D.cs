using System;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxPhysics2D : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionLayer;

        public Vector2 _lastVelocity;
        
        [Header("References")]
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Transform _transform;

        private BoxCollisionHandler2D _handler;
        private Vector2 _velocity;

        private bool _grounded;

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

        private void Awake()
        {
            _handler = new BoxCollisionHandler2D(_transform, _collider, _collisionLayer, 0.1f);
        }

        private void LateUpdate()
        {
            _handler.ResetHandler();

            _handler.CheckHorizontalCollision(ref _velocity, true);
            _handler.CheckVerticalCollision(ref _velocity, true);
            
            if(_velocity.x != 0 && _velocity.y != 0)
                _handler.CheckDiagonalCollision(ref _velocity, true);

            _transform.Translate(_velocity * Time.deltaTime);
            _lastVelocity = _velocity;
            Debug.Log("LastVelocity: " + _lastVelocity + " Collisions: " + _handler.Collisions);
            _velocity = Vector2.zero; // Maximum Drag
        }

        public void Move(Vector2 velocity)
        {
            _velocity += velocity;
        }

        private void Reset()
        {
            _collider = GetComponent<BoxCollider2D>();
            _transform = transform;
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying == false)
                _handler = new BoxCollisionHandler2D(_transform, _collider, _collisionLayer, 0.1f);
    
            _handler.DrawGizmo();
        }
    }
}