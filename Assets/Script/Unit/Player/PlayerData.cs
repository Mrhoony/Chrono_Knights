using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float HP;     // 최대 체력
    public float moveSpeed;     // 이동 속도
    public int Atk;  // 공격력
    public float attackSpeed;  // 공격 속도
    public float defense;   // 방어력
    public float jumpPower;    // 점프력
    public float maxBuffTime;  // 현재 버프량
    public float maxStability; //최대 안정성

    public float up_HP;
    public float up_moveSpeed;
    public int up_Atk;
    public float up_attackSpeed;
    public float up_jumpPower;
    public float up_defense;

    public int[] limitUpgrade;
    public int currentDate;

    public void Init()
    {
        moveSpeed = 4f;
        up_moveSpeed = 0;
        HP = 30f;
        up_HP = 0;
        maxBuffTime = 100;
        Atk = 2;
        up_Atk = 0;
        attackSpeed = 1;
        up_attackSpeed = 0;
        jumpPower = 6f;
        up_jumpPower = 0;
        defense = 1f;
        up_defense = 0;
        maxStability = 100f;
        limitUpgrade = new int[7];
        for(int i = 0; i<7; ++i)
        {
            limitUpgrade[i] = 10;
        }
    }
}
