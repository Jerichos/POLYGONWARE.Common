using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common.UI
{
    public class DragUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _transform;

        public event GenericDelegate<Vector2> EDrag;
        
        private bool _drag;

        private void Update()
        {
            if(!_drag)
                return;
            
            _transform.position = Mouse.current.position.ReadValue();
            EDrag?.Invoke(_transform.position);
        }

        public void StarDrag()
        {
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        }

        public void StopDrag()
        {
            ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer Down");
            _drag = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer Up");
            _drag = false;
        }
    }
}