using POLYGONWARE.Common.Physics;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class BoxRaycast2D : MonoBehaviour
    {
        private readonly LayerMask _layer;
        private readonly BoxCollider2D _collider;
        private readonly Transform _transform;
        private Vector3 _skinnedSize;
        private Vector2 _fullSize;
        private float _hypotenuse;
        private Vector2 _transformOffset;
        private Vector2 _localBottomLeft;
        private Vector2 _localBottomRight;
        private int _horizontalRayCount;
        private int _verticalRayCount;
        private float _horizontalSpace;
        private float _verticalSpace;
        private float _verticalRayGap;
        private float _horizontalRayGap;

        private Vector2 _left => (Vector2)_transform.position + _localBottomLeft;
        private Vector2 _bottom => (Vector2)_transform.position + _localBottomRight;
        
        public Hit2D[] HorizontalHits { get; set; }
        public Hit2D[] VerticalHits { get; set; }

        public Hit2D ClosestVerticalHit
        {
            get
            {
                Hit2D hit = VerticalHits[0];
                
                for (int i = 0; i < VerticalHits.Length; i++)
                {
                    if (VerticalHits[i].collision && VerticalHits[i].distance < hit.distance)
                        hit = VerticalHits[i];
                }

                return hit;
            }
        }
        
        public Hit2D ClosestHorizontalHit
        {
            get
            {
                Hit2D hit = HorizontalHits[0];
                
                for (int i = 0; i < HorizontalHits.Length; i++)
                {
                    if (HorizontalHits[i].collision && HorizontalHits[i].distance < hit.distance)
                        hit = HorizontalHits[i];
                }

                return hit;
            }
        }

        public BoxRaycast2D(Transform transform, BoxCollider2D collider, LayerMask collisionLayer, float maxRayGap = 0.1f, float skin = 0.01f)
        {
            _layer = collisionLayer;
            _collider = collider;
            _transform = transform;
            
            Update(maxRayGap, skin);
        }
        
        public void Update(float maxRayGap = 0.1f, float skin = 0.01f)
        {
            var bounds = _collider.bounds;
            bounds.Expand(skin * -2);
            
            _skinnedSize = bounds.size;
            _fullSize = _collider.bounds.size;

            _hypotenuse = Mathf.Sqrt(Mathf.Pow(_fullSize.x /2, 2) + Mathf.Pow(_fullSize.y /2, 2));

            _transformOffset = _collider.bounds.center - _collider.transform.position;
            
            _localBottomLeft    = new Vector2(-bounds.size.x /2, 0) + _transformOffset;
            _localBottomRight   = new Vector2(0, -bounds.size.y /2) + _transformOffset;
            
            Debug.Log("_transformOffset: " + _transformOffset + " _localBottomLeft: " + _localBottomLeft);

            _horizontalRayGap = maxRayGap;
            _verticalRayGap = maxRayGap;
            
            if (bounds.size.y < maxRayGap)
                _horizontalRayGap = bounds.size.y;
            
            if (bounds.size.x < maxRayGap)
                _verticalRayGap = bounds.size.x;
            
            _horizontalRayCount = Mathf.FloorToInt(bounds.size.y / _horizontalRayGap) + 1;
            _verticalRayCount = Mathf.FloorToInt(bounds.size.x / _verticalRayGap) + 1;
            
            _horizontalSpace = bounds.size.y / (_horizontalRayCount - 1);
            _verticalSpace = bounds.size.x / (_verticalRayCount - 1);

            Debug.Log("_verticalRayCount: " + _verticalRayCount + " _collider.size.x: " + bounds.size.x + " _verticalRayGap: " + _verticalRayGap + " left: " + _left);
            VerticalHits = new Hit2D[_verticalRayCount];
            HorizontalHits = new Hit2D[_horizontalRayCount];
        }

        public bool VerticalCollision(float deltaY)
        {
            if (deltaY == 0)
                return false;

            VerticalHits = new Hit2D[VerticalHits.Length];
            
            var _length = _fullSize.y / 2 + Mathf.Abs(deltaY);
            var direction = Mathf.Sign(deltaY) * Vector2.up;

            var collision = false;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                var _rayOrigin = _left + Vector2.right * (i * _verticalSpace);

                var Hit = Physics2D.Raycast(_rayOrigin, direction, _length, _layer);
                
                if (Hit)
                {
                    collision = true;
                    VerticalHits[i].distance = Hit.distance - _fullSize.y / 2;
                    VerticalHits[i].collision = true;
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, direction * _length, Color.cyan);
            }

            return collision;
        }
        
        public bool HorizontalCollision(float deltaX)
        {
            if (deltaX == 0)
                return false;
            
            HorizontalHits = new Hit2D[HorizontalHits.Length];
            
            var _length = _fullSize.y / 2 + Mathf.Abs(deltaX);
            var direction = Mathf.Sign(deltaX) * Vector2.right;

            var collision = false;

            for (int i = 0; i < _horizontalRayCount; i++)
            {
                var _rayOrigin = _bottom + Vector2.up * (i * _verticalSpace);

                var Hit = Physics2D.Raycast(_rayOrigin, direction, _length, _layer);
                
                if (Hit)
                {
                    collision = true;
                    HorizontalHits[i].distance = Hit.distance - _fullSize.y / 2;
                    HorizontalHits[i].collision = true;
                    
                    Debug.DrawLine(_rayOrigin, Hit.point, Color.red);
                }
                else
                    Debug.DrawRay(_rayOrigin, direction * _length, Color.cyan);
            }

            return collision;
        }

     
    }

    public struct Hit2D
    {
        public bool collision;
        public float distance;
    }
}