using System;
using UnityEngine;

namespace Common.Combat
{
    [Serializable]
    public struct DamageData
    {
        public GameObject Source;
        public GameObject Target;
        public float Damage;
    }
}