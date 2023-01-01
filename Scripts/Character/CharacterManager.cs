using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CharacterManager : Controllable
    {
        [SerializeField] private CharacterPhysics _physics;

        public CharacterPhysics Physics => _physics;
        public override Type InputHandlerType => typeof(CharacterInputHandler);
    }
}