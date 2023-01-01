using UnityEngine;

namespace POLYGONWARE.Common
{
    public static class FrameRateLimiter
    {
        [RuntimeInitializeOnLoadMethod]
        public static void LimitFrameRate()
        {
            Application.targetFrameRate = 144;
            Debug.Log("FrameRate limited to " + Application.targetFrameRate);
        }
    }
}