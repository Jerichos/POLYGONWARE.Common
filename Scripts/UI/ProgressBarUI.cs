using System;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float _value;
        
        [Header("Settings")]
        [SerializeField] private Color _fillColor = Color.white;
        [SerializeField] private Color _backgroundColor = Color.gray;
        [SerializeField] private Color _frameColor = Color.black;

        [Header("References")]
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _frameImage;

        public virtual float SetValue(float current, float max)
        {
            float value =  current / max;
            
            if(Math.Abs(value - _value) < 0.005)
                return _value;

            _value = value;
            UpdateBar(_value);
            return value;
        }

        protected virtual void UpdateBar(float value)
        {
            _fillImage.fillAmount = Mathf.Clamp(value, 0, 1);
        }
        
#if UNITY_EDITOR
        [SerializeField] private bool _inverse;
        protected virtual void OnValidate()
        {
            _fillImage.color = _fillColor;
            _fillImage.fillOrigin = _inverse ? 1 : 0;
            
            _frameImage.color = _frameColor;
            
            _backgroundImage.color = _backgroundColor;
            
            UpdateBar(_value);
        }
#endif
    }
}