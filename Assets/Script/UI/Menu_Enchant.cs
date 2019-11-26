using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Enchant : Menu_EquipmentUpgrade
{
    Sprite[] slotImage;
    public GameObject selectEnchantItem;
    public GameObject[] acceptSlot;
    public bool enchantOn;
    public bool enchantting;

    public Button upgradeButton;

    int selectedEquip;
    int enchantFocused;
    int keySlotFocus;

    public override void Awake()
    {
        base.Awake();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_enchant_set");
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!menu.InventoryOn && !menu.CancelOn)
        {
            if (!enchantOn)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { equipFocused = FocusedSlot1(equipSlots, - 1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { equipFocused = FocusedSlot1(equipSlots, - 1, equipFocused); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipFocused == 7)
                    {
                        equipSlots[equipFocused].transform.GetChild(1).gameObject.SetActive(false);
                        if (enchantting)
                        {
                            enchantOn = true;
                            enchantting = false;
                            selectEnchantItem.SetActive(true);
                        }else
                            npc_blacksmith.GetComponent<NPC_Blacksmith>().CloseEnchantMenu();
                    }
                    else
                    {
                        if (!enchantOn)
                        {
                            enchantOn = true;

                            selectEnchantItem.SetActive(true);

                            upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r, 
                                upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);
                            upgradeButton.interactable = true;
                            
                            selectedEquip = enchantFocused;
                            acceptSlot[0].transform.GetChild(1).gameObject.SetActive(true);

                            upgradeEquipment = equipment[equipFocused];
                            
                            if (equipment[equipFocused].key != null)
                            {
                                acceptSlot[0].GetComponent<Image>().sprite = equipment[equipFocused].key.sprite;
                                acceptSlot[0].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[equipment[equipFocused].key.keyRarity];
                            }
                            else
                            {
                                acceptSlot[0].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                                acceptSlot[0].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                            }
                            acceptSlot[1].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                            acceptSlot[1].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) { enchantFocused = FocusedSlot2(acceptSlot, 1, enchantFocused); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { enchantFocused = FocusedSlot2(acceptSlot, -1, enchantFocused); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { enchantFocused = FocusedSlot2(acceptSlot, 1, enchantFocused); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { enchantFocused = FocusedSlot2(acceptSlot , -1, enchantFocused); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (enchantFocused == 3)
                    {
                        enchantOn = false;
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
                                menu.GetComponent<Menu_InGame>().OpenEnchantInventory();
                                break;
                            case 2:
                                Enchant(equipFocused, selectedkey);
                                break;
                        }
                    }
                }
            }
        }
    }

    // 인챈트 창 열었을 때
    public void OpenEnchantMenu()
    {
        upgradeSet = true;
        equipFocused = 0;
        enchantFocused = 0;
        equipSlots[equipFocused].transform.GetChild(1).gameObject.SetActive(true);
        int count = equipSlots.Length;

        equipment = playerEquipment.equipment;
        for(int i = 0; i < count - 1; ++i)
        {
            if (equipment[i].key != null)
            {
                equipSlots[i].GetComponent<Image>().sprite = equipment[i].key.sprite;       // 키 아이템
                equipSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[equipment[i].key.keyRarity]; // 레어도
            }
            else
            {
                equipSlots[i].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];      // 키 아이템
                equipSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];   // 레어도
            }
        }
        acceptSlot[0].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[1].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetEnchantKey(Key key, int _focus)
    {
        selectedkey = key;
        keySlotFocus = _focus;

        acceptSlot[0].GetComponent<Image>().sprite = slotImage[equipment[equipFocused].key.keyRarity];
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = selectedkey.sprite;
    }

    public void Enchant(int num, Key key)
    {
        if (num < 0 || num > 7) return;

        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStat = new float[] { 0, 0, 0, 0, 0, 0, 0 };
            downStat = new bool[7];
            BoolInit(downStat);

            playerEquipment.Init(num);
            upGradeCount = Random.Range(0, 7);

            switch (key.keyRarity)
            {
                case 1:
                    upGradePercent = Random.Range(5, 11);
                    PercentSet(num, upGradeCount, upGradePercent, 0.8f, key, true);
                    break;
                case 2:
                    upGradePercent = Random.Range(40, 61);
                    PercentSet(num, upGradeCount, upGradePercent, 1f, key, true);
                    break;
                case 3:
                    do
                    {
                        downGradeCount = Random.Range(0, 7);
                    }
                    while (upGradeCount == downGradeCount);
                    downStat[downGradeCount] = true;

                    upGradePercent = Random.Range(100, 121);
                    downGradePercent = Random.Range(40, 51);
                    PercentSet(num, upGradeCount, upGradePercent, downGradeCount, downGradePercent, 1.5f, -1f, key, true);
                    break;
            }
        }
        acceptSlot[0].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[0].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];

        inventory.EnchantedKey(keySlotFocus);

        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 120);
        upgradeButton.interactable = false;
        playerData.renew();
    }
}
