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
        cursorImage = Resources.LoadAll<Sprite>("UI/ui_upgrade_slotncursor");
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (menu.isStorageOn) return;
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
                        upgradeFocus = 0;
                        selectUpgradeItem.SetActive(true);
                    }
                    else
                        npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseUpgradeMenu();
                }
                else
                {
                    upgrading = false;
                    upgradeOn = true;

                    selectUpgradeItem.SetActive(true);
                    OpenSelectUpgradeMenu();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(false);
                equipFocused = 0;
                npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseUpgradeMenu();
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
                            menu.OpenUpgradeStorage(3);
                            break;
                        case 3:
                            Upgrade(equipFocused, selectedkey);
                            break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                upgradeOn = false;
                acceptSlot[upgradeFocus].transform.GetChild(0).gameObject.SetActive(false);
                acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
                upgradeFocus = 0;
                OpenUpgradeMenu();
                selectUpgradeItem.SetActive(false);
            }
        }
    }
    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = storage.GetStorageItem(focus);

        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(true);
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
            if(equipment[i].itemCode != 0)
            {
                equipSlots[i].GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[i].itemCode).keyRarity]; // 테두리 색
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[i].itemCode).sprite;      // 키아이템 이미지
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetStatusName(i, true);
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetUpStatus(i);
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetStatusName(i, false);
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = playerEquipment.GetDownStatus(i);
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = keyItemBorderSprite[6]; // 테두리 색
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];      // 키아이템 이미지
                equipSlots[i].transform.GetChild(2).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(3).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(4).GetComponent<Text>().text = "";
                equipSlots[i].transform.GetChild(5).GetComponent<Text>().text = "";
            }
            equipSlots[i].transform.GetChild(1).gameObject.SetActive(true);
        }

        equipSlots[equipFocused].transform.GetChild(1).gameObject.SetActive(true);
    }
    public void OpenSelectUpgradeMenu()
    {
        acceptSlot[0].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[1].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);
        upgradeButton.interactable = true;

        if (equipment[equipFocused].itemCode != 0)
        {
            acceptSlot[0].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[equipFocused].itemCode).sprite;
            acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[equipFocused].itemCode).keyRarity];
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
        acceptSlot[2].transform.GetChild(2).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(3).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(4).GetComponent<Text>().text = "";
        acceptSlot[2].transform.GetChild(5).GetComponent<Text>().text = "";
    }
    public void Upgrade(int num, Key key)
    {
        if (num < 0 || num > 7) return;

        Debug.Log("upgrade");

        if (Item_Database.instance.GetItem(key.keyCode) != null)
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
            storage.EnchantedKey(keySlotFocus);
        }
        // accept 창 초기화
        acceptSlot[0].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[1].GetComponent<Image>().sprite = keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        
        acceptSlot[2].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[num].itemCode).sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[num].itemCode).keyRarity];
        acceptSlot[2].transform.GetChild(1).GetComponent<Text>().text = playerEquipment.GetStatusName(num, true);
        acceptSlot[2].transform.GetChild(2).GetComponent<Text>().text = playerEquipment.GetUpStatus(num);
        acceptSlot[2].transform.GetChild(3).GetComponent<Text>().text = playerEquipment.GetStatusName(num, false);
        acceptSlot[2].transform.GetChild(4).GetComponent<Text>().text = playerEquipment.GetDownStatus(num);

        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                equipSlots[i].GetComponent<Image>().sprite = itemDatabase.GetItem(equipment[i].itemCode).sprite;       // 키 아이템
                equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = slotImage[itemDatabase.GetItem(equipment[i].itemCode).keyRarity]; // 레어도
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

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);
        upgradeButton.interactable = false;

        acceptSlot[upgradeFocus].transform.GetChild(0).gameObject.SetActive(false);
        upgradeFocus = 4;
        acceptSlot[upgradeFocus].transform.GetChild(0).gameObject.SetActive(true);

        playerData.renew(playerEquipment);
        playerStat.PlayerStatusUpdate();
    }
}
