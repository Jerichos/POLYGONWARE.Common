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

        public BoxCollisionHandler2D(BoxCollider2D collider, LayerMask layer, float maxGap = 0.1f, float skin = 0.15f, float rangeSquish = 0.98f)
        {
            _layer = layer;
            _collider = collider;
            
            var size = _collider.size;

            _verticalRadius = size.y / 2;
            _verticalRange = size.y * rangeSquish;
            
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

            _verticalRadius = size.y / 2;
            _verticalRange = size.y * rangeSquish;
            
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
            Collisions.Up = false;
            Collisions.Down = false;

            if (speed > 0)
                Collisions.Up = CheckCollisionUp(speed);
            else if (speed < 0)
                Collisions.Down = CheckCollisionDown(speed);
        }

        public void CheckHorizontalCollision(float speed)
        {
            Collisions.Right = false;
            Collisions.Left = false;
            
            if (speed > 0)
                Collisions.Right = CheckCollisionRight(speed);
            else if (speed < 0)
                Collisions.Left = CheckCollisionLeft(speed);
        }

        public bool CheckCollisionUp(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _skin;
            _rayOrigin = (Vector2)_collider.bounds.center + Vector2.up * _verticalRadius + Vector2.down * _skin - _horizontalRadius * Vector2.right;
            
            for (int i = 0; i < _verticalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.right * (i * _verticalSpace);
                // Debug.Log(i + " Ray " + _rayOrigin+ " _length " + _length+ " _rayPos " + _rayPos+ " direction " + Vector2.up);

                Hit = Physics2D.Raycast(_rayPos, Vector2.up, _length, _layer);
                
                if (Hit)
                {
                    Debug.Log(_collider.transform.name + " hits " + Hit.transform.name);
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);

                    // Snap here?
                    var position = _collider.transform.position;
                    position.y = Hit.point.y - _collider.size.y;
                    _collider.transform.position = position;
                    
                    return true;
                }
                
                Debug.DrawRay(_rayPos, Vector2.up * (_length), Color.cyan);
            }

            return false;
        }
        
        public bool CheckCollisionDown(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _skin;
            _rayOrigin = (Vector2)_collider.bounds.center + Vector2.down * _verticalRadius + Vector2.up * _skin - _horizontalRadius * Vector2.right;
            
            for (int i = 0; i < _verticalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.right * (i * _verticalSpace);
                // Debug.Log(i + " Ray " + _rayOrigin+ " _length " + _length+ " _rayPos " + _rayPos+ " direction " + Vector2.down);

                Hit = Physics2D.Raycast(_rayPos, Vector2.down, _length, _layer);
                
                if (Hit)
                {
                    Debug.Log(_collider.transform.name + " hits " + Hit.transform.name);
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);

                    // Snap here?
                    var position = _collider.transform.position;
                    position.y = Hit.point.y;
                    _collider.transform.position = position;
                    return true;
                }
                
                Debug.DrawRay(_rayPos, Vector2.down * (_length), Color.cyan);
            }

            return false;
        }
        
        public bool CheckCollisionRight(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _skin;
            _rayOrigin = (Vector2)_collider.bounds.center + Vector2.right * _horizontalRadius + Vector2.left * _skin + Vector2.down * _verticalRadius;
            
            for (int i = 0; i < _horizontalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.up * (i * _horizontalSpace);
                // Debug.Log(i + " Ray " + _rayOrigin+ " _length " + _length+ " _rayPos " + _rayPos+ " direction " + Vector2.down);

                Hit = Physics2D.Raycast(_rayPos, Vector2.right, _length, _layer);
                
                if (Hit)
                {
                    Debug.Log(_collider.transform.name + " hits " + Hit.transform.name);
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);

                    // Snap here?
                    var position = _collider.transform.position;
                    position.x = Hit.point.x - _horizontalRadius;
                    _collider.transform.position = position;
                    return true;
                }
                
                Debug.DrawRay(_rayPos, Vector2.right * (_length), Color.cyan);
            }

            return false;
        }
        
        public bool CheckCollisionLeft(float speed)
        {
            _length = (Mathf.Abs(speed) * Time.deltaTime) + _skin;
            _rayOrigin = (Vector2)_collider.bounds.center + Vector2.left * _horizontalRadius + Vector2.right * _skin + Vector2.down * _verticalRadius;
            
            for (int i = 0; i < _horizontalRayCount + 1; i++)
            {
                _rayPos = _rayOrigin + Vector2.up * (i * _horizontalSpace);
                // Debug.Log(i + " Ray " + _rayOrigin+ " _length " + _length+ " _rayPos " + _rayPos+ " direction " + Vector2.down);

                Hit = Physics2D.Raycast(_rayPos, Vector2.left, _length, _layer);
                
                if (Hit)
                {
                    Debug.Log(_collider.transform.name + " hits " + Hit.transform.name);
                    Debug.DrawLine(_rayPos, Hit.point, Color.red);

                    // Snap here?
                    var position = _collider.transform.position;
                    position.x = Hit.point.x + _horizontalRadius;
                    _collider.transform.position = position;
                    return true;
                }
                
                Debug.DrawRay(_rayPos, Vector2.left * (_length), Color.cyan);
            }

            return false;
        }

        // public bool CheckCollision(Vector2 velocity)
        // {
        //     Collision = false;
        //
        //     var direction = velocity.normalized;
        //     _length = (velocity.magnitude * 1.001f) + _skin;
        //     _rayOrigin = (Vector2)_boxCollider.bounds.center + direction * _radius - Axis * _verticalRange - direction * _skin;
        //
        //     for (int i = 0; i < _verticalRayCount + 1; i++)
        //     {
        //         _rayPos = _rayOrigin + Axis * (i * _verticalSpace);
        //
        //         Hit = Physics2D.Raycast(_rayPos, direction, _length, _layer);
        //         
        //         if (Hit)
        //         {
        //             Debug.DrawLine(_rayPos, Hit.point, Color.red);
        //             HitRange = Vector2.Distance(_rayPos, Hit.point) - _skin;
        //             
        //             Collision = true;
        //             return Collision;
        //         }
        //         Debug.DrawRay(_rayPos, direction * (_length), Color.blue);
        //     }
        //
        //     return Collision;
        // }
        
    }
}