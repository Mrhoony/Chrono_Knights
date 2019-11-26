using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Upgrade : Menu_EquipmentUpgrade
{
    Sprite[] slotImage;
    public bool upgradeOn;

    public override void Awake()
    {
        base.Awake();
        slotImage = Resources.LoadAll<Sprite>("UI/ui_upgrade_set");
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!menu.InventoryOn && !menu.CancelOn)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { equipFocused = FocusedSlot1(equipSlots, -1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { equipFocused = FocusedSlot1(equipSlots, 1, equipFocused); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { equipFocused = FocusedSlot1(equipSlots, -1, equipFocused); }
        }
    }

    public void OpenUpgradeMenu()
    {
        upgradeSet = true;
        equipment = playerEquipment.equipment;
        int count = equipSlots.Length;
        for (int i = 0; i < count; ++i)
        {
            equipSlots[i].GetComponent<Image>().sprite = slotImage[equipment[i].key.keyRarity]; // 테두리 색
            equipSlots[i].GetComponent<SpriteRenderer>().sprite = equipment[i].key.sprite;      // 키아이템 이미지
        }
    }

    public void Upgrade(int num)
    {
        //임시 장비 선택, 키아이템 선택
        num = Random.Range(0, 7);
        Key key = Item_Database.instance.keyItem[0];

        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStat = playerEquipment.GetEquipAddStat(num);
            int length = addStat.Length;
            for(int i = 0; i<length; i++)
            {
                if (addStat[i] > 0)
                    upGradeCount = i;
            }

            switch (key.keyRarity)
            {
                case 1:
                    upGradePercent = Random.Range(1, 6);
                    PercentSet(num, upGradeCount, upGradePercent, 0.8f, key, false);
                    break;
                case 2:
                    upGradePercent = Random.Range(3, 11);
                    PercentSet(num, upGradeCount, upGradePercent, 1f, key, false);
                    break;
                case 3:
                    for (int i = 0; i < length; i++)
                    {
                        if (addStat[i] < 0)
                            downGradeCount = i;
                    }
                    upGradeCount = Random.Range(3, 21);
                    downGradePercent = Random.Range(0, 11);
                    PercentSet(num, upGradeCount, upGradePercent, downGradeCount, downGradePercent, 1.5f, -1f, key, false);

                    break;
            }
        }
        playerData.renew();
        //button.SetActive(false);
    }
}
