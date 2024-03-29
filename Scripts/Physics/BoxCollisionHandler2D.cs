﻿using UnityEngine;

namespace POLYGONWARE.Common
{
    public class BoxCollisionHandler2D
    {
        public RaycastHit2D Hit;

        private LayerMask _layer;
        
        private int _verticalRayCount;
        private int _horizontalRayCount;
        
        private float _length;
        private float _verticalSpace;
        private float _horizontalSpace;
        private float _hypotenuse;
        
        public CollisionData Collisions;
        private BoxCollider2D _collider;

        private Vector2 _left     => (Vector2)_transform.position + _localBottomLeft;
        private Vector2 _bottom    => (Vector2)_transform.position + _localBottomRight;

        private Vector2 _rayOrigin;
        private Vector2 _localBottomLeft;
        private Vector2 _localBottomRight;

        private Vector2 _skinnedSize;
        private Vector2 _fullSize;
        private Vector2 _transformOffset;
        private Vector2 _predictedOffset;

        private Transform _transform;

        public Vector2 Center => (Vector2)_transform.position + _transformOffset;
        
        private BoxPhysics2D[] VerticalHits;
        private BoxPhysics2D[] HorizontalHits;

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

            _hypotenuse = Mathf.Sqrt(Mathf.Pow(_fullSize.x /2, 2) + Mathf.Pow(_fullSize.y /2, 2));

            _transformOffset = collider.bounds.center - collider.transform.position;
            
            _localBottomLeft    = new Vector2(-bounds.extents.x, 0) + _transformOffset;
            _localBottomRight   = new Vector2(0, -bounds.extents.y) + _transformOffset;
            // _localTopLeft       = new Vector2(-bounds.extents.x, 0)      + _transformOffset;
            // _localTopRight      = new Vector2(0, bounds.extents.y)       + _transformOffset;
                
            if (bounds.size.y < maxRayGap)
                maxRayGap = bounds.size.y;
            if (bounds.size.x < maxRayGap)
                maxRayGap = bounds.size.x;
            
            _horizontalRayCount = Mathf.FloorToInt(bounds.size.y / maxRayGap) + 1;
            _verticalRayCount = Mathf.FloorToInt(bounds.size.x / maxRayGap) + 1;
            
            _horizontalSpace = bounds.size.y / (_horizontalRayCount - 1);
            _verticalSpace = bounds.size.x / (_verticalRayCount - 1);

            VerticalHits = new BoxPhysics2D[_verticalRayCount];
            HorizontalHits = new BoxPhysics2D[_horizontalRayCount];

            // Debug.Log("_horizontalRayCount: " + _horizontalRayCount + " space: " + _horizontalSpace);
        }

        public void CheckVerticalCollision(ref Vector2 velocity, bool snap)
        {
            if (velocity.y > 0)
                Collisions.Up = CheckCollisionUp(ref velocity, snap);
            else if (velocity.y < 0)
                Collisions.Down = CheckCollisionDown(ref velocity, snap);
        }

        public void CheckHorizontalCollision(ref Vector2 velocityDelta, bool snap)
        {
            if (velocityDelta.x > 0)
                Collisions.Right = CheckCollisionRight(ref velocityDelta);
            else if (velocityDelta.x < 0)
                Collisions.Left = CheckCollisionLeft(ref velocityDelta);
        }

        public bool CheckDiagonalCollision(ref Vector2 velocity, bool snap)
        {
            var direction = new Vector2(Mathf.Sign(velocity.x) * _fullSize.x, Mathf.Sign(velocity.y) * _fullSize.y).normalized;
            
            _length = _hypotenuse + velocity.magnitude;
            _rayOrigin = Center;

            Hit = Physics2D.Raycast(_rayOrigin, direction, _length, _layer);

            if (Hit)
            {
                Debug.DrawLine(_rayOrigin, Hit.point, Color.red);

                if (snap)
                {
                    velocity.x = 0;
                }
                
                Collisions.Diagonal = true;
                return true;
            }
            
            Debug.DrawRay(_rayOrigin, direction * _length, Color.cyan);
            return false;
        }

        public bool CheckCollisionUp(ref Vector2 deltaVelocity, bool snap)
        {
            VerticalHits.Clear();
            
            _length = _fullSize.y / 2 + Mathf.Abs(deltaVelocity.y);

            var collision = false;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                _rayOrigin = _left + _predictedOffset;
                _rayOrigin += Vector2.right * (i * _verticalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.up, _length, _layer);
                
                if (Hit)
                {
                    var otherBox = VerticalHits.Add(Hit.transform.GetInstanceID());
                    if (otherBox)
                    {
                        collision = true;
                        
                        Vector2 pushVelocity;
                        pushVelocity.x = 0;
                        var gap = (Hit.distance - _fullSize.y / 2);
                        pushVelocity.y = deltaVelocity.y - gap;
                        
                        var push = otherBox.PushVerticallyVirtually(ref pushVelocity).y;
                        otherBox.Gap = gap;

                        var delta = push + gap;

                        if (delta < deltaVelocity.y)
                        {
                            deltaVelocity.y = delta;
                        }
                    }
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.up * _length, Color.cyan);
            }

            for (int i = 0; i < VerticalHits.Length; i++)
            {
                if(!VerticalHits[i])
                    continue;

                VerticalHits[i].PerformVerticalPush(deltaVelocity.y - VerticalHits[i].Gap);
            }

            return collision;
        }
        
        public bool CheckCollisionDown(ref Vector2 deltaVelocity, bool snap)
        {
            VerticalHits.Clear();
            _length = _fullSize.y / 2 + Mathf.Abs(deltaVelocity.y);

            var collision = false;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                _rayOrigin = _left + _predictedOffset;
                _rayOrigin += Vector2.right * (i * _verticalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.down, _length, _layer);
                
                if (Hit)
                {
                    var otherBox = VerticalHits.Add(Hit.transform.GetInstanceID());
                    if (otherBox)
                    {
                        collision = true;
                        
                        Vector2 pushVelocity;
                        pushVelocity.x = 0;
                        var gap = (Hit.distance - _fullSize.y / 2);
                        pushVelocity.y = deltaVelocity.y + gap;
                        
                        var push = otherBox.PushVerticallyVirtually(ref pushVelocity).y;
                        otherBox.Gap = gap;

                        var delta = push - gap;

                        if (delta > deltaVelocity.y)
                        {
                            deltaVelocity.y = delta;
                        }
                    }
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.down * _length, Color.cyan);
            }

            for (int i = 0; i < VerticalHits.Length; i++)
            {
                if(!VerticalHits[i])
                    continue;

                VerticalHits[i].PerformVerticalPush(deltaVelocity.y + VerticalHits[i].Gap);
            }

            return collision;
        }
        
        public bool CheckCollisionRight(ref Vector2 deltaVelocity)
        {
            HorizontalHits.Clear();
            
            _length = _fullSize.x / 2 + Mathf.Abs(deltaVelocity.x);

            var collision = false;

            for (int i = 0; i < _horizontalRayCount; i++)
            {
                _rayOrigin = _bottom;  
                _rayOrigin += Vector2.up * (i * _horizontalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.right, _length, _layer);
                
                if (Hit)
                {
                    var otherBox = HorizontalHits.Add(Hit.transform.GetInstanceID());
                    if (otherBox)
                    {
                        collision = true;
                        
                        Vector2 pushVelocity;
                        pushVelocity.y = 0;
                        var gap = (Hit.distance - _fullSize.x / 2);
                        pushVelocity.x = deltaVelocity.x - gap;
                        
                        var push = otherBox.PushHorizontallyVirtually(ref pushVelocity).x;
                        otherBox.Gap = gap;

                        var delta = push + gap;

                        if (delta < deltaVelocity.x)
                        {
                            deltaVelocity.x = delta;
                        }
                    }
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.right * _length, Color.cyan);
            }

            for (int i = 0; i < HorizontalHits.Length; i++)
            {
                if(!HorizontalHits[i])
                    continue;

                HorizontalHits[i].PerformHorizontalPush(deltaVelocity.x - HorizontalHits[i].Gap);
            }

            if(collision == false)
                _predictedOffset = (Vector2.right) * (deltaVelocity.x * Time.deltaTime);
            
            return collision;
        }
        
        public bool CheckCollisionLeft(ref Vector2 deltaVelocity)
        {
            HorizontalHits.Clear();
            
            _length = _fullSize.x / 2 + Mathf.Abs(deltaVelocity.x);

            var collision = false;

            for (int i = 0; i < _horizontalRayCount; i++)
            {
                _rayOrigin = _bottom;  
                _rayOrigin += Vector2.up * (i * _horizontalSpace);

                Hit = Physics2D.Raycast(_rayOrigin, Vector2.left, _length, _layer);
                
                if (Hit)
                {
                    var otherBox = HorizontalHits.Add(Hit.transform.GetInstanceID());
                    if (otherBox)
                    {
                        collision = true;
                        
                        Vector2 pushVelocity;
                        pushVelocity.y = 0;
                        var gap = (Hit.distance - _fullSize.x / 2);
                        pushVelocity.x = deltaVelocity.x + gap;
                        
                        var push = otherBox.PushHorizontallyVirtually(ref pushVelocity).x;
                        otherBox.Gap = gap;

                        var delta = push - gap;
                        
                        Debug.Log("delta: "+ delta + " deltaVel: " + deltaVelocity);

                        if (delta > deltaVelocity.x)
                        {
                            deltaVelocity.x = delta;
                        }
                    }
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, Vector2.left * _length, Color.cyan);
            }

            for (int i = 0; i < HorizontalHits.Length; i++)
            {
                if(!HorizontalHits[i])
                    continue;

                HorizontalHits[i].PerformHorizontalPush(deltaVelocity.x + HorizontalHits[i].Gap);
            }

            if(collision == false)
                _predictedOffset = (Vector2.right) * (deltaVelocity.x * Time.deltaTime);
            
            return collision;
        }

        public RaycastHit2D GetHitFromDirection(Vector2 direction, float length, LayerMask layer)
        {
            return Physics2D.Raycast(Center, direction.normalized, length, layer);
        }

        public void ResetHandler()
        {
            Collisions = new CollisionData();
            _predictedOffset = Vector2.zero;
            HorizontalHits.Clear();
            VerticalHits.Clear();
        }
        
        public void DrawGizmo()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_left, 0.05f);
            Gizmos.DrawSphere(_bottom, 0.05f);
        }
    }
}