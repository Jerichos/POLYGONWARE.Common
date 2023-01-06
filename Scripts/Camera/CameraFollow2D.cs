using UnityEngine;

namespace POLYGONWARE.Common.Camera
{
    public class CameraFollow2D : PlayerCamera
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private Vector2 _offset = new Vector2(0, 1);

        private Vector3 _position = new Vector3(0, 0, -10);
        
        protected override void FollowTarget()
        {
            _position = Vector2.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
            _position.z = -10;
            transform.position = _position;
        }
    }
}