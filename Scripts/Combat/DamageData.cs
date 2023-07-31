using System;
using UnityEngine;

namespace POLYGONWARE.Common.Combat
{
    [Serializable]
    public struct DamageData
    {
        public Component Source;
        public Component Target;
        public Vector3 HitPosition;
        public Vector3 HitDirection;
        public float HitForce;
        public float Damage;
    }
}