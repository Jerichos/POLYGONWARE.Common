using System;

namespace Common
{
    public interface IInputHandler : IDisposable
    {
        void Update();
    }
}