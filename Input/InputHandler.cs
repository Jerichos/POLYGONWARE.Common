using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public abstract class InputHandler : IInputHandler
    {
        public abstract void Dispose();
        public void Enable()
        {
            throw new System.NotImplementedException();
        }
    }
}