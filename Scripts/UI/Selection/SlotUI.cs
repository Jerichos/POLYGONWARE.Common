using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common.UI
{
public abstract class SlotUI<T> : UnityEngine.UI.Selectable
{
    // TODO: this part has to be figured out...
    [SerializeField] protected SlotSelectionUI<T> _selectionGroupParent;

    private T _value;
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _selectionGroupParent.SetSelected(_value);
    }

    public virtual void SetData(T value)
    {
        _value = value;
    }
}
}