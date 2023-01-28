using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common
{
    public class Selectable : MonoBehaviour, ISelectable
    {
        public static ISelectable Selected;
        
        public event VoidDelegate ESelected;
        public event VoidDelegate EDeselected;

        private bool _hovering;
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        public virtual void Select()
        {
            Selected?.Deselect();
            ESelected?.Invoke();
            Selected = this;
        }

        public virtual void Deselect()
        {
            if(_hovering)
                return;

            Selected = null;
            EDeselected?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hovering = false;
        }
    }
}