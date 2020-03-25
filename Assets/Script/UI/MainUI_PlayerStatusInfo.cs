﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusInfo : MonoBehaviour
{
    public GameObject[] equipmentSlot;
    public GameObject statusinformation;
    
    PlayerStatus playerStatus;
    PlayerData playerData;
    PlayerEquipment playerEquipment;
    public PlayerEquipment.Equipment[] equipment;

    private void Awake()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStatus.playerData;
    }

    public void OnStatusMenu()
    {
        equipment = playerStatus.playerData.GetPlayerEquipment().equipment;

        for (int i = 0; i < 7; ++i)
        {
            equipmentSlot[i].transform.GetChild(0).gameObject.SetActive(true);
            equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = SpriteSet.itemSprite[i];
            if (equipment[i].itemCode != 0)
            {
                equipmentSlot[i].transform.GetChild(1).gameObject.SetActive(true);
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.enchantSlotImage[equipment[i].itemRarity];
            }
            else
            {
                equipmentSlot[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        statusinformation.transform.GetChild(0).GetComponent<Text>().text = playerStatus.GetAttack_Result().ToString();
        statusinformation.transform.GetChild(1).GetComponent<Text>().text = playerStatus.GetDefence_Result().ToString();
        statusinformation.transform.GetChild(2).GetComponent<Text>().text = playerStatus.GetMoveSpeed_Result().ToString();
        statusinformation.transform.GetChild(3).GetComponent<Text>().text = playerStatus.GetAttackSpeed_Result().ToString();
        statusinformation.transform.GetChild(4).GetComponent<Text>().text = playerStatus.GetDashDistance_Result().ToString();
        statusinformation.transform.GetChild(5).GetComponent<Text>().text = playerStatus.GetRecovery_Result().ToString();
        statusinformation.transform.GetChild(6).GetComponent<Text>().text = playerStatus.GetJumpCount().ToString();
        statusinformation.transform.GetChild(7).GetComponent<Text>().text = playerData.GetStatus(7).ToString();
    }
}
