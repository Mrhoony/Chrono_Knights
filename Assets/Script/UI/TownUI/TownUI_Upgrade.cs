using UnityEngine;
using UnityEngine.UI;

public class TownUI_Upgrade : TownUI_EquipmentUpgrade
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
                    if (!equipment[selectEquipFocused].enchant) return;

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
                            canvasManager.OpenUpgradeStorage(3);
                            break;
                        case 2:
                            cursorItemSelect.SetActive(false);
                            if (selectedkey != null)
                                Upgrade(selectEquipFocused, selectedkey);
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
            if (selectItemUIFocused >= 0 || selectItemUIFocused < 2)
            {
                FocusItemSlotMove(acceptSlot[selectItemUIFocused]);
            }
        }
    }
    public void CloseTownUIMenu()
    {
        townUI.CloseUpgradeMenu();
    }

    public void Upgrade(int num, Item item)
    {
        if (num < 0 || num > 7) return;

        Debug.Log("upgrade");

        int upgradePercent;
        int downgradePercent;

        if (itemDatabase.GetItem(item.itemCode) != null)
        {
            switch (item.itemRarity)
            {
                case 1:
                    upgradePercent = Random.Range(5, 11);
                    downgradePercent = Random.Range(1, 6);
                    break;
                case 2:
                    upgradePercent = Random.Range(10, 21);
                    downgradePercent = Random.Range(5, 11);

                    break;
                default:
                    upgradePercent = Random.Range(20, 41);
                    downgradePercent = Random.Range(5, 21);
                    break;
            }
            if(equipment[num].itemRarity != 3)
                PercentSet(num, upgradePercent, item);
            else
                PercentSet(num, upgradePercent, downgradePercent, item);
             
            storage.DeleteItem(keySlotFocus);
            selectedkey = null;
        }
        playerStat.PlayerStatusUpdate();

        // accept 창 초기화
        acceptSlot[0].SetActive(false);
        acceptSlot[1].SetActive(false);
        acceptSlot[2].SetActive(false);

        acceptSlot[3].SetActive(true);
        SetSlot(acceptSlot[3], num, 0);

        SelectEquipmentSet();

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);

        upgradeButton.SetActive(false);
        selectItemUIFocused = 3;
        itemCancel.SetActive(true);
    }
    public void PercentSet(int num, float upPercent, Item item)
    {
        equipment[num].EquipmentStatusUpgrade(equipment[num].upStatus, upPercent, true);
    }
    public void PercentSet(int num, float upPercent, float downPercent, Item item)
    {
        equipment[num].EquipmentStatusUpgrade(equipment[num].upStatus, upPercent, true);

        if (equipment[num].downStatus != 8)
        {
            equipment[num].EquipmentStatusUpgrade(equipment[num].downStatus, downPercent, false);
        }
    }
}
