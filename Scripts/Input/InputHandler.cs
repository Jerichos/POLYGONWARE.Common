using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public abstract class InputHandler : MonoBehaviour, IInputHandler
    {
        public abstract void Dispose();
    }
}