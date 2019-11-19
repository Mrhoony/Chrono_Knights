using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    Sprite sprite;
    string _keyName;
    int _keyEffect;
    int _keyMultiple;
    int _keyRarity;
    int _keyCode;
    
    public Key(){}

    public Key(string keyName, int keyEffect, int keyRarity, int keyCode)
    {
        _keyName = keyName;
        _keyEffect = keyEffect;
        _keyRarity = keyRarity;
        sprite = Resources.Load<Sprite>("ItemIcons/34x34icons180709_" + keyCode);
    }

    public Key(string keyName, int keyEffect, int keyMul, int keyRarity, int keyCode)
    {
        _keyName = keyName;
        _keyEffect = keyEffect;
        _keyMultiple = keyMul;
        _keyRarity = keyRarity;
        sprite = Resources.Load<Sprite>("ItemIcons/34x34icons180709_" + keyCode);
    }
}

public class Equip
{
    string _equipKategorie;
    string _equipName;
    int _baseAtk;
    int _baseDef;
    int _baseAddAtk;
    int _baseAddDef;
    int _rarity;
    float AddVariation;
    float MinVariation;
    EquipSkill _equipSkill;

    public Equip(string equipKategorie, string equipName, int baseAtk, int baseDef, int rarity, EquipSkill equipSkill)
    {
        _equipKategorie = equipKategorie;
        _equipName = equipName;
        switch (rarity)
        {
            case 1:
                AddVariation = Random.Range(0, 81) * 0.01f;
                break;
            case 2:
                AddVariation = Random.Range(40, 101) * 0.01f;
                break;
            case 3:
                AddVariation = Random.Range(100, 151) * 0.01f;
                MinVariation = Random.Range(40, 101) * 0.01f;
                break;
        }

        _baseAddAtk = (int)(baseAtk * AddVariation);
        _baseAddDef = (int)(baseDef * AddVariation);
    }
}

public class EquipSkill
{

}