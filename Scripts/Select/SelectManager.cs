using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public class SelectManager : Singleton<SelectManager>
    {
        [SerializeField] private GameObject _selected;
        [SerializeField] private LayerMask _selectMask;

        private void Update()
        {
            if(Utils.IsPointerOverUI())
                return;
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Selectable.Selected != null)
                {
                    Selectable.Selected.Deselect();
                }
            }
        }
    }
}