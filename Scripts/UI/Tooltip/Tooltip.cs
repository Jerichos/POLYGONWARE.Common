using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
    public class Tooltip : MonoBehaviour, IDeselectHandler
    {
        private GetCallback<TooltipData> _getTooltipDataCallback;

        public void SetupTooltip(GetCallback<TooltipData> callback)
        {
            _getTooltipDataCallback = callback;
        }

        protected void OpenTooltip()
        {
            if (_getTooltipDataCallback == null)
            {
                Debug.LogError("SetupTooltip was not called.");
                return;
            }
            
            var _tooltipData = _getTooltipDataCallback.Invoke();
            TooltipSystem.Instance.Open(_tooltipData);
            EventSystem.current.SetSelectedGameObject(gameObject);
            
            Debug.Log("OpenTooltip");
        }

        protected void CloseTooltip()
        {
            TooltipSystem.Instance.Close();
            Debug.Log("CloseTooltip");
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log("OnDeselect");
            CloseTooltip();
        }
    }
}