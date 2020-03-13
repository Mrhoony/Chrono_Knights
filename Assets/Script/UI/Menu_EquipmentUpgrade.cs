using UnityEngine;
using UnityEngine.UI;

public class Menu_EquipmentUpgrade : MonoBehaviour
{
    protected PlayerStatus playerStat;
    protected CanvasManager menu;
    protected Menu_Storage storage;
    protected Database_Game itemDatabase;
    public GameObject[] acceptSlot;
    public GameObject upgradeButton;
    public GameObject npc_blacksmith;
    protected PlayerEquipment playerEquipment;
    protected PlayerEquipment.Equipment[] equipment;
    protected Item selectedkey;

    public GameObject[] equipSlots;
    protected Sprite[] equipmentSet;
    protected Sprite[] keyItemBorderSprite;
    
    public bool open_BlackSmithUI;
    public bool open_SelectItemUI;
    public bool open_ReSelectEquipment;
    public int selectEquipFocused;
    public int selectItemUIFocused;

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
        playerEquipment = playerStat.playerEquip;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        equipmentSet = Resources.LoadAll<Sprite>("Item/ui_itemset");
    }
    
    public void OpenBlackSmithUI()
    {
        open_BlackSmithUI = true;
        open_SelectItemUI = false;
        open_ReSelectEquipment = false;
        selectEquipFocused = 0;
        equipment = playerEquipment.equipment;
        equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(true);

        for (int i = 0; i < 7; ++i)
        {
            equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = equipmentSet[i];       // 키 아이템
            equipSlots[i].transform.GetChild(1).gameObject.SetActive(true);
            if (equipment[i].itemCode != 0)
            {
                SetSlot(equipSlots[i], i, 2);
            }
            else
            {
                ClearSlot(equipSlots[i], 2);
            }
        }
    }
    public void OpenSelectedItemMenu()
    {
        selectItemUIFocused = 0;
        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);

        acceptSlot[0].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[0].transform.GetChild(1).gameObject.SetActive(true);
        acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = equipmentSet[selectEquipFocused];

        if (equipment[selectEquipFocused].itemCode != 0)
        {
            SetSlot(acceptSlot[0], selectEquipFocused, 2);
        }
        else
        {
            ClearSlot(acceptSlot[0], 2);
        }

        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(false);
        acceptSlot[1].transform.GetChild(2).gameObject.SetActive(false);
        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(false);
        ClearSlot(acceptSlot[2], 1);
    }

    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = storage.GetStorageItem(focus);

        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(true);
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[1].transform.GetChild(2).GetComponent<Image>().sprite = keyItemBorderSprite[selectedkey.itemRarity];

        acceptSlot[2].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[2].transform.GetChild(0).GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[2].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[selectedkey.itemRarity];
    }

    public void SetSlot(GameObject _slot, int _slotNum, int _startNum)
    {
        _slot.transform.GetChild(_startNum).GetComponent<Image>().sprite = keyItemBorderSprite[equipment[_slotNum].itemRarity]; // 레어도
        _slot.transform.GetChild(_startNum + 1).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, true);
        _slot.transform.GetChild(_startNum + 2).GetComponent<Text>().text = playerEquipment.GetUpStatus(_slotNum);
        _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, false);
        _slot.transform.GetChild(_startNum + 4).GetComponent<Text>().text = playerEquipment.GetDownStatus(_slotNum);
    }
    public void ClearSlot(GameObject _slotNum, int _startNum)
    {
        _slotNum.transform.GetChild(_startNum).gameObject.SetActive(false);
        _slotNum.transform.GetChild(_startNum + 1).GetComponent<Text>().text = "";
        _slotNum.transform.GetChild(_startNum + 2).GetComponent<Text>().text = "";
        _slotNum.transform.GetChild(_startNum + 3).GetComponent<Text>().text = "";
        _slotNum.transform.GetChild(_startNum + 4).GetComponent<Text>().text = "";
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

    public void PercentSet(int num, int upCount, float upPercent, Item item)
    {
        equipment[num].EquipmentItemSetting(item);
        equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);

        if (equipment[num].downStatus != 8)
        {
            equipment[num].addStatus[equipment[num].downStatus] = 0;
            equipment[num].downStatus = 8;
        }
    }
    public void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, Item item)
    {
        equipment[num].EquipmentItemSetting(item);
        equipment[num].EquipmentStatusEnchant(upCount, upPercent, true);
        equipment[num].EquipmentStatusEnchant(downCount, downPercent, false);
    }
    public void PercentSet(int num, float upPercent, Item item)
    {
        equipment[num].EquipmentStatusUpgrade(equipment[num].upStatus, upPercent, true);
    }
    public void PercentSet(int num, float upPercent, float downPercent, Item item)
    {
        equipment[num].EquipmentStatusUpgrade(equipment[num].upStatus, upPercent, true);

        if (equipment[num].downStatus != 8)
        {
            equipment[num].EquipmentStatusUpgrade(equipment[num].downStatus, downPercent, false);
        }
    }
}
