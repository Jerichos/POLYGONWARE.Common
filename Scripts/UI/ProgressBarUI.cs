using System;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        [SerializeField][Range(0, 1)] private float _value;

        public float Value
        {
            set
            {
                if(Math.Abs(value - _value) < 0.005)
                    return;

                _value = value;
                UpdateBar(_value);
            }
        }

        private void UpdateBar(float value)
        {
            _value = value;
            var size = _fill.transform.localScale;
            size.x = _value;
            _fill.transform.localScale = size;
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateBar(_value);
        }
#endif
    }
}