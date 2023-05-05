using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common
{
    [Serializable]
    public struct SAttribute<T>
    {
        [SerializeField] private T _value;

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

        public void SetValue(T value)
        {
            Value = value;
        }

        public void InvokeOnChange()
        {
            EChanged?.Invoke(_value);
        }

        public static implicit operator SAttribute<T>(T value)
        {
            return new SAttribute<T> { Value = value };
        }
        
        public static implicit operator T(SAttribute<T> myClassInstance)
        {
            return myClassInstance.Value;
        }

    }
}