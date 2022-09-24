using System;
using UnityEngine;

namespace Common
{
    public class DisableAfterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Start()
        {
            Invoke(nameof(DisableMe), _animator.runtimeAnimatorController.animationClips[0].length);
        }

        private void DisableMe()
        {
            gameObject.SetActive(false);
        }
    }
}