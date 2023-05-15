using System;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common
{
public class ProxyTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    
    public GenericDelegate<Collider> OnEnter;
    public GenericDelegate<Collider> OnExit;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"ProxyEnter {other.gameObject.name}");
        OnEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"ProxyExit {other.gameObject.name}");
        OnExit?.Invoke(other);
    }
}
}