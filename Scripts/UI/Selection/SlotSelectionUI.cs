using System;
using System.Collections.Generic;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
public abstract class SlotSelectionUI<T> : MonoBehaviour
{
    [Header("Slot Selection")]
    [SerializeField] private SlotUI<T> _slotPrefab;
    [SerializeField] private SlotUI<T>[] _predefinedSlots;
    [SerializeField] private Transform _panel;

    private readonly List<SlotUI<T>> _slots = new();

    public event GenericDelegate<T> ESlotSelected;

    public virtual void SetSelected(T value)
    {
        Debug.Log("selected T");
    }

    public virtual void AddButton(T value)
    {
        _panel = !_panel ? transform : _panel;
        var newButton = Instantiate(_slotPrefab, _panel);
        newButton.SetData(value);
        _slots.Add(newButton);
    }
}
}