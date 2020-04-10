using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int maxAmmo;   // 현재 장탄 수
    public readonly int statusAmount = 6;

    public float[] status;            // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount, HP, jumpPower
    public float[] equipmentStatus;   // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount

    private float[] traningStat;
    private float[] limitTraning;
    private int[] traning_count;

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

        equipmentStatus = new float[statusAmount];
        
        equipmentStatus[(int)Status.attack] = 0;
        equipmentStatus[(int)Status.defense] = 0;
        equipmentStatus[(int)Status.moveSpeed] = 0;
        equipmentStatus[(int)Status.attackSpeed] = 0;
        equipmentStatus[(int)Status.dashDistance] = 0;
        equipmentStatus[(int)Status.recovery] = 0;
        
        maxAmmo = 10;
        
        Debug.Log("playerData init");
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
        return equipmentStatus[equipmentStatusNumber];
    }
    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public void ReNew()
    {
        for(int i = 0; i < 6; ++i)
        {
            equipmentStatus[i] = playerEquipment.GetStatusValue(i);
        }
    }
    public PlayerEquipment GetPlayerEquipment()
    {
        return playerEquipment;
    }
}
