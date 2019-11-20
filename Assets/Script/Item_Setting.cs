using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    Sprite sprite;
    string keyName;
    int keyEffect;     // 1 회복, 2 공버프, 3 방버프, 4 공속버프, 5 이속버프
    public int keyRarity;
    int keyCode;

    public string equipName;
    public float equipMoveSpeed;       // 이동 속도
    public int equipAtk;               // 공격력
    public float equipAttackSpeed;     // 공격속도
    public int equipJumpCount;         // 점프 횟수
    public int equipDefense;           // 안정성(방어력)
    public float equipRecovery;        // 회복력
    public float equipDashDistance;    // 대시거리


    public Key(string _keyName, int _keyEffect, int _keyRarity, int _keyCode, string _equipName, int _equipAtk, float _equipAttackSpeed
        , int _equipDefense, float _equipMoveSpeed = 0, int _equipJumpCount = 0, float _equipRecovery = 0, float _equipDashDistance = 0)
    {
        keyName = _keyName;
        keyEffect = _keyEffect;
        keyRarity = _keyRarity;
        sprite = Resources.Load<Sprite>("ItemIcons/34x34icons180709_" + keyCode);

        equipName = _equipName;
    }
}