using System;
using UnityEngine;

namespace POLYGONWARE.Common.Pooling
{
    public class PooledObject : MonoBehaviour
    {
        [HideInInspector] public uint PrefabID;

        public virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public void DeactivateAndReturnToPool()
        {
            gameObject.SetActive(false);
            PoolSystem.ReturnInstance(this);
        }
        
        protected virtual void OnReset()
        {
            
        }

        private void OnEnable()
        {
            OnReset();
        }
    }
}