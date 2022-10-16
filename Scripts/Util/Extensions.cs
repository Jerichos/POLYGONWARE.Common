using UnityEngine;

namespace Common
{
    public static class Extensions
    {
        public static void DebugLog(this Transform transform, string log)
        {
            Debug.Log(transform.name + ": " + log);
        }
    }
}