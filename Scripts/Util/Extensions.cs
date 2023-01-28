using UnityEngine;

namespace POLYGONWARE.Common
{
    public static class Extensions
    {
        public static void Log(this Transform transform, string logMessage)
        {
            Debug.Log(transform.name + ": " + logMessage, transform);
        }
        
        public static void Log(this GameObject gameObject, string logMessage)
        {
            Debug.Log(gameObject.name + ": " + logMessage, gameObject);
        }
    }
}