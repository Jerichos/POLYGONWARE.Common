using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class TooltipSystem : Singleton<TooltipSystem>
    {
        [SerializeField] private TooltipUI _tooltip;

        public void Open(TooltipData data)
        {
            _tooltip.Open(data);
        }

        public void Close()
        {
            _tooltip.Close();
        }
    }
}