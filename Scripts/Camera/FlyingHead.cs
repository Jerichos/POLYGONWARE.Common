using UnityEngine;

namespace POLYGONWARE.Common
{
    public class FlyingHead : Controllable
    {
        [SerializeField] private float _horizontalSpeed = 5;
        [SerializeField] private float _verticalSpeed = 5;
        
        public void HorizontalMove(Vector3 direction)
        {
            direction.y = 0;
            transform.Translate(direction * _horizontalSpeed * Time.deltaTime);
        }

        public void VerticalMove(float y)
        {
            transform.Translate(y * Vector3.up * _verticalSpeed * Time.deltaTime);
        }
    }
}