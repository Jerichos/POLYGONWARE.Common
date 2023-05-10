using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{

public class ButtonSlotUI : Button
{
    [Header("Slot")]
    [SerializeField] private Image _imageIcon;

    public GenericDelegate<int> ClickCallback;

    public void SetSlot(UnityEngine.Sprite icon, GenericDelegate<int> clickCallback)
    {
        _imageIcon.sprite = icon;
        ClickCallback = clickCallback;
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        ClickCallback?.DynamicInvoke();
    }
}
}