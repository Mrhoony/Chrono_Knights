using UnityEngine;
using UnityEngine.UI;

public class TownUI_Enchant : TownUI_EquipmentUpgrade
{
    Skill[] selectedSkillList;
    public GameObject skillListCursor;
    public GameObject skillListObject;
    public Text[] skillList;
    public bool enchantSkillSelect;
    public int skillSelectFocused;
    public int selectedEquipmentNumber;

    public void Update()
    {
        if (canvasManager.GameMenuOnCheck() || canvasManager.DialogBoxOn()) return;
        if (!open_BlackSmithUI) return;

        if (!open_SelectItemUI)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if (selectEquipFocused == 7)
                {
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
                CloseTownUIMenu();
            }
            if (Input.GetKeyDown(KeyCode.Escape))              // esc 를 눌렀을 때
            {
                CloseTownUIMenu();
            }
            if (selectEquipFocused != 7)
            {
                FocusEquipSlotMove(equipSlots[selectEquipFocused]);
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { selectEquipFocused = FocusSlotEquipmentSelect(1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { selectEquipFocused = FocusSlotEquipmentSelect(-1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { selectEquipFocused = FocusSlotEquipmentSelect(1, selectEquipFocused); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { selectEquipFocused = FocusSlotEquipmentSelect(-1, selectEquipFocused); }
        }
        else
        {
            if (enchantSkillSelect)
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    SkillSelect();
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { skillSelectFocused = FocusedSlot(1, skillSelectFocused, 2); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { skillSelectFocused = FocusedSlot(-1, skillSelectFocused, 2); }
                FocusSkillMove();
            }
            else
            {
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
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { selectItemUIFocused = FocusSlotItemSelect(1, selectItemUIFocused); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { selectItemUIFocused = FocusSlotItemSelect(-1, selectItemUIFocused); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { selectItemUIFocused = FocusSlotItemSelect(1, selectItemUIFocused); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { selectItemUIFocused = FocusSlotItemSelect(-1, selectItemUIFocused); }
            }
        }
    }
    public void CloseTownUIMenu()
    {
        cursorEquipSelect.SetActive(false);
        selectEquipFocused = 0;
        townUI.CloseEnchantMenu();
    }

    public void Enchant(int _Num, Item item)
    {
        if (_Num < 0 || _Num > 7) return;
        selectedEquipmentNumber = _Num;

        if (itemDatabase.GetItem(item.itemCode) == null) return;

        playerEquipment.PlayerEquipmentInit(selectedEquipmentNumber);

        int upgradeCount;
        int upgradePercent;

        upgradeCount = Random.Range(0, 6);

        switch (item.itemRarity)
        {
            case 1:
                upgradePercent = Random.Range(5, 20);
                PercentSet(upgradeCount, upgradePercent, item);
                EnchantEnd(selectedEquipmentNumber);
                break;
            case 2:
                upgradePercent = Random.Range(20, 40);
                PercentSet(upgradeCount, upgradePercent, item);
                EnchantEnd(selectedEquipmentNumber);
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

                PercentSet(upgradeCount, upgradePercent, downgradeCount, downgradePercent, item);
                SkillSetting();
                break;
        }
    }

    int FocusedSlot(int _Focus, int AdjustValue, int _MaxFocused)
    {
        if (_Focus + AdjustValue < 0) _Focus = _MaxFocused;
        else if (_Focus + AdjustValue > _MaxFocused) _Focus = 0;
        else _Focus += AdjustValue;

        return _Focus;
    }

    public void EnchantEnd(int num)
    {
        storage.DeleteItem(keySlotFocus);
        selectedkey = null;
        playerStat.PlayerStatusUpdate();

        canvasManager.Menus[0].GetComponent<Menu_Inventory>().SetAvailableSlot(playerEquipment.equipment[num].itemRarity);
        canvasManager.SetPlayerStatusInfo(playerStat.playerData.GetPlayerEquipment().equipment);

        // accept 창 초기화
        acceptSlot[0].SetActive(false);
        acceptSlot[1].SetActive(false);

        acceptSlot[2].SetActive(true);
        acceptSlot[3].SetActive(true);
        SetSlot(acceptSlot[3], num, 0);

        SelectEquipmentSet();

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);

        selectItemUIFocused = 3;
        upgradeButton.SetActive(false);
        itemCancel.SetActive(true);
    }

    public void PercentSet(int upCount, float upPercent, Item item)
    {
        equipment[selectedEquipmentNumber].EquipmentItemSetting(item);
        equipment[selectedEquipmentNumber].EquipmentStatusEnchant(upCount, upPercent, true);

        if (equipment[selectedEquipmentNumber].downStatus != 8)
        {
            equipment[selectedEquipmentNumber].addStatus[equipment[selectedEquipmentNumber].downStatus] = 0;
            equipment[selectedEquipmentNumber].downStatus = 8;
        }
    }
    public void PercentSet(int upCount, float upPercent, int downCount, float downPercent, Item item)
    {
        equipment[selectedEquipmentNumber].EquipmentItemSetting(item);
        equipment[selectedEquipmentNumber].EquipmentStatusEnchant(upCount, upPercent, true);
        equipment[selectedEquipmentNumber].EquipmentStatusEnchant(downCount, downPercent, false);
    }

    public void SkillSetting()
    {
        selectedSkillList = new Skill[3];
        enchantSkillSelect = true;
        skillSelectFocused = 0;
        for (int i = 0; i < 3; ++i)
        {
            selectedSkillList[i] = Database_Game.instance.SkillSetting(equipment[selectedEquipmentNumber].equipmentType);
            skillList[i].text = selectedSkillList[i].skillName + " : " + selectedSkillList[i].skillDescription;
        }
        skillListObject.SetActive(true);
        skillListCursor.SetActive(true);
    }

    public void SkillSelect()
    {
        playerEquipment.EquipmentSkillSetting(selectedEquipmentNumber, selectedSkillList[skillSelectFocused].skillCode);
        skillListCursor.SetActive(false);
        skillListObject.SetActive(false);
        enchantSkillSelect = false;
        EnchantEnd(selectedEquipmentNumber);
    }

    public void FocusSkillMove()
    {
        skillListCursor.transform.position
            = Vector2.Lerp(skillListCursor.transform.position, new Vector2(skillListCursor.transform.position.x, skillList[skillSelectFocused].transform.position.y), Time.deltaTime * cursorSpd);
    }
}
