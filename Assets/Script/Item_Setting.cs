using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    Sprite sprite;
    public string keyName;
    public int keyEffect;     // 1 회복, 2 공버프, 3 방버프, 4 공속버프, 5 이속버프
    public int keyRarity;
    public int keyCode;

    public string equipName;
    
    public Key(string _keyName, int _keyEffect, int _keyRarity, int _keyCode)
    {
        keyName = _keyName;
        keyEffect = _keyEffect;
        keyRarity = _keyRarity;
        sprite = Resources.Load<Sprite>("ItemIcons/34x34icons180709_" + keyCode);
    }
}