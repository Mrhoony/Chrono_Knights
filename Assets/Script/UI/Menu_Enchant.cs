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

    int enchantFocused;

    public override void Awake()
    {
        base.Awake();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_enchant_set");
    }

    public void Update()
    {
        if (!menu.GetComponent<Menu_InGame>().InventoryOn && !menu.GetComponent<Menu_InGame>().CancelOn)
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
                        equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(false);
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
                            acceptSlot[enchantFocused].transform.GetChild(0).gameObject.SetActive(false);
                            upgradeEquipment = equipment[equipFocused];
                            enchantOn = true;
                            selectEnchantItem.SetActive(true);
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
                                menu.GetComponent<Menu_InGame>().OpenInventory();
                                break;
                            case 2:
                                break;
                        }
                    }
                }
            }
        }
    }

    public void OpenEnchantMenu()
    {
        upgradeSet = true;
        equipFocused = 0;
        enchantFocused = 0;
        equipSlots[equipFocused].transform.GetChild(0).gameObject.SetActive(true);
        int count = equipSlots.Length;

        equipment = playerEquipment.equipment;
        for(int i = 0; i < count - 1; ++i)
        {
            if(equipment[i].key != null)
            {
                equipSlots[i].GetComponent<Image>().sprite = slotImage[equipment[i].key.keyRarity]; // 테두리 색
                equipSlots[i].GetComponent<SpriteRenderer>().sprite = equipment[i].key.sprite;      // 키아이템 이미지
            }
        }
    }

    public void Enchant(int num)
    {
        //임시 장비 선택, 키아이템 선택
        num = Random.Range(0, 7);
        Key key = Item_Database.instance.keyItem[1];

        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStat = new float[] { 0, 0, 0, 0, 0, 0, 0 };
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

                    upGradeCount = Random.Range(100, 121);
                    downGradePercent = Random.Range(40, 51);
                    PercentSet(num, upGradeCount, upGradePercent, downGradeCount, downGradePercent, 1.5f, -1f, key, true);

                    break;
            }
        }
        playerData.renew();
        //button.SetActive(false);
    }
}
