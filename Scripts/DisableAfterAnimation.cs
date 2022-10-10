﻿using System;
using UnityEngine;

namespace Common
{
    public class DisableAfterAnimation : PooledObject
    {
        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            Invoke(nameof(DisableMe), _animator.runtimeAnimatorController.animationClips[0].length);
        }

        private void DisableMe()
        {
            gameObject.SetActive(false);
            PoolSystem.ReturnInstance(this);
        }
    }
}