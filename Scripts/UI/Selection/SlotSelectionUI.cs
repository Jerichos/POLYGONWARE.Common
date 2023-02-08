using System;
using System.Collections.Generic;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
public class SlotSelectionUI<T> : MonoBehaviour
{
    [SerializeField] private T _slotPrefab;
    [SerializeField] private T[] _predefinedSlots;

    private readonly List<T> _slots = new();

    public event GenericDelegate<int> ESlotSelected;

    public virtual void SetSelected<T2>(SlotUI<T2> slotUI)
    {
        Debug.Log("selected T");
    }
}
}