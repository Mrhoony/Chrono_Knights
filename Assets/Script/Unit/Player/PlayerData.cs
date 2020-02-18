using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int maxAmmo;   // 현재 장탄 수

    public float[] status;      // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount, HP, jumpPower
    public float[] equipmentStatus;    // attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount

    private float[] traningStat;
    private float[] limitTraning;
    private int[] traning_count;

    public PlayerEquipment playerEquipment;

    public void Init()
    {
        playerEquipment = new PlayerEquipment();

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

        traning_count = new int[6];
        int count = traning_count.Length;
        for (int i = 0; i < count; ++i)
        {
            traning_count[i] = 0;
        }

        status = new float[9];
        equipmentStatus = new float[7];

        status[7] = 100f;
        status[8] = 7f;

        status[0] = 1f;
        equipmentStatus[0] = 0;

        status[1] = 1f;
        equipmentStatus[1] = 0;

        status[2] = 4f;
        equipmentStatus[2] = 0;

        status[3] = 1f;
        equipmentStatus[3] = 0;

        status[4] = 1f;
        equipmentStatus[4] = 0;

        status[5] = 1f;
        equipmentStatus[5] = 0;

        status[6] = 2f;
        equipmentStatus[6] = 0;

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
        for(int i = 0; i < 7; ++i)
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
