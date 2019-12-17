using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Number, ReturnTown, ReturnPreFloor, FreePassNextFloor, FreePassThisFloor, BossFloor, RepeatThisFloor
}

[System.Serializable]
public class Key
{
    public ItemType Type;
    
    public Sprite[] sprites;
    public Sprite sprite;
    public string keyName;
    public int keyEffect;     // 1 회복, 2 공버프, 3 방버프, 4 공속버프, 5 이속버프
    public int keyRarity;
    public int keyCode;
    public int Value;
    public string Description;
    
    public Key(string _keyName, int _keyEffect, int _keyRarity, int _keyCode, ItemType type, string _Description)
    {
        keyName = _keyName;
        keyEffect = _keyEffect;
        keyRarity = _keyRarity;
        sprites = Resources.LoadAll<Sprite>("item/ItemIcon");
        sprite = sprites[_keyCode];
        Description = _Description;
        switch (type)
        {
            case ItemType.Number:
                {
                    if(keyRarity < 2)
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
    }
}