using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Common
{
    public class BoxCollisionHandler2D
    {
        public RaycastHit2D Hit;
        public float HitRange { get; set; }

        private LayerMask _layer;
        private int _verticalRayCount;
        private int _horizontalRayCount;
        private float _length;
        private float _verticalRadius;
        private float _horizontalRadius;
        private float _skin;
        private float _verticalRange;
        private float _horizontalRange;
        private float _verticalSpace;
        private float _horizontalSpace;
        private Vector2 _rayOrigin;
        private Vector2 _rayPos;

        public RaycastHit2D[] Hits;

        public CollisionData Collisions;
        
        private BoxCollider2D _collider;

        private Vector2 Center => _collider.transform.position + Vector3.up * _collider.bounds.extents.y;

        public BoxCollisionHandler2D(BoxCollider2D collider, LayerMask layer, float maxGap = 0.1f, float skin = 0.15f, float rangeSquish = 0.98f)
        {
            _layer = layer;
            _collider = collider;

            var size = _collider.bounds.extents * 2;

            _verticalRange = size.y * rangeSquish;
            _verticalRadius = size.y / 2;
            
            _horizontalRange = size.x * rangeSquish;
            _horizontalRadius = size.x / 2;
            
            _verticalRayCount = Mathf.CeilToInt(_horizontalRange / maxGap) + 1;
            _verticalSpace = _verticalRayCount == 0? 0 : _horizontalRange / _verticalRayCount;
            
            Debug.Log("VerticalRayCount: " + _verticalRayCount + " horizontalRange: " + _horizontalRange + " maxGap: " + maxGap);
            
            _horizontalRayCount = Mathf.CeilToInt(_verticalRange / maxGap) + 1;
            _horizontalSpace = _horizontalRayCount == 0? 0 : _verticalRange / _horizontalRayCount;
            
            _verticalRange /= 2;
            _horizontalRange /= 2;
            
            _skin = skin;
        }
        
        public void UpdateCollider(BoxCollider2D collider, float maxGap = 0.4f, float skin = 0.15f, float rangeSquish = 0.98f)
        {
            _collider = collider;
            
            var size = _collider.size;

            _verticalRange = size.y * rangeSquish;
            _verticalRadius = size.y / 2;
            
            _horizontalRange = size.x * rangeSquish;
            _horizontalRadius = size.x / 2;
            
            _verticalRayCount = Mathf.CeilToInt(_horizontalRange / maxGap) + 1;
            _verticalSpace = _verticalRayCount == 0? 0 : _verticalRange / _verticalRayCount;
            _verticalRange /= 2;
            
            Debug.Log("VerticalRayCount: " + _verticalRayCount + " horizontalRange: " + _horizontalRange + " maxGap: " + maxGap);
            
            _horizontalRayCount = Mathf.CeilToInt(_verticalRange / maxGap) + 1;
            _horizontalSpace = _horizontalRayCount == 0? 0 : _verticalRange / _verticalRayCount;
            _horizontalRange /= 2;
            
            _skin = skin;
        }

        public void CheckVerticalCollision(float speed)
        {
            if (speed > 0)
                Collisions.Up = CheckCollisionUp(speed);
            else if (speed < 0)
                Collisions.Down = CheckCollisionDown(speed);
        }

        public void CheckHorizontalCollision(float speed)
        {
            if (speed > 0)
                Collisions.Right = CheckCollisionRight(speed);
            else if (speed < 0)
                Collisions.Left = CheckCollisionLeft(speed);
        }

        public bool CheckDiagonalCollision(Vector2 velocity)
        {
            var direction = velocity.normalized;
            
            _length = velocity.magnitude * Time.deltaTime;

            Vector2 corner = new Vector2(Mathf.Sign(direction.x) * _horizontalRange, Mathf.Sign(direction.y) * _verticalRange);
            
            _rayOrigin = Center + corner;

            Hit = Physics2D.Raycast(_rayOrigin, direction, _length, _layer);

            if (Hit)
            {
                Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                
                // I guess in diagonal scenario we should not snap...
                // But let's snap horizontally
                var position = _collider.transform.position;
                position.x = Hit.point.x - (_horizontalRadius * Mathf.Sign(direction.x));
                position.y = direction.y > 0? Hit.point.y - _collider.size.y : Hit.point.y;
                _collider.transform.position = position;
                
                Collisions.Diagonal = true;
                
                return true;
            }
            
            Debug.DrawRay(_rayOrigin, direction * _length, Color.cyan);
            return false;
        }

        public bool CheckCollisionUp(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _verticalRadius;
            _rayOrigin = Center - _horizontalRange * Vector2.right;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _verticalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.right * (i * _verticalSpace);

                Hit = Physics2D.Raycast(_rayPos, Vector2.up, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                        _closestHit = Hit;
                    
                    collision = true;
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayPos, Vector2.up * (_length), Color.cyan);
            }
            
            if (collision)
            {
                var position = _collider.transform.position;
                position.y = _closestHit.point.y - _collider.size.y;
                _collider.transform.position = position;
            }

            return collision;
        }
        
        public bool CheckCollisionDown(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _verticalRadius;
            _rayOrigin = Center - _horizontalRange * Vector2.right;

            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };

            for (int i = 0; i < _verticalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.right * (i * _verticalSpace);
                // Debug.Log(i + " Ray " + _rayOrigin+ " _length " + _length+ " _rayPos " + _rayPos+ " direction " + Vector2.down);

                Hit = Physics2D.Raycast(_rayPos, Vector2.down, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                        _closestHit = Hit;

                    collision = true;
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayPos, Vector2.down * (_length), Color.cyan);
            }

            if (collision)
            {
                var position = _collider.transform.position;
                position.y = _closestHit.point.y;
                _collider.transform.position = position;
            }

            return collision;
        }
        
        public bool CheckCollisionRight(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _horizontalRadius;
            _rayOrigin = Center + Vector2.down * _verticalRange;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _horizontalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.up * (i * _horizontalSpace);

                Hit = Physics2D.Raycast(_rayPos, Vector2.right, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                    {
                        _closestHit = Hit;
                    }

                    collision = true;
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayPos, Vector2.right * (_length), Color.cyan);
            }

            // Snap
            if (collision)
            {
                var position = _collider.transform.position;
                position.x = _closestHit.point.x - _horizontalRadius;
                _collider.transform.position = position;
            }
            
            return collision;
        }
        
        public bool CheckCollisionLeft(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _horizontalRadius;
            _rayOrigin = Center + Vector2.down * _verticalRange;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _horizontalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.up * (i * _horizontalSpace);

                Hit = Physics2D.Raycast(_rayPos, Vector2.left, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                    {
                        _closestHit = Hit;
                    }
                    
                    collision = true;
                    
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayPos, Vector2.left * (_length), Color.cyan);
            }
            
            // Snap
            if (collision)
            {
                var position = _collider.transform.position;
                position.x = _closestHit.point.x + _horizontalRadius;
                _collider.transform.position = position;
            }

            return collision;
        }

        public void ResetCollisions()
        {
            Collisions = new CollisionData();
        }
    }
}