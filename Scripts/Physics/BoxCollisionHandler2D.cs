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
        private float _verticalRange;
        private float _horizontalRange;
        private float _verticalSpace;
        private float _horizontalSpace;
        private Vector2 _rayOrigin;

        public CollisionData Collisions;
        public RaycastHit2D ClosestHit;
        
        private BoxCollider2D _collider;

        private Vector2 _bottomLeft     => (Vector2)_transform.position + _localBottomLeft;
        private Vector2 _bottomRight    => (Vector2)_transform.position + _localBottomRight;
        private Vector2 _topLeft        => (Vector2)_transform.position + _localTopLeft;
        private Vector2 _topRight       => (Vector2)_transform.position + _localTopRight;

        private Vector2 _localBottomLeft;
        private Vector2 _localBottomRight;
        private Vector2 _localTopLeft;
        private Vector2 _localTopRight;

        private Vector2 _skinnedSize;
        private Vector2 _fullSize;
        private Vector2 _transformOffset;

        private Vector2 _predictedOffset;

        private Transform _transform;

        private float _hypotenuse;

        private Vector2 Center => (Vector2)_transform.position + _transformOffset;

        public BoxCollisionHandler2D(Transform transform, BoxCollider2D collider, LayerMask layer, float maxRayGap = 0.1f, float skin = 0.15f)
        {
            _layer = layer;
            _collider = collider;
            _transform = transform;
            
            UpdateCollider(collider, maxRayGap, skin);
        }

        public void UpdateCollider(BoxCollider2D collider, float maxRayGap = 0.4f, float skin = 0.15f)
        {
            _collider = collider;

            var bounds = _collider.bounds;
            bounds.Expand(skin * -2);
            
            
            _skinnedSize = bounds.size;
            _fullSize = collider.bounds.size;
            Debug.Log("Skin: " + skin + " FullSize: " + _fullSize + " SkinnedSize: " + _skinnedSize);

            _hypotenuse = Mathf.Sqrt(Mathf.Pow(_skinnedSize.x, 2) + Mathf.Pow(_skinnedSize.x, 2));

            _transformOffset = collider.bounds.center - collider.transform.position;
            
            _localBottomLeft    = new Vector2(-bounds.extents.x, 0)     + _transformOffset;
            _localBottomRight   = new Vector2(0, -bounds.extents.y)      + _transformOffset;
            _localTopLeft       = new Vector2(-bounds.extents.x, 0)      + _transformOffset;
            _localTopRight      = new Vector2(0, bounds.extents.y)       + _transformOffset;
                
            if (bounds.size.y < maxRayGap)
                maxRayGap = bounds.size.y;
            if (bounds.size.x < maxRayGap)
                maxRayGap = bounds.size.x;
            
            _horizontalRayCount = Mathf.FloorToInt(bounds.size.y / maxRayGap) + 1;
            _verticalRayCount = Mathf.FloorToInt(bounds.size.x / maxRayGap) + 1;
            
            _horizontalSpace = bounds.size.y / (_horizontalRayCount - 1);
            _verticalSpace = bounds.size.x / (_verticalRayCount - 1);
            
            Debug.Log("_horizontalRayCount: " + _horizontalRayCount + " space: " + _horizontalSpace);
        }

        public void CheckVerticalCollision(ref Vector2 velocity, bool snap)
        {
            if (velocity.y > 0)
                Collisions.Up = CheckCollisionUp(ref velocity, snap);
            else if (velocity.y < 0)
                Collisions.Down = CheckCollisionDown(ref velocity, snap);
        }

        public void CheckHorizontalCollision(ref Vector2 velocity, bool snap)
        {
            if (velocity.x > 0)
                Collisions.Right = CheckCollisionRight(ref velocity, snap);
            else if (velocity.x < 0)
                Collisions.Left = CheckCollisionLeft(ref velocity, snap);
        }

        public bool CheckDiagonalCollision(ref Vector2 velocity, bool snap)
        {
            var direction = new Vector2(Mathf.Sign(velocity.x), Mathf.Sign(velocity.y)).normalized;
            
            _length = _hypotenuse;
            _rayOrigin = Center;

            Hit = Physics2D.Raycast(_rayOrigin, direction, _length, _layer);

            if (Hit)
            {
                Debug.DrawLine(_rayOrigin, Hit.point, Color.red);

                if (snap)
                {
                    // var position = _collider.transform.position;
                    // position.x = Hit.point.x - _fullSize.x / 2f * Mathf.Sign(direction.x);
                    // position.y = direction.y > 0? Hit.point.y - _fullSize.y * 2 : Hit.point.y;
                    // _collider.transform.position = position;
                    
                    Debug.Log("Snap Diagonal");
                    velocity.x = 0;
                }
                
                Collisions.Diagonal = true;
                
                return true;
            }
            
            Debug.DrawRay(_rayOrigin, direction * _length, Color.cyan);
            return false;
        }

        public bool CheckCollisionUp(ref Vector2 velocity, bool snap)
        {
            _length = _fullSize.y / 2 + Mathf.Abs(velocity.y) * Time.deltaTime;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _verticalRayCount; i++)
            {
                _rayOrigin = _bottomLeft + _predictedOffset;
                _rayOrigin += Vector2.right * (i * _verticalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.up, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                        _closestHit = Hit;
                    
                    collision = true;
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.up * (_length), Color.cyan);
            }
            
            if (collision && snap)
            {
                var position = _collider.transform.position;
                position.y = _closestHit.point.y - _fullSize.y;
                _collider.transform.position = position;
                velocity.y = 0;
            }

            return collision;
        }
        
        public bool CheckCollisionDown(ref Vector2 velocity, bool snap)
        {
            _length = _fullSize.y / 2 + Mathf.Abs(velocity.y) * Time.deltaTime;

            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };

            for (int i = 0; i < _verticalRayCount; i++)
            {
                _rayOrigin = _bottomLeft + _predictedOffset;
                _rayOrigin += Vector2.right * (i * _verticalSpace);
                Debug.Log(i + " _rayOrigin " + _rayOrigin + " _length " + _length + " _verticalSpace " + _verticalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.down, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                        _closestHit = Hit;

                    collision = true;
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.down * _length, Color.cyan);
            }

            if (collision && snap)
            {
                var position = _collider.transform.position;
                position.y = _closestHit.point.y;
                _collider.transform.position = position;
                
                Debug.Log("Snap Down");
                velocity.y = 0;
            }

            return collision;
        }
        
        public bool CheckCollisionRight(ref Vector2 velocity, bool snap)
        {
            _length = _fullSize.x / 2 + Mathf.Abs(velocity.x) * Time.deltaTime;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _horizontalRayCount; i++)
            {
                _rayOrigin = _bottomRight;  
                _rayOrigin += Vector2.up * (i * _horizontalSpace);
                // Debug.Log(i + " _rayOrigin " + _rayOrigin + " _length " + _length + " direction " + Vector2.down);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.right, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                    {
                        _closestHit = Hit;
                    }

                    collision = true;
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.right * _length, Color.cyan);
            }

            // Snap
            if (collision && snap)
            {
                var position = _collider.transform.position;
                position.x = _closestHit.point.x - _fullSize.x / 2;
                _collider.transform.position = position;
                
                Debug.Log("Snap Right");
                velocity.x = 0;
            }
            else
            {
                _predictedOffset = (Vector2.right) * (velocity.x * Time.deltaTime);
            }
            
            return collision;
        }
        
        public bool CheckCollisionLeft(ref Vector2 velocity, bool snap)
        {
            _length = _fullSize.x / 2 + Mathf.Abs(velocity.x) * Time.deltaTime;
            
            var collision = false;
            var _closestHit = new RaycastHit2D
            {
                distance = float.MaxValue
            };
            
            for (int i = 0; i < _horizontalRayCount; i++)
            {
                _rayOrigin = _bottomRight;
                _rayOrigin += Vector2.up * (i * _horizontalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.left, _length, _layer);
                
                if (Hit)
                {
                    if (Hit.distance < _closestHit.distance)
                    {
                        _closestHit = Hit;
                    }
                    
                    collision = true;
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.left * (_length), Color.cyan);
            }
            
            // Snap
            if (collision && snap)
            {
                var position = _collider.transform.position;
                position.x = _closestHit.point.x + _fullSize.x / 2;
                _collider.transform.position = position;
                
                velocity.x = 0;
            }
            else
            {
                _predictedOffset = (Vector2.right) * (velocity.x * Time.deltaTime);
            }

            return collision;
        }

        public void SnapToClosestHit(Vector2 velocity)
        {
            var position = _collider.transform.position;
            _collider.transform.position = position;
        }

        public void ResetHandler()
        {
            Collisions = new CollisionData();
            ClosestHit = new RaycastHit2D();
            _predictedOffset = Vector2.zero;
        }
        
        public void DrawGizmo()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_bottomLeft, 0.05f);
            Gizmos.DrawSphere(_bottomRight, 0.05f);
            Gizmos.DrawSphere(_topLeft, 0.05f);
            Gizmos.DrawSphere(_topRight, 0.05f);
        }
    }
}