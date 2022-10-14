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
        
        [Header("References")]
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Transform _transform;

        private BoxCollisionHandler2D _collisionHandler;
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
            _collisionHandler = new BoxCollisionHandler2D(_collider, _collisionLayer, 1f);
        }

        private void LateUpdate()
        {
            _collisionHandler.ResetCollisions();
            _collisionHandler.CheckVerticalCollision(_velocity.y);

            if (_collisionHandler.Collisions.Up)
            {
                _velocity.y = 0;
            }
            
            if (_collisionHandler.Collisions.Down)
            {
                Grounded = true;
                _velocity.y = 0;
            }
            else
            {
                Grounded = false;
            }

            _collisionHandler.CheckHorizontalCollision(_velocity.x);

            if (_collisionHandler.Collisions.Right || _collisionHandler.Collisions.Left)
            {
                _velocity.x = 0;
            }

            if (!_collisionHandler.Collisions.Right && !_collisionHandler.Collisions.Left)
            {
                _collisionHandler.CheckDiagonalCollision(_velocity);
            }
            
            if (_collisionHandler.Collisions.Diagonal)
            {
                _velocity.x = 0;
                _velocity.y = 0;
            }
            
            _transform.Translate(_velocity * Time.deltaTime);
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
    }

    public struct CollisionData
    {
        public bool Up;
        public bool Down;
        public bool Right;
        public bool Left;
        public bool Diagonal;

        public override string ToString()
        {
            return "UP: " + Up + " DOWN: " + Down + " RIGHT: " + Right + " LEFT: " + Left + " DIAGONAL:" + Diagonal;
        }
    }
}