using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct CooldownTimer
    {
        public readonly float Cooldown;
        private float _lastTime;

        public CooldownTimer(float cooldown)
        {
            if (cooldown <= 0)
                cooldown = 0.01f;
            
            Cooldown = cooldown;
            _lastTime = -cooldown;
        }
        
        public CooldownTimer(float cooldown, CooldownTimer timer)
        {
            this = timer;
            
            if (cooldown <= 0)
                cooldown = 0.01f;

            if (_lastTime == 0)
                _lastTime = -cooldown;
            
            Cooldown = cooldown;
        }

        public bool IsReady => Time.time - _lastTime > Cooldown;
        public float TimeLeft => Cooldown - (Time.time - _lastTime);

        public void Restart()
        {
            _lastTime = Time.time;
        }

        public void Restart(float time)
        {
            _lastTime = time;
        }

        public void Refresh()
        {
            _lastTime = -Cooldown;
        }
    }
}