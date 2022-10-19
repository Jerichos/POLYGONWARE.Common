using System;
using UnityEngine;

namespace Common
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public Type type;

        public MonoScriptAttribute(Type newType)
        {
            type = newType;
            Debug.Log("Name of: " + type);
        }
    }
}