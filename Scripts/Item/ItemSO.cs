using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
[CreateAssetMenu(fileName = "Item", menuName = "Data/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemData _itemData;
    public ItemData ItemData => _itemData;
}

[Serializable]
public struct ItemData
{
    public Sprite Icon;
    public string Name;
    public int MaxStack;
}

}