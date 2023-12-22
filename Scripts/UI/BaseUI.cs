using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour
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
        public virtual void Open()
        {
            // gameObject.Log("Open");
            _panel.gameObject.SetActive(true);
            OnOpen();
        }

        public virtual void Open<T>(T arg)
        {
            // gameObject.Log("Open T");
            _panel.gameObject.SetActive(true);
            OnOpen();
        }

        public virtual void Close()
        {
            // gameObject.Log("Close");
            _panel.gameObject.SetActive(false);
            OnClose();
        }

        public virtual void Toggle()
        {
            //gameObject.Log("toggle");
            var open = !_panel.gameObject.activeInHierarchy;

            if(open)
                Open();
            else
                Close();
        }

        protected virtual void OnOpen()
        {
            
        }

        protected virtual void OnClose()
        {
            
        }
    }
}