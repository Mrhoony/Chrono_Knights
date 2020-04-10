using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int maxAmmo;   // 현재 장탄 수
    public readonly int statusAmount = 6;

    public float[] status;            // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount, HP, jumpPower

    public float[] traningStat;
    public float[] limitTraning;
    public int[] traning_count;

    public PlayerEquipment playerEquipment;

    public void Init()
    {
        playerEquipment = new PlayerEquipment();
        playerEquipment.Init();

        status = new float[9];
        status[(int)Status.attack] = 1f;
        status[(int)Status.defense] = 1f;
        status[(int)Status.moveSpeed] = 1f;
        status[(int)Status.attackSpeed] = 1f;
        status[(int)Status.dashDistance] = 1f;
        status[(int)Status.recovery] = 1f;

        status[(int)Status.jumpCount] = 2f;
        status[(int)Status.HP] = 100f;
        status[(int)Status.jumpPower] = 7f;

        traningStat = new float[statusAmount];
        traningStat[(int)Status.attack] = 0;
        traningStat[(int)Status.defense] = 0;
        traningStat[(int)Status.moveSpeed] = 0;
        traningStat[(int)Status.attackSpeed] = 0;
        traningStat[(int)Status.dashDistance] = 0;
        traningStat[(int)Status.recovery] = 0;

        limitTraning = new float[statusAmount];
        limitTraning[(int)Status.attack] = 10f;
        limitTraning[(int)Status.defense] = 10f;
        limitTraning[(int)Status.moveSpeed] = 1f;
        limitTraning[(int)Status.attackSpeed] = 1f;
        limitTraning[(int)Status.dashDistance] = 1f;
        limitTraning[(int)Status.recovery] = 10f;

        traning_count = new int[statusAmount];
        for (int i = 0; i < statusAmount; ++i)
        {
            traning_count[i] = 0;
        }
        
        maxAmmo = 10;
    }

    public int[] GetTraningCount()
    {
        return traning_count;
    }
    public float[] GetTraningStat()
    {
        return traningStat;
    }
    public float[] GetLimitTraning()
    {
        return limitTraning;
    }
    public float GetStatus(int StatusNumber)
    {
        return status[StatusNumber];
    }
    public float GetEquipmentStatus(int equipmentStatusNumber)
    {
        return playerEquipment.GetStatusValue(equipmentStatusNumber);
    }
    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
    
    public PlayerEquipment GetPlayerEquipment()
    {
        return playerEquipment;
    }
}
