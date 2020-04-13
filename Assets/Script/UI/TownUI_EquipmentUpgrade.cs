using UnityEngine;
using UnityEngine.UI;

public class TownUI_EquipmentUpgrade : MonoBehaviour
{
    public TownUI townUI;
    protected PlayerStatus playerStat;
    protected CanvasManager canvasManager;
    protected Menu_Storage storage;
    protected Database_Game itemDatabase;
    public GameObject npc_blacksmith;

    protected PlayerEquipment playerEquipment;
    protected PlayerEquipment.Equipment[] equipment;

    public GameObject[] acceptSlot;
    public Item selectedkey;
    public GameObject upgradeButton;
    public GameObject itemCancel;

    public GameObject[] equipSlots;
    public GameObject equipCancel;

    public GameObject selectUseItem;

    public GameObject cursorEquipSelect;
    public GameObject cursorItemSelect;
    public float cursorSpd;
    
    public bool open_BlackSmithUI;
    public bool open_SelectItemUI;
    // public bool open_ReSelectEquipment;
    public int selectEquipFocused;
    public int selectItemUIFocused;
    
    public int keySlotFocus;

    // Start is called before the first frame update
    public void Awake()
    {
        canvasManager = transform.parent.GetComponent<TownUI>().canvasManager;
        itemDatabase = Database_Game.instance;
        storage = canvasManager.storage.GetComponent<Menu_Storage>();
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerEquipment = playerStat.playerData.playerEquipment;
    }

    public void OpenTownUIMenu()
    {
        open_BlackSmithUI = true;
        open_SelectItemUI = false;
        // open_ReSelectEquipment = false;
        equipment = playerEquipment.equipment;
        SelectEquipmentSet();
    }
    public void SelectEquipmentSet()
    {
        selectEquipFocused = 0;
        cursorEquipSelect.transform.position = new Vector2(equipSlots[0].transform.position.x - 3.5f, equipSlots[0].transform.position.y);
        cursorEquipSelect.SetActive(true);           // 1번 슬롯 포커스 온

        for (int i = 0; i < 7; ++i)
        {
            if (equipment[i].itemCode != 0)
            {
                SetSlot(equipSlots[i], i, 0);
            }
            else
            {
                ClearSlot(equipSlots[i], i, 0);
            }
        }
    }
    public void OpenSelectedItemMenu()
    {
        upgradeButton.GetComponent<Image>().color = new Color(upgradeButton.GetComponent<Image>().color.r,
            upgradeButton.GetComponent<Image>().color.g, upgradeButton.GetComponent<Image>().color.b, 255);
        
        selectItemUIFocused = 0;

        acceptSlot[0].SetActive(true);
        acceptSlot[0].transform.GetChild(0).gameObject.SetActive(true);       // 장비 아이템
        acceptSlot[0].transform.GetChild(0).GetComponent<Image>().sprite = SpriteSet.itemSprite[selectEquipFocused];       // 장비 아이템
        cursorItemSelect.transform.position = new Vector2(acceptSlot[0].transform.position.x, acceptSlot[0].transform.position.y + 35f);
        cursorItemSelect.SetActive(true);           // 1번 슬롯 포커스 온

        acceptSlot[1].SetActive(true);
        acceptSlot[1].transform.GetChild(0).gameObject.SetActive(false);       // 아이템 슬롯
        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(false);       // 아이템 레어리티

        acceptSlot[2].SetActive(true);
        if (equipment[selectEquipFocused].itemCode != 0)
        {
            SetSlot(acceptSlot[2], selectEquipFocused, 0);
        }
        else
        {
            ClearSlot(acceptSlot[2], selectEquipFocused, 0);
        }

        acceptSlot[3].SetActive(false);
    }
    public void CloseSelectedItemMenu()
    {
        open_SelectItemUI = false;
        selectItemUIFocused = 0;
        cursorItemSelect.SetActive(false);
    }

    public void SetKey(int focus)
    {
        keySlotFocus = focus;
        selectedkey = storage.GetStorageItem(focus);

        acceptSlot[1].transform.GetChild(0).GetComponent<Image>().sprite = selectedkey.sprite;
        acceptSlot[1].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.enchantSlotImage[selectedkey.itemRarity];

        acceptSlot[1].transform.GetChild(0).gameObject.SetActive(true);
        acceptSlot[1].transform.GetChild(1).gameObject.SetActive(true);
    }
    public void SetSlot(GameObject _slot, int _slotNum, int _startNum)
    {
        _slot.transform.GetChild(_startNum).gameObject.SetActive(true);
        _slot.transform.GetChild(_startNum).GetComponent<Image>().sprite = SpriteSet.itemSprite[_slotNum];
        _slot.transform.GetChild(_startNum + 1).gameObject.SetActive(true);
        _slot.transform.GetChild(_startNum + 1).GetComponent<Image>().sprite = SpriteSet.enchantSlotImage[equipment[_slotNum].itemRarity]; // 레어도
        _slot.transform.GetChild(_startNum + 2).gameObject.SetActive(true);
        _slot.transform.GetChild(_startNum + 2).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, true) + "+" + playerEquipment.GetUpStatus(_slotNum) + " %";

        if(playerEquipment.GetStatusName(_slotNum, false) != "")
        {
            _slot.transform.GetChild(_startNum + 3).gameObject.SetActive(true);
            _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = playerEquipment.GetStatusName(_slotNum, false) + playerEquipment.GetDownStatus(_slotNum) + " %";
        }
        else
        {
            _slot.transform.GetChild(_startNum + 3).gameObject.SetActive(true);
            _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = "";
        }
    }
    public void ClearSlot(GameObject _slot, int _slotNum, int _startNum)
    {
        _slot.transform.GetChild(_startNum).gameObject.SetActive(true);
        _slot.transform.GetChild(_startNum).GetComponent<Image>().sprite = SpriteSet.itemSprite[_slotNum];
        _slot.transform.GetChild(_startNum + 1).gameObject.SetActive(false);
        _slot.transform.GetChild(_startNum + 2).GetComponent<Text>().text = "인챈트 없음";
        _slot.transform.GetChild(_startNum + 3).GetComponent<Text>().text = "";
    }

    public void FocusEquipSlotMove(GameObject _slot)
    {
        cursorEquipSelect.transform.position
            = Vector2.Lerp(cursorEquipSelect.transform.position, new Vector2(_slot.transform.position.x - 35f, _slot.transform.position.y), Time.deltaTime * cursorSpd);
    }
    public void FocusItemSlotMove(GameObject _slot)
    {
        cursorItemSelect.transform.position
            = Vector2.Lerp(cursorItemSelect.transform.position, new Vector2(_slot.transform.position.x, _slot.transform.position.y + 35f), Time.deltaTime * cursorSpd);
    }

    public int FocusSlotEquipmentSelect(int AdjustValue, int _focused)
    {
        _focused += AdjustValue;

        if (_focused < 0)
            _focused = 7;
        if (_focused > 7)
            _focused = 0;

        if(_focused == 7)
        {
            cursorEquipSelect.SetActive(false);
            equipCancel.SetActive(true);
        }
        else
        {
            equipCancel.SetActive(false);
            cursorEquipSelect.SetActive(true);
        }

        return _focused;
    }
    public int FocusSlotItemSelect(int AdjustValue, int _focused)
    {
        _focused += AdjustValue;
        
        if (_focused < 0)
            _focused = 3;
        if (_focused > 3)
            _focused = 0;

        if (_focused == 2)
        {
            cursorItemSelect.SetActive(false);
            itemCancel.SetActive(false);
            upgradeButton.SetActive(true);
        }
        else if(_focused == 3)
        {
            cursorItemSelect.SetActive(false);
            upgradeButton.SetActive(false);
            itemCancel.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(false);
            itemCancel.SetActive(false);
            cursorItemSelect.SetActive(true);
        }

        return _focused;
    }
}
