using System;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class CurrentMaxUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentText;
        [SerializeField] private TMP_Text _maxText;

        public float Current
        {
            set => _currentText.SetText(Mathf.Ceil(value).ToString("0"));
        }

        public float Max
        {
            set => _maxText.SetText(Mathf.Ceil(value).ToString("0"));
        }
        
#if UNITY_EDITOR
        [SerializeField] private float _currentValue;
        [SerializeField] private float _maxValue;

        private void OnValidate()
        {
            Current = _currentValue;
            Max = _maxValue;
        }
#endif
    }
}