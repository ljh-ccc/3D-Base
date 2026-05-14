using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Item_New", menuName = "RPG Data/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Default data")]
    public string itemId;
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    [TextArea]
    public string description;

    [Header("Currency Data")]
    public int buyPrice;
    public int sellPrice;

    [Header("Used")]
    public bool canUse;
    public bool canStack;
    public int maxStackCount = 99;

    [Header("Use Effect")]
    public List<ItemEffect> effects = new List<ItemEffect>();
}

