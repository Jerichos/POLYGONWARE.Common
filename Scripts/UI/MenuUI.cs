using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class MenuUI : BaseUI
    {
        [SerializeField] private BaseUI _defaultUI;

        private IUI _currentUI;
        private IUI _previousUI;
        
        private void Start()
        {
            SwitchUI(_defaultUI);
        }

        public void SwitchUI(IUI ui)
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