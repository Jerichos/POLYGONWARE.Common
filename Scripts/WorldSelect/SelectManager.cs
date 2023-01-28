using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public class SelectManager : Singleton<SelectManager>
    {
        [SerializeField] private GameObject _selected;
        [SerializeField] private LayerMask _selectMask;

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("1 select");

                if (Selectable.Selected != null)
                {
                    Debug.Log("2 selected");
                    Selectable.Selected.Deselect();
                }
            }
        }
    }
}