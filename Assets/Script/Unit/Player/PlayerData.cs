using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float HP;            // 최대 체력
    public float maxBuffTime;   // 현재 버프량
    public float jumpPower;     // 점프력

    public float moveSpeed;     // 이동 속도
    public int Atk;             // 공격력
    public float attackSpeed;   // 공격 속도
    public float jumpCount;     // 점프 횟수
    public int defense;       // 방어력
    public float recovery;      // 회복력
    public float dashDistance;  // 대시 거리

    // 키 아이템으로 장비 강화
    public float up_moveSpeed;
    public int up_Atk;
    public float up_attackSpeed;
    public float up_jumpCount;
    public int up_defense;
    public float up_recovery;
    public float up_dashDistance;

    // 트레이닝으로 스탯 강화
    public float Traning_moveSpeed;
    public int Traning_Atk;
    public float Traning_attackSpeed;
    public float Traning_jumpCount;
    public int Traning_defense;
    public float Traning_recovery;
    public float Traning_dashDistance;

    public int[] limitUpgrade;
    public int currentDate;

    public void Init()
    {
        HP = 100f;

        moveSpeed = 4f;
        Traning_moveSpeed = 0;
        up_moveSpeed = 0;
        
        Atk = 2;
        Traning_Atk = 0;
        up_Atk = 0;
        
        attackSpeed = 1;
        Traning_attackSpeed = 0;
        up_attackSpeed = 0;

        jumpPower = 6f;

        jumpCount = 1;
        Traning_jumpCount = 0;
        up_jumpCount = 0;

        defense = 0;
        Traning_defense = 0;
        up_defense = 0;

        recovery = 10f;
        Traning_recovery = 0;
        up_recovery = 0;

        dashDistance = 10f;
        Traning_dashDistance = 0;
        up_dashDistance = 0;

        maxBuffTime = 100;
        limitUpgrade = new int[7];

        for(int i = 0; i<7; ++i)
        {
            limitUpgrade[i] = 10;
        }
    }
}
