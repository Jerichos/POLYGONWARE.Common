using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common
{
    public class Selectable : MonoBehaviour, ISelectable
    {
        [SerializeField] protected bool _isSelected;

        public static event GenericDelegate<ISelectable> ESelected; 

        protected bool IsSelected
        {
            set
            {
                _isSelected = value;
                
                if(_isSelected)
                    OnSelect();
                else
                    OnDeselect();
            }
        }
        
        public static ISelectable Selected;
        
        private bool _hovering;
        
      
        public virtual void OnSelect()
        {
            
        }

        public virtual void OnDeselect()
        {
            
        }

        public virtual void Select()
        {
            Selected?.Deselect();
            Selected = this;
            IsSelected = true;
            ESelected?.Invoke(Selected);
        }

        public virtual void Deselect()
        {
            if(_hovering)
                return;

            Selected = null;
            IsSelected = false;
            ESelected?.Invoke(Selected);
        }
        
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _hovering = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _hovering = false;
        }
    }
}