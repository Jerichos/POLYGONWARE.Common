﻿using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct CooldownTimer
    {
        private readonly float _cooldown;
        private float _lastTime;

        public CooldownTimer(float cooldown)
        {
            if (cooldown <= 0)
                cooldown = 0.01f;
            
            _cooldown = cooldown;
            _lastTime = -cooldown;
        }
        
        public CooldownTimer(float cooldown, CooldownTimer timer)
        {
            this = timer;
            
            if (cooldown <= 0)
                cooldown = 0.01f;

            if (_lastTime == 0)
                _lastTime = -cooldown;
            
            _cooldown = cooldown;
        }

        public bool IsReady => Time.time - _lastTime > _cooldown;
        public float TimeLeft => _cooldown - (Time.time - _lastTime);

        public void Restart()
        {
            _lastTime = Time.time;
        }

        public void Refresh()
        {
            _lastTime = -_cooldown;
        }
    }
}