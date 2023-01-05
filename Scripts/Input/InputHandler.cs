using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public abstract class InputHandler : MonoBehaviour, IInputHandler
    {
        public abstract InputHandler CreateHandler(IInputActionCollection2 inputAction, IControllable controllable);
        public abstract void Dispose();
    }
}