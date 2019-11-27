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
    PlayerStatus pStatus;
    PlayerData pData;

    PlayerEquipment playerEquipment;
    public PlayerEquipment.Equipment[] equipment;

    private void Awake()
    {
        pStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        pData = pStatus.playerData;
        playerEquipment = pData.playerEquipment;
        equipmentBorder = Resources.LoadAll<Sprite>("UI/ui_status_set");
        inventorySet = Resources.LoadAll<Sprite>("UI/Inventory_Set");
    }

    public void OnStatusMenu()
    {
        equipment = playerEquipment.equipment;
        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].key != null)
            {
                equipmentSlot[i].GetComponent<Image>().sprite = equipment[i].key.sprite;
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = equipmentBorder[equipment[i].key.keyRarity];
            }
            else
            {
                equipmentSlot[i].GetComponent<Image>().sprite = inventorySet[6];
                equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = inventorySet[6];
            }
        }
    }
}
