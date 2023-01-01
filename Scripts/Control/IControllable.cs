using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public interface IControllable
    {
        Transform Transform { get; }
        public IController Controller { get; }
        void TakeControl(IController owner);
        Type InputHandlerType { get; }
    }
}