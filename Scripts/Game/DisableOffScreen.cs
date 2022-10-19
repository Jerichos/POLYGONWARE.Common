using POLYGONWARE.Common.Pooling;
using UnityEngine;

namespace POLYGONWARE.Common.Game
{
    public class DisableOffScreen : MonoBehaviour
    {
        private Vector2 _offset = Vector2.one * 2;

        public Vector2 Offset
        {
            set => _offset = value;
        }
        
        private void OnEnable()
        {
            InvokeRepeating(nameof(CheckOnScreen), 1, 0.1f);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void CheckOnScreen()
        {
            if (!PixelCameraManager.Instance.IsOnScreen(transform.position, _offset))
            {
                gameObject.SetActive(false);
                
                var pooled = GetComponent<PooledObject>();
                if (pooled)
                    PoolSystem.ReturnInstance(pooled);
            }
        }

    }
}