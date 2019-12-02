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
    public float Atk;           // 공격력
    public float attackSpeed;   // 공격 속도
    public float jumpCount;     // 점프 횟수
    public float defense;       // 방어력
    public float recovery;      // 회복력
    public float dashDistance;  // 대시 거리

    // 키 아이템으로 장비 강화
    public float up_moveSpeed;
    public float up_Atk;
    public float up_attackSpeed;
    public float up_jumpCount;
    public float up_defense;
    public float up_recovery;
    public float up_dashDistance;

    float[] traningStat;
    float[] limitTraning;
    public float[] traning_count;

    public int currentDate;

    public PlayerEquipment playerEquipment;

    public void Init()
    {
        traningStat = new float[6];
        traningStat[0] = 0;
        traningStat[1] = 0;
        traningStat[2] = 0;
        traningStat[3] = 0;
        traningStat[4] = 0;
        traningStat[5] = 0;

        limitTraning = new float[6];
        limitTraning[0] = 10f;
        limitTraning[1] = 10f;
        limitTraning[2] = 1f;
        limitTraning[3] = 1f;
        limitTraning[4] = 1f;
        limitTraning[5] = 10f;

        traning_count = new float[6];
        int count = traning_count.Length;
        for (int i = 0; i < count; ++i)
        {
            traning_count[i] = 0;
        }

        HP = 100f;
        jumpPower = 7f;

        Atk = 2;
        up_Atk = 0;

        defense = 1;
        up_defense = 0;

        moveSpeed = 4f;
        up_moveSpeed = 0;
        
        attackSpeed = 1;
        up_attackSpeed = 0;

        jumpCount = 1;
        up_jumpCount = 0;

        dashDistance = 1f;
        up_dashDistance = 0;

        recovery = 1f;
        up_recovery = 0;

        maxBuffTime = 100;
    }

    public float[] GetTraningStat()
    {
        return traningStat;
    }
    public float[] GetLimitTraning()
    {
        return limitTraning;
    }

    public void renew()
    {
        up_moveSpeed = playerEquipment.GetStatusValue(0);
        up_Atk = playerEquipment.GetStatusValue(1);
        up_attackSpeed = playerEquipment.GetStatusValue(2);
        up_defense = playerEquipment.GetStatusValue(3);
        up_jumpCount = playerEquipment.GetStatusValue(4);
        up_recovery = playerEquipment.GetStatusValue(5);
        up_dashDistance = playerEquipment.GetStatusValue(6);
    }
}
