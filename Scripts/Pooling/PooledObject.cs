using UnityEngine;

namespace Common
{
    public class PooledObject : MonoBehaviour
    {
        [HideInInspector] public uint PrefabID;

        public virtual void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}