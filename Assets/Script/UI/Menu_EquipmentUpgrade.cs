using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_EquipmentUpgrade : MonoBehaviour
{
    public PlayerStat playerStat;
    public PlayerData playerData;
    public Menu_InGame menu;
    public Menu_Inventory inventory;

    public GameObject npc_blacksmith;
    public PlayerEquipment playerEquipment;
    public PlayerEquipment.Equipment[] equipment;
    public bool upgradeSet;

    public PlayerEquipment.Equipment upgradeEquipment;
    public Key selectedkey;
    public int[] limitUpgrade;

    public GameObject[] equipSlots;
    public int equipFocused;

    public float[] addStat;
    public bool[] downStat;

    public int upGradeCount;
    public int upGradePercent;
    public int downGradeCount;
    public int downGradePercent;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        upgradeSet = false;
        menu = transform.parent.GetComponent<Menu_TownUI>().menuIngame;
        inventory = menu.Menus[0].GetComponent<Menu_Inventory>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
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

        if (focused < 0)
            focused = 3;
        if (focused > 3)
            focused = 0;
        
        slots[focused].transform.GetChild(1).gameObject.SetActive(true);

        return focused;
    }

    public void PercentSet(int num, int upCount, float upPercent, float Max, Key _key, bool enchant)
    {
        addStat[upCount] += upPercent * 0.01f;
        if (addStat[upCount] > Max)
            addStat[upCount] = Max;
        if (enchant)
            playerEquipment.SetKeyItem(num, _key);
        playerEquipment.SetEquipOption(num, "test", addStat);
    }

    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, float Max, float Min, Key _key, bool enchant)
    {
        addStat[upCount] += upPercent * 0.01f;
        addStat[downCount] -= downPercent * 0.01f;
        if (addStat[upCount] > Max)
            addStat[upCount] = Max;
        if (addStat[downCount] < Min)
            addStat[downCount] = Min;

        if (enchant)
            playerEquipment.SetKeyItem(num, _key);
        playerEquipment.SetEquipOption(num, "test", addStat);
    }

    public void BoolInit(bool[] b)
    {
        int length = b.Length;
        for (int i = 0; i < length; i++)
            b[i] = false;
    }
}
