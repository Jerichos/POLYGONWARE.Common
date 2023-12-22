using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class MenuUI : BaseUI
    {
        [SerializeField] private BaseUI _defaultUI;

        private BaseUI _currentUI;
        private BaseUI _previousUI;
        
        private void Start()
        {
            SwitchUI(_defaultUI);
        }

        public void SwitchUI(BaseUI ui)
        {
            _previousUI = _currentUI;
            _previousUI?.Close();
            _currentUI = ui;
            _currentUI.Open();
        }

        public void Back()
        {
            SwitchUI(_previousUI);
        }
    }
}