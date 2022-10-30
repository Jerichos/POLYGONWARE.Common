using System;
using UnityEngine;

namespace POLYGONWARE.Common.Util
{
    public class CAttribute<T> where T: IComparable
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if(_value.Equals(value))
                    return;
                
                _value = value;
                EChanged?.Invoke(_value);
            }
        }

        public event GenericDelegate<T> EChanged;

        public CAttribute(){ }
        public CAttribute(T value)
        {
            _value = value;
        }
    }
    
    public class CReadOnlyAttribute<T> where T: IComparable
    {
        private T _value;

        public T Value
        {
            get => _value;
            private set
            {
                if (_value.Equals(value))
                    return;
                _value = value;
                EChanged?.Invoke(_value);
            }
        }

        public event GenericDelegate<T> EChanged;

        public CReadOnlyAttribute(){ }
        public CReadOnlyAttribute(T value)
        {
            _value = value;
        }
    }

    [Serializable]
    public struct SAttribute<T> where T : IComparable
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if(_value.Equals(value))
                    return;

                _value = value;
                EChanged?.Invoke(_value);
            }
        }

        public event GenericDelegate<T> EChanged;
    }
}