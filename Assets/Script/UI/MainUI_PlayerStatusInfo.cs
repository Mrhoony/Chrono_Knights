using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusInfo : MonoBehaviour
{
    public GameObject[] playerStatusInfo;
    public GameObject[] equipmentSlot;
    Sprite[] equipmentBorder;
    Sprite[] inventorySet;
    PlayerStatus playerStatus;
    PlayerData playerData;

    PlayerEquipment playerEquipment;
    public PlayerEquipment.Equipment[] equipment;

    private void Awake()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStatus.playerData;
        playerEquipment = playerData.GetPlayerEquipment();
        equipmentBorder = Resources.LoadAll<Sprite>("UI/ui_status_set");
        inventorySet = Resources.LoadAll<Sprite>("UI/Inventory_Set");
    }

    public void OnStatusMenu()
    {
        equipment = playerEquipment.equipment;
        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                equipmentSlot[i].GetComponent<Image>().sprite = Item_Database.instance.GetItem(equipment[i].itemCode).sprite;
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = equipmentBorder[Item_Database.instance.GetItem(equipment[i].itemCode).keyRarity];
            }
            else
            {
                equipmentSlot[i].GetComponent<Image>().sprite = inventorySet[6];
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = inventorySet[6];
            }
        }
    }
}
