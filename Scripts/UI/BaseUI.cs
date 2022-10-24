using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
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
    }
}