using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int maxAmmo;   // 현재 장탄 수
    public readonly int statusAmount = 6;

    public float[] status;      // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount, HP, jumpPower
    public float[] equipmentStatus;    // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount

    private float[] traningStat;
    private float[] limitTraning;
    private int[] traning_count;

    public PlayerEquipment playerEquipment;

    public void Init()
    {
        playerEquipment = new PlayerEquipment();

        status = new float[9];
        status[(int)PlayerStat.attack] = 1f;
        status[(int)PlayerStat.defense] = 1f;
        status[(int)PlayerStat.moveSpeed] = 1f;
        status[(int)PlayerStat.attackSpeed] = 1f;
        status[(int)PlayerStat.dashDistance] = 1f;
        status[(int)PlayerStat.recovery] = 1f;

        status[(int)PlayerStat.jumpCount] = 2f;
        status[(int)PlayerStat.HP] = 100f;
        status[(int)PlayerStat.jumpPower] = 7f;

        traningStat = new float[statusAmount];
        traningStat[(int)PlayerStat.attack] = 0;
        traningStat[(int)PlayerStat.defense] = 0;
        traningStat[(int)PlayerStat.moveSpeed] = 0;
        traningStat[(int)PlayerStat.attackSpeed] = 0;
        traningStat[(int)PlayerStat.dashDistance] = 0;
        traningStat[(int)PlayerStat.recovery] = 0;

        limitTraning = new float[statusAmount];
        limitTraning[(int)PlayerStat.attack] = 10f;
        limitTraning[(int)PlayerStat.defense] = 10f;
        limitTraning[(int)PlayerStat.moveSpeed] = 1f;
        limitTraning[(int)PlayerStat.attackSpeed] = 1f;
        limitTraning[(int)PlayerStat.dashDistance] = 1f;
        limitTraning[(int)PlayerStat.recovery] = 10f;

        traning_count = new int[statusAmount];
        for (int i = 0; i < statusAmount; ++i)
        {
            traning_count[i] = 0;
        }

        equipmentStatus = new float[statusAmount];
        
        equipmentStatus[(int)PlayerStat.attack] = 0;
        equipmentStatus[(int)PlayerStat.defense] = 0;
        equipmentStatus[(int)PlayerStat.moveSpeed] = 0;
        equipmentStatus[(int)PlayerStat.attackSpeed] = 0;
        equipmentStatus[(int)PlayerStat.dashDistance] = 0;
        equipmentStatus[(int)PlayerStat.recovery] = 0;



        maxAmmo = 10;
        
        playerEquipment.PlayerEquipmentInit();

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

    public void renew(PlayerEquipment _playerEquipment)
    {
        playerEquipment = _playerEquipment;
        for(int i = 0; i < 6; ++i)
        {
            equipmentStatus[i] = playerEquipment.GetStatusValue(i);
        }
        Debug.Log("player renew");
    }

    public PlayerEquipment GetPlayerEquipment()
    {
        return playerEquipment;
    }
}
