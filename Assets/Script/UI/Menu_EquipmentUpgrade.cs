using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_EquipmentUpgrade : MonoBehaviour
{
    protected PlayerStatus playerStat;
    protected PlayerData playerData;
    protected MainUI_Menu menu;
    protected Menu_Inventory inventory;

    public GameObject npc_blacksmith;
    protected PlayerEquipment playerEquipment;
    protected PlayerEquipment.Equipment[] equipment;

    public PlayerEquipment.Equipment upgradeEquipment;
    public Key selectedkey;
    public int[] limitUpgrade;

    public GameObject[] equipSlots;
    public Sprite[] cursorImage;
    public int equipFocused;

    public int upgradeCount;
    public int upgradePercent;
    public int downgradeCount;
    public int downgradePercent;

    protected int keySlotFocus;

    // Start is called before the first frame update
    public virtual void Start()
    {
        menu = transform.parent.GetComponent<Menu_TownUI>().menu;
        inventory = menu.Menus[0].GetComponent<Menu_Inventory>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStat.playerData;
        playerEquipment = playerStat.playerEquip;
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

    public void PercentSet(int num, int upCount, float upPercent, Key key, bool enchant)
    {
        if (enchant)
        {
            equipment[num].key = key;
            equipment[num].enchant = enchant;
            equipment[num].name = key.keyName;
            equipment[num].upStatus = upCount;
            equipment[num].addStatus[equipment[num].upStatus] = upPercent * 0.01f;
            if (equipment[num].addStatus[equipment[num].upStatus] > equipment[num].max)
                equipment[num].addStatus[equipment[num].upStatus] = equipment[num].max;
        }
        else
        {
            equipment[num].name += key.keyName;
            equipment[num].addStatus[equipment[num].upStatus] += upPercent * 0.01f;
            if (equipment[num].addStatus[equipment[num].upStatus] > equipment[num].max)
                equipment[num].addStatus[equipment[num].upStatus] = equipment[num].max;
        }
    }

    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, Key key, bool enchant)
    {
        if (enchant)
        {
            equipment[num].key = key;
            equipment[num].enchant = enchant;
            equipment[num].name = key.keyName;
            equipment[num].upStatus = upCount;
            equipment[num].downStatus = downCount;
            equipment[num].addStatus[equipment[num].upStatus] = upPercent * 0.01f;

            if (equipment[num].addStatus[equipment[num].upStatus] > equipment[num].max)
                equipment[num].addStatus[equipment[num].upStatus] = equipment[num].max;
            equipment[num].addStatus[equipment[num].downStatus] = -downPercent * 0.01f;

            if (equipment[num].addStatus[equipment[num].downStatus] > equipment[num].max)
                equipment[num].addStatus[equipment[num].downStatus] = equipment[num].max;
        }
        else
        {
            equipment[num].name += key.keyName;
            equipment[num].addStatus[equipment[num].upStatus] += upPercent * 0.01f;

            if (equipment[num].addStatus[equipment[num].upStatus] > equipment[num].max)
                equipment[num].addStatus[equipment[num].upStatus] = equipment[num].max;

            equipment[num].addStatus[equipment[num].downStatus] -= downPercent * 0.01f;
            if (equipment[num].addStatus[equipment[num].downStatus] < equipment[num].min)
                equipment[num].addStatus[equipment[num].downStatus] = equipment[num].min;

        }
    }
    
    public void BoolInit(bool[] b)
    {
        int length = b.Length;
        for (int i = 0; i < length; i++)
            b[i] = false;
    }
}
