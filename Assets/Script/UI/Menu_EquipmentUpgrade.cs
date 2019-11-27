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
    public bool upgradeSet;

    public PlayerEquipment.Equipment upgradeEquipment;
    public Key selectedkey;
    public int[] limitUpgrade;

    public GameObject[] equipSlots;
    public int equipFocused;

    public float[] addStatus;

    public int upgradeCount;
    public int upgradePercent;
    public int downgradeCount;
    public int downgradePercent;

    protected int keySlotFocus;

    // Start is called before the first frame update
    public virtual void Start()
    {
        upgradeSet = false;
        menu = transform.parent.GetComponent<Menu_TownUI>().menu;
        inventory = menu.Menus[0].GetComponent<Menu_Inventory>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStat.playerData;
        playerEquipment = playerStat.playerEquip;
    }

    public int FocusedSlot1(GameObject[] slots, int AdjustValue, int focused)
    {
        slots[focused].transform.GetChild(1).gameObject.SetActive(false);
        
        focused += AdjustValue;

        if (focused < 0)
            focused = 7;
        if (focused > 7)
            focused = 0;
        
        slots[focused].transform.GetChild(1).gameObject.SetActive(true);

        return focused;
    }

    public int FocusedSlot2(GameObject[] slots, int AdjustValue, int focused)
    {
        slots[focused].transform.GetChild(1).gameObject.SetActive(false);

        focused += AdjustValue;
        if(focused == 2)
        {
            focused += AdjustValue;
        }
        
        if (focused < 0)
            focused = 4;
        if (focused > 4)
            focused = 0;

        slots[focused].transform.GetChild(1).gameObject.SetActive(true);

        return focused;
    }

    public void PercentSet(int num, int upCount, float upPercent, float Max, Key key, bool enchant)
    {
        addStatus[upCount] += upPercent * 0.01f;
        if (addStatus[upCount] > Max)
            addStatus[upCount] = Max;

        if (enchant)
        {
            equipment[num].key = key;
            equipment[num].enchant = enchant;
            equipment[num].name = key.keyName;
            equipment[num].upStatus = upCount;
            equipment[num].addStatus = addStatus;
        }
        else
        {

        }
    }

    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, float Max, float Min, Key key, bool enchant)
    {
        addStatus[upCount] += upPercent * 0.01f;
        addStatus[downCount] -= downPercent * 0.01f;
        if (addStatus[upCount] > Max)
            addStatus[upCount] = Max;
        if (addStatus[downCount] < Min)
            addStatus[downCount] = Min;

        if (enchant)
        {
            equipment[num].key = key;
            equipment[num].enchant = enchant;
            equipment[num].name = key.keyName;
            equipment[num].upStatus = upCount;
            equipment[num].downStatus = downCount;
            equipment[num].addStatus = addStatus;
        }
        else
        {

        }
    }
    
    public void BoolInit(bool[] b)
    {
        int length = b.Length;
        for (int i = 0; i < length; i++)
            b[i] = false;
    }
}
