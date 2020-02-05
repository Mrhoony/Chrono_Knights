using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Number, ReturnTown, ReturnPreFloor, FreePassNextFloor, FreePassThisFloor, BossFloor, RepeatThisFloor
}
public enum ItemUsingType
{
    health, attack, defense, moveSpeed, bullet     // 1 회복, 2 공버프, 3 방버프, 4 공속버프, 5 이속버프
}

public class Item
{
    public ItemType itemType;
    public ItemUsingType usingType;
    public int usingStatus;
    
    public Sprite sprite;
    public string itemName;
    public int itemRarity;
    public int itemCode;
    public int Value;
    public string Description;
    
    public Item(string _itemName, int _itemRarity, int _itemCode, string _Description, ItemType _itemType, ItemUsingType _usingType, int _usingStatus = 0)
    {
        itemName = _itemName;
        itemRarity = _itemRarity;
        itemCode = _itemCode;
        sprite = Resources.LoadAll<Sprite>("item/ui_itemset")[itemCode];
        Description = _Description;
        itemType = _itemType;
        switch (itemType)
        {
            case ItemType.Number:
                {
                    if(itemRarity < 2)
                    {
                        Value = Random.Range(1, 5);
                    }
                    else
                    {
                        Value = Random.Range(1, 4) * 5;
                    }
                }
                break;
        }
        usingType = _usingType;
        usingStatus = _usingStatus;
    }
}