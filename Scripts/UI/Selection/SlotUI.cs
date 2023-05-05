using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common.UI
{
public abstract class SlotUI : UnityEngine.UI.Selectable
{
    // TODO: this part has to be figured out...
    [SerializeField] protected SlotSelectionUI _selectionGroupParent;

    public int SlotID { get; protected set; }

    private bool _blockDeselect;
    
    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log("OnSelect");
        base.OnSelect(eventData);
        _selectionGroupParent.OnSlotSelected(this);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (_blockDeselect)
        {
            Debug.Log("BLOCKED");
            return;
        }
        
        Debug.Log("OnDeselect");
        base.OnDeselect(eventData);
        _selectionGroupParent.OnSlotDeselected(this);
    }

    public void Deselect()
    {
        base.OnDeselect(new BaseEventData(EventSystem.current));
    }

    public void SetDeselectBlock(bool block)
    {
        _blockDeselect = block;
        
        if(!block)
            Deselect();
    }
}
}