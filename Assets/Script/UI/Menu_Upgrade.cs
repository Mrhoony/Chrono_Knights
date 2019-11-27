using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Upgrade : Menu_EquipmentUpgrade
{
    public GameObject selectUpgradeItem;
    public GameObject[] acceptSlot;
    public Button upgradeButton;
    Sprite[] slotImage;

    bool upgradeOn;
    bool upgrading;

    int upgradeFocus;

    public override void Start()
    {
        base.Start();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_upgrade_set");
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!menu.InventoryOn && !menu.CancelOn)
        {
            if (!upgradeOn)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { equipFocused = FocusedSlot1(equipSlots, -1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { equipFocused = FocusedSlot1(equipSlots, -1, equipFocused); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipFocused == 7)
                    {
                        equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(false);
                        if (upgrading)
                        {
                            upgradeOn = true;
                            upgrading = false;
                            selectUpgradeItem.SetActive(true);
                        }
                        else
                            npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseUpgradeMenu();
                    }
                    else
                    {
                        upgradeOn = true;

                        selectUpgradeItem.SetActive(true);

                        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
                            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);
                        upgradeButton.interactable = true;

                        acceptSlot[0].transform.GetChild(1).gameObject.SetActive(true);
                        upgradeEquipment = equipment[equipFocused];

                        if (equipment[equipFocused].key != null)
                        {
                            acceptSlot[0].GetComponent<Image>().sprite = equipment[equipFocused].key.sprite;
                            acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[equipment[equipFocused].key.keyRarity];
                        }
                        else
                        {
                            acceptSlot[0].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                            acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        }
                        acceptSlot[1].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        acceptSlot[2].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) { upgradeFocus = FocusedSlot2(acceptSlot, 1, upgradeFocus); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { upgradeFocus = FocusedSlot2(acceptSlot, -1, upgradeFocus); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { upgradeFocus = FocusedSlot2(acceptSlot, 1, upgradeFocus); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { upgradeFocus = FocusedSlot2(acceptSlot, -1, upgradeFocus); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (upgradeFocus == 4)
                    {
                        upgradeOn = false;
                        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
                        selectUpgradeItem.SetActive(false);
                    }
                    else
                    {
                        switch (upgradeFocus)
                        {
                            case 0:
                                upgrading = true;
                                upgradeOn = false;
                                selectUpgradeItem.SetActive(false);
                                break;
                            case 1:
                                menu.OpenUpgradeInventory(3);
                                break;
                            case 3:
                                Upgrade(equipFocused, selectedkey);
                                break;
                        }
                    }
                }
            }
        }
    }
    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = inventory.inventoryKeylist[focus];
        acceptSlot[1].GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[selectedkey.keyRarity];

        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].GetComponent<Image>().sprite = acceptSlot[0].GetComponent<Image>().sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite;
    }

    public void OpenUpgradeMenu()
    {
        upgradeOn = false;
        equipFocused = 0;
        upgradeFocus = 0;

        equipment = playerEquipment.equipment;

        for (int i = 0; i < 7; ++i)
        {
            if(equipment[i].key != null)
            {
                equipSlots[i].GetComponent<Image>().sprite = slotImage[equipment[i].key.keyRarity]; // 테두리 색
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = equipment[i].key.sprite;      // 키아이템 이미지
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6]; // 테두리 색
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];      // 키아이템 이미지
            }
            equipSlots[i].transform.GetChild(1).gameObject.SetActive(true);
        }

        equipSlots[equipFocused].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Upgrade(int num, Key key)
    {
        if (num < 0 || num > 7) return;

        if (Item_Database.instance.KeyInformation(key) != null)
        {
            switch (key.keyRarity)
            {
                case 1:
                    upgradePercent = Random.Range(1, 6);
                    PercentSet(num, upgradeCount, upgradePercent, key, false);
                    break;
                case 2:
                    upgradePercent = Random.Range(3, 11);
                    PercentSet(num, upgradeCount, upgradePercent, key, false);
                    break;
                case 3:
                    upgradePercent = Random.Range(3, 21);
                    downgradePercent = Random.Range(0, 11);
                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, key, false);

                    break;
            }
            inventory.EnchantedKey(keySlotFocus);
        }
        // accept 창 초기화
        acceptSlot[0].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        
        acceptSlot[2].GetComponent<Image>().sprite = equipment[num].key.sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[equipment[num].key.keyRarity];

        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].key != null)
            {
                equipSlots[i].GetComponent<Image>().sprite = equipment[i].key.sprite;       // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[equipment[i].key.keyRarity]; // 레어도
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];      // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];   // 레어도
            }
        }

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);
        upgradeButton.interactable = false;

        acceptSlot[upgradeFocus].transform.GetChild(0).gameObject.SetActive(false);
        upgradeFocus = 4;
        acceptSlot[upgradeFocus].transform.GetChild(0).gameObject.SetActive(true);

        playerData.renew();
    }
}
