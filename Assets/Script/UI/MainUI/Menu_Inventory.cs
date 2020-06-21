using System.Collections;
using UnityEngine;

public class Menu_Inventory : Menu_InGameMenu
{
    public Menu_Storage storage;
    public TownUI_Shop shop;
    public MainUI_PlayerStatusInfo statusInfo;
    public GameObject moneyText;
    
    public int seletedItemCount;         // 창고에서 선택된 아이템 수
    public int[] storageSelectedItem;
    
    public bool isShopOpen;
    public bool isDungeonOpen;
    public int money;

    public override void Init()
    {
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;
        money = 0;

        slot = new GameObject[24];
        itemList = new Item[24];
        storageSelectedItem = new int[24];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            itemList[i - 1] = null;
            storageSelectedItem[i - 1] = 99;
        }
        isUIOn = false;
        isItemSelect = false;
        SetAvailableSlot(0);
        SetMoneyGameObject();
    }
    private void Update()
    {
        if (canvasManager.isCancelOn) return;
        if (!isUIOn) return;
        
        if (isItemSelect)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { slotInstance.ItemConfirmFocus(1); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { slotInstance.ItemConfirmFocus(-1); }

            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if(slotInstance.GetFocus() == 0)
                {
                    if (isDungeonOpen)
                    {
                        UseKeyInDungeon(focused);
                        slotInstance.SetDisActiveItemConfirm();
                    }
                    if (isShopOpen)
                    {
                        money += itemList[focused].itemRarity;
                        UseItem(focused);
                        SetMoneyGameObject();
                        slotInstance.SetDisActiveItemConfirm();
                    }
                }
                else
                {
                    if (isShopOpen)
                    {
                        slotInstance.SetDisActiveItemConfirm();
                    }
                    else
                    {
                        UseItem(focused);
                        itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[focused]);
                        slotInstance.SetDisActiveItemConfirm();
                    }
                }
                isItemSelect = false;
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))    // 아이템 선택 취소
            {
                slotInstance.SetDisActiveItemConfirm();
                isItemSelect = false;
            }
        }
        else
        {
            if (isShopOpen)
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { FocusedSlot(1); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { FocusedSlot(-1); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { FocusedSlot(6); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { FocusedSlot(-6); }

                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    if (itemList[focused] == null) return;
                    slotInstance.SetActiveItemConfirm("판매", "취소");
                    isItemSelect = true;
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
                {
                    isUIOn = false;
                    cursor.SetActive(false);
                    canvasManager.CloseShopInventory();
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Jump"]))    // 포커스 상점으로 변경
                {
                    isUIOn = false;
                    Invoke("ShopFocusChange", 0.01f);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { FocusedSlot(1); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { FocusedSlot(-1); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { FocusedSlot(6); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { FocusedSlot(-6); }

                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    if (itemList[focused] == null) return;

                    slotInstance.SetActiveItemConfirm("사용", "버리기");
                    isItemSelect = true;
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))    // 아이템 선택 취소
                {
                    if (isDungeonOpen)
                    {
                        isDungeonOpen = false;
                    }
                    canvasManager.CloseInGameMenu();
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Jump"]))    // 포커스 상점으로 변경
                {
                    isUIOn = false;
                    Invoke("PlayerStatusFocusChange", 0.01f);
                }
            }

            FocusMove();
        }
    }

    public void FocusOn()
    {
        slotInstance = slot[focused].GetComponent<Slot>();
        cursor.SetActive(true);
        isUIOn = true;
    }
    public void PlayerStatusFocusChange()
    {
        isUIOn = false;
        cursor.SetActive(false);
        statusInfo.FocusOn();
        Debug.Log("Focus : playerStatusInfo");
    }

    #region shop 관련

    public void OpenInventory(TownUI_Shop _shop)   // 상점에서 인벤토리 열 때
    {
        isUIOn = false;
        isShopOpen = true;
        focused = 0;
        slotInstance = slot[focused].GetComponent<Slot>();

        shop = _shop;
        InventorySet();

        if (itemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[0]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
    }
    public int BuyItem(Item _Item, int _price)
    {
        if (money < _price) return 1;
        if (!PutInInventory(_Item, true)) return 2;
        money -= _price;
        SetMoneyGameObject();
        InventorySet();
        return 0;
    }
    public void ShopFocusChange()
    {
        cursor.SetActive(false);
        shop.FocusOn();
        isUIOn = false;
        Debug.Log("Focus : shop");
    }
    public void SetMoneyGameObject()
    {
        moneyText.GetComponent<UnityEngine.UI.Text>().text = money.ToString();
    }

    #endregion

    #region Dungeon 관련

    public void UseKeyInDungeon(int _focused)      // 던전 포탈 앞에서 아이템 사용시
    {
        if (DungeonManager.instance.UseKeyInDungeon(itemList[_focused]))
        {
            UseItem(_focused);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[_focused]);
            canvasManager.CloseInGameMenu();
            DungeonManager.instance.WaitingEventStart();
        }
    }
    public void UseItemInQuickSlot(int _focused)   // 퀵슬롯으로 아이템 사용시 ( 마을에서 사용시 창고도 비우기 or 마을에서 사용 x )
    {
        DungeonManager.instance.UseItemInDungeon(itemList[_focused]);
        UseItem(_focused);
    }
    public void PutInBox(bool _isDead)             // 던전에서 복귀할 때 창고에 키 넣기
    {
        int randomCount;
        for (int i = 0; i < availableSlot; ++i)
        {
            if (_isDead)
            {
                randomCount = Random.Range(0, 4);
                if (randomCount < 3)
                {
                    if (storageSelectedItem[i] == 99)
                    {
                        storage.PutInBox(itemList[i], false);
                    }
                }
            }
            else
            {
                if (storageSelectedItem[i] == 99)
                {
                    storage.PutInBox(itemList[i], false);
                }
            }
            UseItem(i);
        }
        isUIOn = false;
    }

    #endregion

    public void OpenInventory(bool _isDungeon, MainUI_PlayerStatusInfo _StatusInfo)     // I 키로 인벤토리 열 때
    {
        isUIOn = true;
        isDungeonOpen = _isDungeon;
        statusInfo = _StatusInfo;
        focused = 0;
        slotInstance = slot[focused].GetComponent<Slot>();
        cursor.SetActive(true);

        InventorySet();

        if (itemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[0]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
    }
    public void CloseInventory()
    {
        cursor.SetActive(false);
        itemInformation.SetActive(false);
        isShopOpen = false;
        isDungeonOpen = false;
        isUIOn = false;
        slotInstance.SetDisActiveItemConfirm();
    }
    public void InventorySet()                     // 인벤토리 활성화시 아이템 세팅
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] != null)
            {
                slot[i].GetComponent<Menu_InGameSlot>().SetItemSprite(itemList[i]);
            }
            else
            {
                slot[i].GetComponent<Menu_InGameSlot>().SetItemSprite(null);
            }
        }
        for(int i = availableSlot; i < 24; ++i)
        {
            slot[i].GetComponent<Menu_InGameSlot>().SetOverSlot();
        }
    }
    public void SetSelectedItem(int _value, int[] _selectedSlot)    // 창고에서 선택된 아이템 가져오기
    {
        seletedItemCount = _value;

        for (int i = 0; i < seletedItemCount; ++i)
        {
            itemList[i] = storage.GetSelectStorageItem(_selectedSlot[i]);
            storageSelectedItem[i] = _selectedSlot[i];
        }
        for (int i = seletedItemCount; i < availableSlot; ++i)
        {
            itemList[i] = null;
            storageSelectedItem[i] = 99;
        }
    }
    
    public void UseItem(int _focused)          // 아이템 한개 인벤토리에서 제거
    {
        if(storageSelectedItem[_focused] != 99)
        {
            storage.DeleteItem(storageSelectedItem[_focused]);
        }
        DeleteItem(_focused);
        InventorySet();
    }
    public void DeleteItem(int _focused)
    {
        itemList[_focused] = null;
        storageSelectedItem[_focused] = 99;
    }

    public int GetSelectedItemCount()
    {
        return seletedItemCount;
    }          // 선택된 아이템 수 반환
    public int GetAvailableSlot()
    {
        return availableSlot;
    }              // 사용 가능한 슬롯 수 반환
    public int GetCurrentMoney()
    {
        return money;
    }               // 사용 가능한 슬롯 수 반환

    public Item[] GetItemList()
    {
        return itemList;
    }                // 인벤토리 아이템 리스트 반환
    public Item GetItem(int _focus)
    {
        return itemList[_focus];
    }
    public bool PutInInventory(Item _Item, bool _IsBuy)        // 아이템 획득시 인벤토리
    {
        for (int i = 0; i < availableSlot; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = _Item;
                storageSelectedItem[i] = storage.PutInBox(_Item, _IsBuy);

                return true;
            }
        }
        return false;
    }

    public void LoadInventoryData(int _BagRarity, int _money)
    {
        SetAvailableSlot(_BagRarity);
        money = _money;
        SetMoneyGameObject();
    }
    public void SetAvailableSlot(int _BagRarity)
    {
        int _availableSlot = 6 * (1 + _BagRarity);
        
        for(int i = _availableSlot; i < 24; ++i)
        {
            if(itemList[i] != null)
            {
                itemList[i] = null;
                if(storageSelectedItem[i] != 99)
                {
                    storage.InventorySelectCancel(storageSelectedItem[i]);
                    storageSelectedItem[i] = 99;
                    --seletedItemCount;
                } 
            }
        }

        availableSlot = _availableSlot;
    }

    public void FocusMove()
    {
        if (focused < 0 || focused > availableSlot) return;
        cursor.transform.position = Vector2.Lerp(cursor.transform.position, slot[focused].transform.position, Time.deltaTime * cursorSpeed);
    }
    public new void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot-1) { return; }
        
        focused += AdjustValue;

        if (itemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[focused]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
        slotInstance = slot[focused].GetComponent<Slot>();
    }
}
