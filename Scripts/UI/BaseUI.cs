using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
        private void Awake()
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
            gameObject.SetActive(true);
            return this;
        }

        public virtual IUI Open<T>(T arg)
        {
            // gameObject.Log("Open T");
            gameObject.SetActive(true);
            return this;
        }

        public virtual IUI Close()
        {
            // gameObject.Log("Close");
            OnClose();
            gameObject.SetActive(false);
            return this;
        }

        public virtual IUI Toggle()
        {
            // gameObject.Log("toggle");
            var open = !gameObject.activeInHierarchy;
            gameObject.SetActive(open);
            return this;
        }

        protected virtual void OnClose() { }
    }
}