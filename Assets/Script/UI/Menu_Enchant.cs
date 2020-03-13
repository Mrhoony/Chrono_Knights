using UnityEngine;
using UnityEngine.UI;

public class Menu_Enchant : Menu_EquipmentUpgrade
{
    public GameObject selectEnchantItem;
    Sprite[] slotImage;

    public override void OnEnable()
    {
        base.OnEnable();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_enchant_set");
    }

    public void Update()
    {
        if (menu.GameMenuOnCheck()) return;
        if (!open_BlackSmithUI) return;

        if (!open_SelectItemUI)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { selectEquipFocused = FocusSlotEquipmentSelect(equipSlots, 1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { selectEquipFocused = FocusSlotEquipmentSelect(equipSlots, -1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { selectEquipFocused = FocusSlotEquipmentSelect(equipSlots, 1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { selectEquipFocused = FocusSlotEquipmentSelect(equipSlots, -1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (selectEquipFocused == 7)
                {
                    if (open_ReSelectEquipment)    // 장비 재선택 취소
                    {
                        open_ReSelectEquipment = false;
                        open_SelectItemUI = true;
                        selectItemUIFocused = 0;
                        selectEnchantItem.SetActive(true);
                    }
                    else                 // 마법 부여 취소
                    {
                        equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                        selectEquipFocused = 0;
                        npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseEnchantMenu();
                    }
                }
                else
                {
                    open_SelectItemUI = true;
                    selectEnchantItem.SetActive(true);
                    OpenSelectedItemMenu();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (open_ReSelectEquipment)    // 장비 재선택 취소
                {
                    open_ReSelectEquipment = false;
                    open_SelectItemUI = true;
                    selectEnchantItem.SetActive(true);
                }
                else                 // 마법 부여 취소
                {
                    equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                    selectEquipFocused = 0;
                    npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseEnchantMenu();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { selectItemUIFocused = FocusSlotItemSelect(acceptSlot, 1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { selectItemUIFocused = FocusSlotItemSelect(acceptSlot, -1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { selectItemUIFocused = FocusSlotItemSelect(acceptSlot, 1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { selectItemUIFocused = FocusSlotItemSelect(acceptSlot, -1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (selectItemUIFocused == 4)
                {
                    open_SelectItemUI = false;
                    acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(false);
                    selectItemUIFocused = 0;
                    selectEnchantItem.SetActive(false);
                }
                else
                {
                    switch (selectItemUIFocused)
                    {
                        case 0:
                            open_SelectItemUI = false;
                            open_ReSelectEquipment = true;
                            selectEnchantItem.SetActive(false);
                            break;
                        case 1:
                            menu.OpenUpgradeStorage(2);
                            break;
                        case 3:
                            Enchant(selectEquipFocused, selectedkey);
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                open_SelectItemUI = false;
                acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(false);
                selectItemUIFocused = 0;
                selectEnchantItem.SetActive(false);
            }
        }
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
                        downgradeCount = Random.Range(0, 6);
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
        acceptSlot[0].transform.GetChild(1).gameObject.SetActive(false);
        ClearSlot(acceptSlot[0] , 2);
        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(false);
        acceptSlot[1].transform.GetChild(2).gameObject.SetActive(false);

        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = equipmentSet[num];
        SetSlot(acceptSlot[2], num, 1);

        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                SetSlot(equipSlots[i], i, 2);
            }
            else
            {
                ClearSlot(equipSlots[i], 2);
            }
        }

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);
        
        acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(false);
        selectItemUIFocused = 4;
        acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(true);
        
        playerStat.PlayerStatusUpdate(playerEquipment);
    }
}
