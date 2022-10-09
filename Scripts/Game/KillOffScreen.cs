using UnityEngine;

namespace Common
{
    public class KillOffScreen : MonoBehaviour
    {
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
            if (!CameraManager2D.Instance.IsOnScreen(transform.position, Vector2.one * 2))
            {
                gameObject.SetActive(false);
            }
        }
    }
}