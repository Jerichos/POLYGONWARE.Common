using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Test
{
    [SerializeField]
    public class RandomVelocityTest : MonoBehaviour
    {
        public BoxPhysics2D _boxPhysics;
        public Vector2 _randomVelocityRange = Vector2.one * 10;

        public void Update()
        {
            Vector2 velocity;
            velocity.x = Random.Range(-_randomVelocityRange.x, _randomVelocityRange.x);
            velocity.y = Random.Range(-_randomVelocityRange.y, _randomVelocityRange.y);
            _boxPhysics.Move(velocity);
        }
    }
}