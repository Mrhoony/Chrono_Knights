using UnityEngine;
using UnityEngine.UI;

public class Menu_Enchant : Menu_EquipmentUpgrade
{
    public GameObject selectEnchantItem;
    public GameObject[] acceptSlot;
    public Button enchantButton;
    Sprite[] slotImage;

    public bool enchantOn;
    public bool enchantting;

    public int enchantFocused;

    public override void OnEnable()
    {
        base.OnEnable();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_enchant_set");
        cursorImage = Resources.LoadAll<Sprite>("UI/ui_upgrade_slotncursor");
    }

    public void Update()
    {
        if (menu.isStorageOn) return;

        if (!enchantOn)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { equipFocused = FocusSlotEquipmentSelect(equipSlots, 1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { equipFocused = FocusSlotEquipmentSelect(equipSlots, -1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { equipFocused = FocusSlotEquipmentSelect(equipSlots, 1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { equipFocused = FocusSlotEquipmentSelect(equipSlots, -1, equipFocused); }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (equipFocused == 7)
                {
                    if (enchantting)    // 장비 재선택 취소
                    {
                        enchantting = false;
                        enchantOn = true;
                        enchantFocused = 0;
                        selectEnchantItem.SetActive(true);
                    }
                    else                 // 마법 부여 취소
                    {
                        equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(false);
                        equipFocused = 0;
                        npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseEnchantMenu();
                    }
                }
                else
                {
                    enchantting = false;
                    enchantOn = true;

                    selectEnchantItem.SetActive(true);
                    OpenSelectedEnchantMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(false);
                equipFocused = 0;
                npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseEnchantMenu();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { enchantFocused = FocusSlotItemSelect(acceptSlot, 1, enchantFocused); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { enchantFocused = FocusSlotItemSelect(acceptSlot, -1, enchantFocused); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { enchantFocused = FocusSlotItemSelect(acceptSlot, 1, enchantFocused); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { enchantFocused = FocusSlotItemSelect(acceptSlot, -1, enchantFocused); }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (enchantFocused == 4)
                {
                    enchantOn = false;
                    acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(false);
                    acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
                    enchantFocused = 0;
                    selectEnchantItem.SetActive(false);
                }
                else
                {
                    switch (enchantFocused)
                    {
                        case 0:
                            enchantting = true;
                            enchantOn = false;
                            selectEnchantItem.SetActive(false);
                            break;
                        case 1:
                            menu.OpenUpgradeStorage(2);
                            break;
                        case 3:
                            acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(false);
                            enchantFocused = 0;
                            Enchant(equipFocused, selectedkey);
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                enchantOn = false;
                acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(false);
                acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
                enchantFocused = 0;
                OpenEnchantMenu();
                selectEnchantItem.SetActive(false);
            }
        }
    }
    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = storage.GetStorageItem(focus);

        acceptSlot[1].GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[selectedkey.itemRarity];

        acceptSlot[2].GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[selectedkey.itemRarity];
    }

    // 인챈트 창 열었을 때
    public void OpenEnchantMenu()
    {
        equipFocused = 0;
        enchantFocused = 0;
        equipment = playerEquipment.equipment;

        for(int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                equipSlots[i].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[i].itemCode).sprite;       // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[i].itemCode).itemRarity]; // 레어도
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetStatusName(i, true);
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetUpStatus(i);
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetStatusName(i, false);
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = playerEquipment.GetDownStatus(i);
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = keyItemBorderSprite[6];      // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];   // 레어도
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = "";
            }
            equipSlots[i].transform.GetChild(1).gameObject.SetActive(true);
        }

        equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OpenSelectedEnchantMenu()
    {
        acceptSlot[0].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);

        enchantButton.GetComponent<Image>().color = new Color(enchantButton.GetComponent<Image>().color.r,
            enchantButton.GetComponent<Image>().color.g, enchantButton.GetComponent<Image>().color.b, 255);
        enchantButton.interactable = true;
        
        if (equipment[equipFocused].itemCode != 0)
        {
            acceptSlot[0].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[equipFocused].itemCode).sprite;
            acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[equipFocused].itemCode).itemRarity];
            acceptSlot[0].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetStatusName(equipFocused, true);
            acceptSlot[0].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetUpStatus(equipFocused);
            acceptSlot[0].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetStatusName(equipFocused, false);
            acceptSlot[0].transform.GetChild(5).GetComponent<Text>().text = playerEquipment.GetDownStatus(equipFocused);
        }
        else
        {
            acceptSlot[0].GetComponent<Image>().sprite = keyItemBorderSprite[6];
            acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
            acceptSlot[0].transform.GetChild(2).GetComponent<Text>().text = "";
            acceptSlot[0].transform.GetChild(3).GetComponent<Text>().text = "";
            acceptSlot[0].transform.GetChild(4).GetComponent<Text>().text = "";
            acceptSlot[0].transform.GetChild(5).GetComponent<Text>().text = "";
        }

        acceptSlot[1].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[2].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[2].transform.GetChild(1).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(2).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(3).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(4).GetComponent<Text>().text = "";
    }

    public void Enchant(int num, Item item)
    {
        if (num < 0 || num > 7) return;

        if (itemDatabase.GetItem(item.itemCode) != null)
        {
            playerEquipment.Init(num);
            upgradeCount = Random.Range(0, 6);

            switch (item.itemRarity)
            {
                case 1:
                    upgradePercent = Random.Range(0.05f, 0.1f);
                    PercentSet(num, upgradeCount, upgradePercent, item, true);
                    break;
                case 2:
                    upgradePercent = Random.Range(0.2f, 0.4f);
                    PercentSet(num, upgradeCount, upgradePercent, item, true);
                    break;
                case 3:
                    do
                    {
                        downgradeCount = Random.Range(0, 7);
                    }
                    while (upgradeCount == downgradeCount);

                    upgradePercent = Random.Range(0.6f, 1f);
                    downgradePercent = Random.Range(0.4f, 0.5f);
                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, item, true);
                    break;
            }
            storage.EnchantedKey(keySlotFocus);
        }

        // accept 창 초기화
        acceptSlot[0].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[1].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        
        acceptSlot[2].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[num].itemCode).sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[num].itemCode).itemRarity];
        acceptSlot[2].transform.GetChild(1).GetComponent<Text>().text = playerEquipment.GetStatusName(num, true);
        acceptSlot[2].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetUpStatus(num);
        acceptSlot[2].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetStatusName(num, false);
        acceptSlot[2].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetDownStatus(num);

        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                equipSlots[i].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[i].itemCode).sprite;       // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[i].itemCode).itemRarity]; // 레어도
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetStatusName(i, true);
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetUpStatus(i);
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetStatusName(i, false);
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = playerEquipment.GetDownStatus(i);
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = keyItemBorderSprite[6];      // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];   // 레어도
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = "";
            }
        }

        enchantButton.GetComponent<Image>().color = new Color(enchantButton.GetComponent<Image>().color.r,
            enchantButton.GetComponent<Image>().color.g, enchantButton.GetComponent<Image>().color.b, 120);
        enchantButton.interactable = false;
        
        acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(false);
        enchantFocused = 4;
        acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(true);

        playerData.renew(playerEquipment);
        playerStat.PlayerStatusUpdate();
    }
}
