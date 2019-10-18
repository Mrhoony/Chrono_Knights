using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float _moveSpeed;     // 이동 속도
    public float _HP;     // 최대 체력
    public float _MaxBuffTime;   // 현재 버프량

    public int _Atk;  // 공격력
    public float _jumpPower;
    public float _defense;   // 방어력
    public float _maxStability;
    
    public void Init()
    {
        moveSpeed = 4f;
        HP = 30f;
        MaxBuffTime = 100;
        Atk = 2;
        jumpPower = 6f;
        defense = 1f;
        maxStability = 100f;
    }

    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    public float HP
    {
        get { return _HP; }
        set { _HP = value; }
    }
    public float MaxBuffTime
    {
        get { return _MaxBuffTime; }
        set { _MaxBuffTime = value; }
    }
    public int Atk
    {
        get { return _Atk; }
        set { _Atk = value; }
    }
    public float jumpPower
    {
        get { return _jumpPower; }
        set { _jumpPower = value; }
    }
    public float defense
    {
        get { return _defense; }
        set { _defense = value; }
    }
    public float maxStability
    {
        get { return _maxStability; }
        set { _maxStability = value; }
    }
}
