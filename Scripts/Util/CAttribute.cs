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
}