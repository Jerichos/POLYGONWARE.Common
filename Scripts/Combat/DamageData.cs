using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct DamageData
    {
        public GameObject Source;
        public GameObject Target;
        public float Damage;
    }
}