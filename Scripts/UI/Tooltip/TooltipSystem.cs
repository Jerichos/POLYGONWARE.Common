using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class TooltipSystem : Singleton<TooltipSystem>
    {
        [SerializeField] private TooltipPanelUI _tooltipPanel;

        protected override void Awake()
        {
            base.Awake();
            
            _tooltipPanel.Close();
        }

        public void Open(TooltipData data)
        {
            _tooltipPanel.Open(data);
        }

        public void Close()
        {
            _tooltipPanel.Close();
        }
    }
}