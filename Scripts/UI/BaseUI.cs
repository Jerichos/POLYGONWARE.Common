using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public abstract class BaseUI : MonoBehaviour, IUI
    {
        private void Awake()
        {
            Close();
        }

        public virtual IUI Open()
        {
            gameObject.SetActive(true);
            return this;
        }

        public virtual IUI Open<T>(T arg)
        {
            gameObject.SetActive(true);
            return this;
        }

        public virtual IUI Close()
        {
            gameObject.SetActive(false);
            return this;
        }

        public virtual IUI Toggle()
        {
            var open = !gameObject.activeInHierarchy;
            gameObject.SetActive(open);
            return this;
        }
    }
}