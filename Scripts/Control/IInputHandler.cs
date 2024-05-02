using System;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public interface IInputHandler : IDisposable
    {
        void Enable();
    }
}