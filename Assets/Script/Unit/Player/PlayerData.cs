using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float moveSpeed;     // 이동 속도
    public float HP;     // 최대 체력
    public int Atk;  // 공격력
    public float attackSpeed;  // 공격 속도
    public float defense;   // 방어력
    public float jumpPower;    // 점프력
    public float maxBuffTime;  // 현재 버프량
    public float maxStability; //최대 안정성

    // 키 아이템으로 스탯 강화
    public float up_moveSpeed;
    public float up_HP;
    public int up_Atk;
    public float up_attackSpeed;
    public float up_jumpPower;
    public float up_defense;

    // 트레이닝으로 스탯 강화
    public float Traning_moveSpeed;
    public float Traning_HP;
    public int Traning_Atk;
    public float Traning_attackSpeed;
    public float Traning_jumpPower;
    public float Traning_defense;

    public int[] limitUpgrade;
    public int currentDate;

    public void Init()
    {
        moveSpeed = 4f;
        Traning_moveSpeed = 0;
        up_moveSpeed = 0;

        HP = 30f;
        Traning_HP = 0;
        up_HP = 0;

        Atk = 2;
        Traning_Atk = 0;
        up_Atk = 0;

        attackSpeed = 1;
        Traning_attackSpeed = 0;
        up_attackSpeed = 0;

        jumpPower = 6f;
        Traning_jumpPower = 0;
        up_jumpPower = 0;

        defense = 1f;
        Traning_defense = 0;
        up_defense = 0;

        maxBuffTime = 100;
        maxStability = 100f;
        limitUpgrade = new int[7];

        for(int i = 0; i<7; ++i)
        {
            limitUpgrade[i] = 10;
        }
    }
}
