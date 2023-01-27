using UnityEngine;

namespace POLYGONWARE.Common
{
    public struct CooldownTimer
    {
        public float Cooldown;

        private float _lastTime;

        public CooldownTimer(float cooldown)
        {
            Cooldown = cooldown;
            _lastTime = -cooldown;
        }

        public bool TimePassed()
        {
            return Time.realtimeSinceStartup - _lastTime > Cooldown;
        }

        public void Reset()
        {
            _lastTime = Time.realtimeSinceStartup;
        }
    }
}