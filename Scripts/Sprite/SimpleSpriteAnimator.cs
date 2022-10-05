using System;
using UnityEngine;

namespace Common
{
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _rate = 2;

        [SerializeField] private SpriteRenderer _renderer;

        private int _currentSprite;
        
        private void ChangeSprite()
        {
            _renderer.sprite = _sprites[_currentSprite];

            _currentSprite++;
            if (_currentSprite >= _sprites.Length)
                _currentSprite = 0;
        }
        
        private void OnEnable()
        {
            _currentSprite = 0;
            InvokeRepeating(nameof(ChangeSprite), 0, 1f / _rate);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}