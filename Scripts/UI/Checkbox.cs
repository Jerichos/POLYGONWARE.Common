using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{
    [RequireComponent(typeof(Button))]
    public class Checkbox : MonoBehaviour
    {
        [SerializeField] private bool _check;
        [SerializeField] private Image _checkImage;
        [SerializeField] private Button _button;

        public event GenericDelegate<bool> ECheck; 

        public bool Check
        {
            set
            {
                _check = value;
                
                _checkImage.gameObject.SetActive(_check);
                ECheck?.Invoke(_check);
            }
        }

        public void Toggle()
        {
            Check = !_check;
        }
        
        public void SetWithoutNotify(bool value)
        {
            _check = value;
            _checkImage.gameObject.SetActive(_check);
        }

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Toggle);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void OnValidate()
        {
            Check = _check;
            _button = GetComponent<Button>();
        }

        
    }
}