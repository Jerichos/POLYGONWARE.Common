using UnityEngine;

namespace Common
{
    public static class Util
    {
        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return layerMask.value == 1 << layer;
        }
    }
}