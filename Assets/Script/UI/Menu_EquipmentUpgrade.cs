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
    public int equipFocused;

    public int upgradeCount;
    public float upgradePercent;
    public int downgradeCount;
    public float downgradePercent;

    public int keySlotFocus;

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

    public int FocusSlotEquipmentSelect(GameObject[] slots, int AdjustValue, int focused)
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

    public int FocusSlotItemSelect(GameObject[] slots, int AdjustValue, int focused)
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
            equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);
            
            if (equipment[num].downStatus != 8)
            {
                equipment[num].addStatus[equipment[num].downStatus] = 0;
                equipment[num].downStatus = 8;
            }
        }
        else
        {
            equipment[num].EquipmentStatusUpgrade(upCount, upPercent, true);
        }
    }

    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, Item item, bool enchant)
    {
        if (enchant)
        {
            equipment[num].EquipmentItemSetting(item);
            equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);
            equipment[num].EquipmentStatusEnchant(downCount, downPercent, false);
        }
        else
        {
            equipment[num].EquipmentStatusUpgrade(upCount, upPercent, true);

            if (equipment[num].downStatus != 8)
            {
                equipment[num].EquipmentStatusUpgrade(downCount, downPercent, false);
            }
        }
    }
}
