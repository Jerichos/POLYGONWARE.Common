using System;
using UnityEngine;

namespace POLYGONWARE.Common.Combat
{
    [Serializable]
    public struct DamageData
    {
        public Component Source;
        public Component Target;
        public float Damage;
    }
}