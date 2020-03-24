using UnityEngine;
using UnityEngine.UI;

public class TownUI_Upgrade : TownUI_EquipmentUpgrade
{
    public GameObject selectUpgradeItem;

    public void Update()
    {
        if (canvasManager.GameMenuOnCheck()) return;
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
                    /*
                    if (open_ReSelectEquipment)
                    {
                        open_ReSelectEquipment = false;
                        open_SelectItemUI = true;
                        selectItemUIFocused = 0;
                        selectUpgradeItem.SetActive(true);
                    }
                    else
                    {
                        equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                        selectEquipFocused = 0;
                        CloseTownUIMenu();
                    }
                    */
                    equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                    selectEquipFocused = 0;
                    CloseTownUIMenu();
                }
                else
                {
                    if (!equipment[selectEquipFocused].enchant) return;

                    open_SelectItemUI = true;
                    selectUpgradeItem.SetActive(true);
                    OpenSelectedItemMenu();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                /*
                if (open_ReSelectEquipment)    // 장비 재선택 취소
                {
                    open_ReSelectEquipment = false;
                    open_SelectItemUI = true;
                    selectUpgradeItem.SetActive(true);
                }
                else                 // 마법 부여 취소
                {
                    equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                    selectEquipFocused = 0;
                    CloseTownUIMenu();
                }
                */
                equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(false);
                selectEquipFocused = 0;
                CloseTownUIMenu();
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
                    selectUpgradeItem.SetActive(false);
                }
                else
                {
                    switch (selectItemUIFocused)
                    {
                        case 0:
                            open_SelectItemUI = false;
                            open_ReSelectEquipment = true;
                            selectUpgradeItem.SetActive(false);
                            break;
                        case 1:
                            canvasManager.OpenUpgradeStorage(3);
                            break;
                        case 3:
                            Upgrade(selectEquipFocused, selectedkey);
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                open_SelectItemUI = false;
                acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(false);
                selectItemUIFocused = 0;
                selectUpgradeItem.SetActive(false);
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

        if (itemDatabase.GetItem(item.itemCode) != null)
        {
            int upgradePercent;
            int downgradePercent;

            switch (item.itemRarity)
            {
                case 1:
                    upgradePercent = Random.Range(5, 10);
                    PercentSet(num, upgradePercent, item);
                    break;
                case 2:
                    upgradePercent = Random.Range(10, 20);
                    PercentSet(num,  upgradePercent, item);
                    break;
                case 3:
                    upgradePercent = Random.Range(20, 40);
                    downgradePercent = Random.Range(5, 20);
                    PercentSet(num,  upgradePercent, downgradePercent, item);

                    break;
            }
            storage.EnchantedKey(keySlotFocus);
            selectedkey = null;
        }
        playerStat.PlayerStatusUpdate(playerEquipment);

        // accept 창 초기화
        acceptSlot[0].SetActive(false);
        acceptSlot[1].SetActive(false);

        acceptSlot[2].SetActive(true);
        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = SpriteSet.itemSprite[selectEquipFocused];
        SetSlot(acceptSlot[2], num, 0);

        SelectEquipmentSet();

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);

        acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(false);
        selectItemUIFocused = 4;
        acceptSlot[selectItemUIFocused].transform.GetChild(0).gameObject.SetActive(true);
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
