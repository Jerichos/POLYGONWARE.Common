using System;
using System.Collections.Generic;
using UnityEngine;

namespace POLYGONWARE.Common
{
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _slotCount = 10;
    [SerializeField] private PredefinedItem[] _predefinedItems;

    public ItemSlot[] Slots;

    private void Awake()
    {
        Slots = new ItemSlot[_slotCount];
    }

    public void SortInventory()
    {
        
    }

    public void AddItem(Item item)
    {
        
    }

    public Item RemoveItem(int slotID)
    {
        var item = Slots[slotID].Item;
        
        Slots[slotID].Count = 0;
        Slots[slotID].Item = null;

        return item;
    }
}

[Serializable]
public struct ItemSlot
{
    public int Count;
    public Item Item;
}

[Serializable]
public struct PredefinedItem
{
    public ItemSO ItemSO;
    public int Count;

    public bool IsEmpty => Count == 0 || ItemSO == null;
}
}