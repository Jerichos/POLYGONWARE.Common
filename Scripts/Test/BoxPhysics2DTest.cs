using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Test
{
    public class BoxPhysics2DTest : MonoBehaviour
    {
        public BoxPhysics2D[] _boxPhysics;

        public Vector2 _randomVelocityRange = Vector2.one * 10;

        private Vector2 _velocity;

        public Vector2 _minBoundaries;
        public Vector2 _maxBoundaries;
        
        public void Update()
        {
            foreach (var boxPhysics in _boxPhysics)
            {
                // if (boxPhysics.transform.position.x > _maxBoundaries.x ||
                //     boxPhysics.transform.position.y > _maxBoundaries.y ||
                //     boxPhysics.transform.position.x < _minBoundaries.x ||
                //     boxPhysics.transform.position.y < _minBoundaries.y)
                // {
                //     Debug.Break();
                // }
                
                _velocity.x = Random.Range(-_randomVelocityRange.x, _randomVelocityRange.x);
                _velocity.y = Random.Range(-_randomVelocityRange.y, _randomVelocityRange.y);
                boxPhysics.AddVelocity(_velocity);

            }
        }

        private void OnValidate()
        {
            _boxPhysics = GetComponentsInChildren<BoxPhysics2D>();
        }
    }
}