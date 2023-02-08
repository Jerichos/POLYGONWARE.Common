using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
public class SlotUI<T> : UnityEngine.UI.Selectable
{
    // TODO: this part has to be figured out...
    [SerializeField] private SlotSelectionUI<T> _selectionGroupParent;
    
    public event GenericDelegate<T> ESelected;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _selectionGroupParent.SetSelected(this);
    }
}
}