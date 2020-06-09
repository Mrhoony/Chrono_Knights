using UnityEngine;
using UnityEngine.UI;

public class TownUI_Enchant : TownUI_EquipmentUpgrade
{
    public void Update()
    {
        if (canvasManager.GameMenuOnCheck() || canvasManager.DialogBoxOn()) return;
        if (!open_BlackSmithUI) return;

        if (!open_SelectItemUI)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { selectEquipFocused = FocusSlotEquipmentSelect(1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { selectEquipFocused = FocusSlotEquipmentSelect(-1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { selectEquipFocused = FocusSlotEquipmentSelect(1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { selectEquipFocused = FocusSlotEquipmentSelect(-1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if (selectEquipFocused == 7)
                {
                    cursorEquipSelect.SetActive(false);
                    selectEquipFocused = 0;
                    CloseTownUIMenu();
                }
                else
                {
                    open_SelectItemUI = true;
                    selectUseItem.SetActive(true);
                    OpenSelectedItemMenu();
                }
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
            {
                cursorEquipSelect.SetActive(false);
                selectEquipFocused = 0;
                CloseTownUIMenu();
            }
            if (Input.GetKeyDown(KeyCode.Escape))              // esc 를 눌렀을 때
            {
                cursorEquipSelect.SetActive(false);
                selectEquipFocused = 0;
                CloseTownUIMenu();
            }
            if (selectEquipFocused != 7)
            {
                FocusEquipSlotMove(equipSlots[selectEquipFocused]);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { selectItemUIFocused = FocusSlotItemSelect(1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { selectItemUIFocused = FocusSlotItemSelect(-1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { selectItemUIFocused = FocusSlotItemSelect(1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { selectItemUIFocused = FocusSlotItemSelect(-1, selectItemUIFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if (selectItemUIFocused == 3)
                {
                    CloseSelectedItemMenu();
                    selectUseItem.SetActive(false);
                }
                else
                {
                    switch (selectItemUIFocused)
                    {
                        case 0:
                            CloseSelectedItemMenu();
                            selectUseItem.SetActive(false);
                            break;
                        case 1:
                            canvasManager.OpenUpgradeStorage(2);
                            break;
                        case 2:
                            cursorItemSelect.SetActive(false);
                            if (selectedkey != null)
                                Enchant(selectEquipFocused, selectedkey);
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
            {
                CloseSelectedItemMenu();
                selectUseItem.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))              // esc 를 눌렀을 때
            {
                CloseSelectedItemMenu();
                selectUseItem.SetActive(false);
            }
            if (selectEquipFocused >= 0 || selectEquipFocused < 2)
            {
                FocusItemSlotMove(acceptSlot[selectItemUIFocused]);
            }
        }
    }
    public void CloseTownUIMenu()
    {
        townUI.CloseEnchantMenu();
    }

    public void Enchant(int num, Item item)
    {
        if (num < 0 || num > 7) return;

        if (itemDatabase.GetItem(item.itemCode) != null)
        {
            playerEquipment.PlayerEquipmentInit(num);

            int upgradeCount;
            int upgradePercent;

            upgradeCount = Random.Range(0, 6);

            switch (item.itemRarity)
            {
                case 1:
                    upgradePercent = Random.Range(5, 20);
                    PercentSet(num, upgradeCount, upgradePercent, item);
                    break;
                case 2:
                    upgradePercent = Random.Range(20, 40);
                    PercentSet(num, upgradeCount, upgradePercent, item);
                    break;
                case 3:
                    int downgradeCount;
                    int downgradePercent;
                    do
                    {
                        downgradeCount = Random.Range(0, 6);
                    }
                    while (upgradeCount == downgradeCount);

                    upgradePercent = Random.Range(40, 60);
                    downgradePercent = Random.Range(60, 80);

                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, item);
                    break;
            }
            storage.DeleteItem(keySlotFocus);
            selectedkey = null;
        }
        playerStat.PlayerStatusUpdate();

        canvasManager.Menus[0].GetComponent<Menu_Inventory>().SetAvailableSlot(playerEquipment.equipment[num].itemRarity);
        canvasManager.SetPlayerStatusInfo(playerStat.playerData.GetPlayerEquipment().equipment);

        // accept 창 초기화
        acceptSlot[0].SetActive(false);
        acceptSlot[1].SetActive(false);
        acceptSlot[2].SetActive(false);

        acceptSlot[3].SetActive(true);
        SetSlot(acceptSlot[3], num, 0);

        SelectEquipmentSet();

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);

        selectItemUIFocused = 3;
        upgradeButton.SetActive(false);
        itemCancel.SetActive(true);
    }

    public void PercentSet(int num, int upCount, float upPercent, Item item)
    {
        equipment[num].EquipmentItemSetting(item);
        equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);

        if (equipment[num].downStatus != 8)
        {
            equipment[num].addStatus[equipment[num].downStatus] = 0;
            equipment[num].downStatus = 8;
        }
    }
    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, Item item)
    {
        equipment[num].EquipmentItemSetting(item);
        equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);
        equipment[num].EquipmentStatusEnchant(downCount, downPercent, false);
        equipment[num].EquipmentSkillSetting();
        playerEquipment.EquipmentSkillSetting(num);
    }
}
