using System;

namespace Common
{
    public delegate void SAttributeDelegate<in T>(T value);
    
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
                EChanged.Invoke(_value);
            }
        }

        public event SAttributeDelegate<T> EChanged;
    }
}