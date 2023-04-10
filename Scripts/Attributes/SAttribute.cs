using System;
using POLYGONWARE.Common.Util;

namespace POLYGONWARE.Common
{
    public struct SAttribute<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                EChanged?.Invoke(_value);
            }
        }

        public event GenericDelegate<T> EChanged;

        public void InvokeOnChange()
        {
            EChanged?.Invoke(_value);
        }

        public static implicit operator SAttribute<T>(T value)
        {
            return new SAttribute<T> { Value = value };
        }
    }
}