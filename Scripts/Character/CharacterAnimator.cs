using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterPhysics _physics;

        private void Update()
        {
            _animator.SetFloat(CharacterAnimationProperties.Speed, _physics.Velocity.magnitude);
        }
    }
}