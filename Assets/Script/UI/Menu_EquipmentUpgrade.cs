using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_EquipmentUpgrade : MonoBehaviour
{
    protected PlayerStatus playerStat;
    protected PlayerData playerData;
    protected CanvasManager menu;
    protected Menu_Storage storage;
    protected Database_Game itemDatabase;

    public GameObject npc_blacksmith;
    protected PlayerEquipment playerEquipment;
    protected PlayerEquipment.Equipment[] equipment;
    protected Item selectedkey;

    public GameObject[] equipSlots;
    protected Sprite[] cursorImage;
    protected Sprite[] keyItemBorderSprite;
    protected int equipFocused;

    protected int upgradeCount;
    protected int upgradePercent;
    protected int downgradeCount;
    protected int downgradePercent;

    protected int keySlotFocus;

    // Start is called before the first frame update
    public virtual void OnEnable()
    {
        menu = transform.parent.GetComponent<Menu_TownUI>().menu;
        itemDatabase = Database_Game.instance;
        storage = menu.storage.GetComponent<Menu_Storage>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStat.playerData;
        playerEquipment = playerStat.playerEquip;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
    }

    public int FocusedSlot1(GameObject[] slots, int AdjustValue, int focused)
    {
        slots[focused].transform.GetChild(0).gameObject.SetActive(false);
        
        focused += AdjustValue;

        if (focused < 0)
            focused = 7;
        if (focused > 7)
            focused = 0;
        
        slots[focused].transform.GetChild(0).gameObject.SetActive(true);

        return focused;
    }

    public int FocusedSlot2(GameObject[] slots, int AdjustValue, int focused)
    {
        slots[focused].transform.GetChild(0).gameObject.SetActive(false);

        focused += AdjustValue;
        if(focused == 2)
        {
            focused += AdjustValue;
        }
        
        if (focused < 0)
            focused = 4;
        if (focused > 4)
            focused = 0;

        slots[focused].transform.GetChild(0).gameObject.SetActive(true);

        return focused;
    }

    public void PercentSet(int num, int upCount, float upPercent, Item item, bool enchant)
    {
        if (enchant)
        {
            equipment[num].EquipmentItemSetting(item);
            equipment[num].upStatus = upCount;
            equipment[num].addStatus[upCount] = upPercent * 0.1f;

            Debug.Log(equipment[num].upStatus);
            Debug.Log(equipment[num].addStatus[upCount]);

            if (equipment[num].addStatus[upCount] > equipment[num].max)
                equipment[num].addStatus[upCount] = equipment[num].max;
            
            if (equipment[num].downStatus != 8)
            {
                equipment[num].addStatus[equipment[num].downStatus] = 0;
                equipment[num].downStatus = 8;
            }

            equipment[num].skillCode = item.skillCode;
        }
        else
        {
            equipment[num].name += item.itemName;
            equipment[num].addStatus[upCount] += upPercent * 0.1f;
            if (equipment[num].addStatus[upCount] > equipment[num].max)
                equipment[num].addStatus[upCount] = equipment[num].max;
        }
    }

    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, Item item, bool enchant)
    {
        if (enchant)
        {
            equipment[num].EquipmentItemSetting(item);
            equipment[num].upStatus = upCount;
            equipment[num].downStatus = downCount;
            equipment[num].addStatus[upCount] = upPercent * 0.1f;
            if (equipment[num].addStatus[upCount] > equipment[num].max)
                equipment[num].addStatus[upCount] = equipment[num].max;

            equipment[num].addStatus[downCount] = -downPercent * 0.1f;
            if (equipment[num].addStatus[downCount] > equipment[num].max)
                equipment[num].addStatus[downCount] = equipment[num].max;


            equipment[num].skillCode = item.skillCode;
        }
        else
        {
            equipment[num].name += item.itemName;
            equipment[num].addStatus[upCount] += upPercent * 0.1f;
            if (equipment[num].addStatus[upCount] > equipment[num].max)
                equipment[num].addStatus[upCount] = equipment[num].max;

            if(equipment[num].downStatus != 8)
            {
                equipment[num].addStatus[downCount] -= downPercent * 0.1f;
                if (equipment[num].addStatus[downCount] < equipment[num].min)
                    equipment[num].addStatus[downCount] = equipment[num].min;
            }
        }
    }
    
    public void BoolInit(bool[] b)
    {
        int length = b.Length;
        for (int i = 0; i < length; i++)
            b[i] = false;
    }
}
