using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public interface IControllable
    {
        Type InputHandlerType { get; }
        Transform Transform { get; }
        public IController Controller { get; }
        void TakeControl(IController owner);
    }
}