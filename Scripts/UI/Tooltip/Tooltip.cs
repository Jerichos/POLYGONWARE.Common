using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TooltipData _tooltipData;

        public void SetData(TooltipData data)
        {
            _tooltipData = data;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipSystem.Instance.Open(_tooltipData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Instance.Close();
        }
    }
}