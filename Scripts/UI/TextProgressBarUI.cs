using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class TextProgressBarUI : ProgressBarUI
    {
        [Header("Text")] 
        [SerializeField] private bool _showPercentage;
        [SerializeField] private TMP_Text _text;

        public override float SetValue(float current, float max)
        {
            float value = base.SetValue(current, max);

            if (_showPercentage)
            {
                float percentage = (float)decimal.Round((decimal)(value * 100), 0);
                percentage = Mathf.Ceil(percentage);
                _text.SetText(percentage.ToString(CultureInfo.InvariantCulture) + "%");
            }
            else
            {
                current = Mathf.Ceil(current);
                _text.SetText(current.ToString(CultureInfo.InvariantCulture));
            }
            
            
            return value;
        }

#if UNITY_EDITOR
        [SerializeField] private string _defaultText;

        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif
    }
}