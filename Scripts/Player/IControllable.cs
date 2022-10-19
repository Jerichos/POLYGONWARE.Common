using UnityEngine;

namespace POLYGONWARE.Common.Player
{
    public interface IControllable
    {
        Transform Transform { get; }
        public IController Controller { get; }
        void TakeControl(IController owner);
    }
}