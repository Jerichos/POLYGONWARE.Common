using System.Collections.Generic;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
public abstract class SlotSelectionUI : BaseUI
{
    [Header("Slot Selection")]
    [SerializeField] private SlotUI _slotPrefab;
    [SerializeField] private SlotUI[] _predefinedSlots;
    [SerializeField] protected Transform _slotPanel;

    protected List<SlotUI> _slots;

    public virtual void OnSlotSelected(SlotUI slot)
    {
    }

    public virtual void OnSlotDeselected(SlotUI slot)
    {
    }

    public virtual SlotUI AddSlot(SlotUI value)
    {
        _slotPanel = !_slotPanel ? transform : _slotPanel;
        var newButton = Instantiate(_slotPrefab, _slotPanel);
        _slots.Add(newButton);
        return newButton;
    }

    public virtual void AddSlot()
    {
        _slotPanel = !_slotPanel ? transform : _slotPanel;
        var newButton = Instantiate(_slotPrefab, _slotPanel);
        _slots.Add(newButton);
    }

}
}