using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Enchant : Menu_EquipmentUpgrade
{
    public GameObject selectEnchantItem;
    public GameObject[] acceptSlot;
    public Button enchantButton;
    Sprite[] slotImage;

    bool enchantOn;
    bool enchantting;
    
    int enchantFocused;

    public override void Start()
    {
        base.Start();
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
                        enchantOn = true;

                        selectEnchantItem.SetActive(true);

                        enchantButton.GetComponent<Image>().color = new Color(enchantButton.GetComponent<Image>().color.r,
                            enchantButton.GetComponent<Image>().color.g, enchantButton.GetComponent<Image>().color.b, 255);
                        enchantButton.interactable = true;

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
                        acceptSlot[2].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
                        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
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
                    if (enchantFocused == 4)
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
                                menu.OpenUpgradeInventory(2);
                                break;
                            case 3:
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

        equipment = playerEquipment.equipment;

        for(int i = 0; i < 7; ++i)
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
        equipSlots[equipFocused].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SetKey(int focus)
    {
        selectedkey = inventory.inventoryKeylist[focus];
        acceptSlot[1].GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[1].transform.GetChild(0).GetComponent<Image>().sprite = slotImage[selectedkey.keyRarity];
    }

    public void Enchant(int num, Key key)
    {
        if (num < 0 || num > 7) return;

        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStatus = new float[] { 0, 0, 0, 0, 0, 0, 0 };

            playerEquipment.Init(num);
            upgradeCount = Random.Range(0, 7);

            switch (key.keyRarity)
            {
                case 1:
                    upgradePercent = Random.Range(5, 11);
                    PercentSet(num, upgradeCount, upgradePercent, 0.8f, key, true);
                    break;
                case 2:
                    upgradePercent = Random.Range(40, 61);
                    PercentSet(num, upgradeCount, upgradePercent, 1f, key, true);
                    break;
                case 3:
                    do
                    {
                        downgradeCount = Random.Range(0, 7);
                    }
                    while (upgradeCount == downgradeCount);

                    upgradePercent = Random.Range(100, 121);
                    downgradePercent = Random.Range(40, 51);
                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, 1.5f, -1f, key, true);
                    break;
            }
        }
        acceptSlot[0].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[0].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];
        acceptSlot[1].transform.GetChild(0).GetComponent<Image>().sprite = inventory.keyItemBorderSprite[6];

        acceptSlot[2].GetComponent<Image>().sprite = equipSlots[num].GetComponent<Image>().sprite;
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = equipSlots[num].transform.GetChild(0).GetComponent<Image>().sprite;

        inventory.EnchantedKey(keySlotFocus);

        for (int i = 0; i < 7; ++i)
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

        enchantButton.GetComponent<Image>().color = new Color(enchantButton.GetComponent<Image>().color.r,
            enchantButton.GetComponent<Image>().color.g, enchantButton.GetComponent<Image>().color.b, 120);
        enchantButton.interactable = false;
        playerData.renew();
    }
}
