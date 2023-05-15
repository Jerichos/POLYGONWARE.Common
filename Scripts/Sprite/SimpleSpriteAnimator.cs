using UnityEngine;

namespace POLYGONWARE.Common
{
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Sprite[] _sprites;
        [SerializeField] private float _rate = 2;
        [SerializeField] private bool _disableAfterAnimation;
        [SerializeField] private SpriteRenderer _renderer;

        private int _currentSprite;
        
        private void ChangeSprite()
        {
            _renderer.sprite = _sprites[_currentSprite];

            _currentSprite++;
            if (_currentSprite >= _sprites.Length)
            {
                if(_disableAfterAnimation)
                    gameObject.SetActive(false);
                
                _currentSprite = 0;
            }
        }
        
        private void OnEnable()
        {
            float cooldown = 1f / _rate;
            _currentSprite = 0;
            _renderer.sprite = _sprites[_currentSprite];
            InvokeRepeating(nameof(ChangeSprite), cooldown, cooldown);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}