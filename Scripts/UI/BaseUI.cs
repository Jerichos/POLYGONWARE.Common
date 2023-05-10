using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
        [SerializeField] protected Transform _panel;
        
        protected virtual void Awake()
        {
            if (enabled)
            {
                Invoke(nameof(EnableDelayed), 0.01f);
                enabled = false;
            }
        }

        private void EnableDelayed()
        {
            enabled = true;
        }

        public virtual IUI Open()
        {
            // gameObject.Log("Open");
            _panel.gameObject.SetActive(true);
            OnOpen();
            return this;
        }

        public virtual IUI Open<T>(T arg)
        {
            // gameObject.Log("Open T");
            _panel.gameObject.SetActive(true);
            OnOpen();
            return this;
        }

        public virtual IUI Close()
        {
            // gameObject.Log("Close");
            _panel.gameObject.SetActive(false);
            OnClose();
            return this;
        }

        public virtual IUI Toggle()
        {
            // gameObject.Log("toggle");
            var open = !_panel.gameObject.activeInHierarchy;

            return open ? Open() : Close();
        }

        protected virtual void OnOpen()
        {
            
        }

        protected virtual void OnClose()
        {
            
        }
    }
}