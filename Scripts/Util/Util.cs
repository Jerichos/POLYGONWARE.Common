using UnityEngine;

namespace POLYGONWARE.Common.Util
{
    public static class Util
    {
        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return layerMask.value == 1 << layer;
        }
    }
}