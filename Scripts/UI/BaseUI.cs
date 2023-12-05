using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
        [SerializeField] private bool _showOnAwake;
        [SerializeField] protected Transform _panel;

        public bool IsOpen => _panel.gameObject.activeSelf;
        
        protected virtual void Awake()
        {
            if (!_panel)
            {
                _panel = transform.Find("panel");
                if(!_panel)
                    gameObject.LogError("could not find child transform of name 'panel' of this UI");
            }
            
            // if (enabled)
            // {
            //     Invoke(nameof(EnableDelayed), 0.01f);
            //     enabled = false;
            // }
            
            if (_showOnAwake)
                Open();
            else
                Close();
        }

        // private void EnableDelayed()
        // {
        //     enabled = true;
        // }

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
            //gameObject.Log("toggle");
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