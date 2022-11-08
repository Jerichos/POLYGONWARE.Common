using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
    public class TooltipOnHover : Tooltip, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            OpenTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CloseTooltip();
        }
    }
}