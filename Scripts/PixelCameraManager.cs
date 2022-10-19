using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace POLYGONWARE.Common
{
    public class PixelCameraManager : Singleton<PixelCameraManager>
    {
        protected float _halfWidth;
        protected float _halfHeight;
        
         protected PixelPerfectCamera _pixelPerfectCamera;

        protected override void Awake()
        {
            base.Awake();

            _pixelPerfectCamera = GetComponent<PixelPerfectCamera>();

            if (_pixelPerfectCamera)
            {
                _halfWidth = _pixelPerfectCamera.refResolutionX / 2f / _pixelPerfectCamera.assetsPPU;
                _halfHeight = _pixelPerfectCamera.refResolutionY / 2f / _pixelPerfectCamera.assetsPPU;
            }
        }

        public bool IsOnScreen(Vector2 position, Vector2 offset)
        {
            if (position.x < transform.position.x - (_halfWidth + offset.x))
                return false;
            if (position.x > transform.position.x + (_halfWidth + offset.x))
                return false;
            if (position.y > transform.position.y + (_halfHeight + offset.y))
                return false;
            if (position.y < transform.position.y - (_halfHeight + offset.y))
                return false;
            
            return true;
        }
    }
}