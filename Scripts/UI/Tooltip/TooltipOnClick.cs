using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
    public class TooltipOnClick : Tooltip, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OpenTooltip();
        }
    }
}