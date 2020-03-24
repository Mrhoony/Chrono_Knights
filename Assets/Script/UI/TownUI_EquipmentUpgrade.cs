using UnityEngine;
using UnityEngine.UI;

public class TownUI_EquipmentUpgrade : MonoBehaviour
{
    public TownUI townUI;
    protected PlayerStatus playerStat;
    protected CanvasManager canvasManager;
    protected Menu_Storage storage;
    protected Database_Game itemDatabase;
    public GameObject[] acceptSlot;
    public GameObject upgradeButton;
    public GameObject npc_blacksmith;
    protected PlayerEquipment playerEquipment;
    protected PlayerEquipment.Equipment[] equipment;
    public Item selectedkey;

    public GameObject[] equipSlots;
    
    public bool open_BlackSmithUI;
    public bool open_SelectItemUI;
    public bool open_ReSelectEquipment;
    public int selectEquipFocused;
    public int selectItemUIFocused;

    public bool isTownMenuOn = false;
    public int keySlotFocus;

    // Start is called before the first frame update
    public void Awake()
    {
        canvasManager = transform.parent.GetComponent<TownUI>().canvasManager;
        itemDatabase = Database_Game.instance;
        storage = canvasManager.storage.GetComponent<Menu_Storage>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerEquipment = playerStat.playerEquip;
    }

    public void OpenTownUIMenu()
    {
        open_BlackSmithUI = true;
        open_SelectItemUI = false;
        open_ReSelectEquipment = false;
        equipment = playerEquipment.equipment;
        selectEquipFocused = 0;
        equipSlots[selectEquipFocused].transform.GetChild(0).gameObject.SetActive(true);           // 1번 슬롯 포커스 온
        SelectEquipmentSet();
    }
    public void SelectEquipmentSet()
    {
        for (int i = 0; i < 7; ++i)
        {
            equipSlots[i].transform.GetChild(1).gameObject.SetActive(true);
            equipSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.itemSprite[i];       // 장비 아이템
            if (equipment[i].itemCode != 0)
            {
                equipSlots[i].transform.GetChild(2).gameObject.SetActive(true);
                SetSlot(equipSlots[i], i, 1);
            }
            else
            {
                equipSlots[i].transform.GetChild(2).gameObject.SetActive(false);
                ClearSlot(equipSlots[i], 2);
            }
        }
    }

    public void OpenSelectedItemMenu()
    {
        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);

        acceptSlot[0].SetActive(true);
        acceptSlot[1].SetActive(true);
        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(false);       // 장비 아이템
        acceptSlot[1].transform.GetChild(2).gameObject.SetActive(false);       // 장비 아이템

        selectItemUIFocused = 0;
        acceptSlot[0].transform.GetChild(selectItemUIFocused).gameObject.SetActive(true);           // 1번 슬롯 포커스 온
        acceptSlot[0].transform.GetChild(1).gameObject.SetActive(true);       // 장비 아이템
        acceptSlot[0].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.itemSprite[selectEquipFocused];       // 장비 아이템

        if (equipment[selectEquipFocused].itemCode != 0)
        {
            SetSlot(acceptSlot[0], selectEquipFocused, 1);
        }
        else
        {
            ClearSlot(acceptSlot[0], 2);
        }
        acceptSlot[2].SetActive(false);
    }

    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = storage.GetStorageItem(focus);
        
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.keyItemBorderSprite[selectedkey.itemRarity];
        acceptSlot[1].transform.GetChild(2).GetComponent<Image>().sprite = selectedkey.sprite;

        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(true);       // 장비 아이템
        acceptSlot[1].transform.GetChild(2).gameObject.SetActive(true);       // 장비 아이템
    }

    public void SetSlot(GameObject _slot, int _slotNum, int _startNum)
    {
        _slot.transform.GetChild(_startNum + 1).gameObject.SetActive(true);
        _slot.transform.GetChild(_startNum + 1).GetComponent<Image>().sprite = SpriteSet.keyItemBorderSprite[equipment[_slotNum].itemRarity]; // 레어도
        _slot.transform.GetChild(_startNum + 2).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, true) + "+" + playerEquipment.GetUpStatus(_slotNum) + " %";

        if(playerEquipment.GetStatusName(_slotNum, false) != "")
        {
            _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, false) + "-" + playerEquipment.GetDownStatus(_slotNum) + " %";
        }
        else
        {
            _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = "";
        }
    }
    public void ClearSlot(GameObject _slot, int _startNum)
    {
        _slot.transform.GetChild(_startNum).gameObject.SetActive(false);
        _slot.transform.GetChild(_startNum + 1).GetComponent<Text>().text = "인챈트 없음";
        _slot.transform.GetChild(_startNum + 2).GetComponent<Text>().text = "";
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
}
