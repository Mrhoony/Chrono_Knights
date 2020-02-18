using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusInfo : MonoBehaviour
{
    public GameObject[] playerStatusInfo;
    public GameObject[] equipmentSlot;
    public GameObject currentProgress;
    public GameObject statusinformation;
    Sprite[] equipmentBorder;
    Sprite[] inventorySet;
    PlayerStatus playerStatus;
    PlayerData playerData;
    PlayerEquipment playerEquipment;
    public PlayerEquipment.Equipment[] equipment;

    private void Awake()
    {
        equipmentBorder = Resources.LoadAll<Sprite>("UI/ui_status_set");
        inventorySet = Resources.LoadAll<Sprite>("UI/Inventory_Set");

        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStatus.playerData;
    }

    public void OnStatusMenu()
    {
        playerEquipment = playerStatus.playerData.GetPlayerEquipment();
        equipment = playerEquipment.equipment;
        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                equipmentSlot[i].GetComponent<Image>().sprite = Database_Game.instance.GetItem(equipment[i].itemCode).sprite;
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = equipmentBorder[Database_Game.instance.GetItem(equipment[i].itemCode).itemRarity];
            }
            else
            {
                equipmentSlot[i].GetComponent<Image>().sprite = inventorySet[6];
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = inventorySet[6];
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
