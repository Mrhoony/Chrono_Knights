using System.Collections;
using UnityEngine;

public class Menu_Inventory : Menu_InGameMenu
{
    public Menu_Storage storage;
    public TownUI_Shop shop;
    public GameObject moneyText;

    public GameObject cursorInvenSelect;
    public float cursorSpd;
    
    public int seletedItemCount;         // 창고에서 선택된 아이템 수
    public int[] storageSelectedItem;
    public bool isDungeonOpen;
    public bool isShopOpen;
    public bool isThisWindowFocus;
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
            if (Input.GetKeyDown(KeyCode.RightArrow)) { slotInstance.ItemConfirmFocus(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { slotInstance.ItemConfirmFocus(-1); }

            if (Input.GetKeyDown(KeyCode.Z))
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
                        DeleteUseItem(focused);
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
                        DeleteItem(focused);
                        itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[focused]);
                        slotInstance.SetDisActiveItemConfirm();
                    }
                }
                isItemSelect = false;
            }
            if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
            {
                slotInstance.SetDisActiveItemConfirm();
                isItemSelect = false;
            }
        }
        else
        {
            if (isShopOpen)
            {
                if (!isThisWindowFocus) return;

                if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (itemList[focused] == null) return;
                    slotInstance.SetActiveItemConfirm("판매", "취소");
                    isItemSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    isThisWindowFocus = false;
                    cursorInvenSelect.SetActive(false);
                    canvasManager.CloseShopInventory();
                }
                if (Input.GetKeyDown(KeyCode.C))    // 포커스 상점으로 변경
                {
                    isThisWindowFocus = false;
                    cursorInvenSelect.SetActive(false);
                    StartCoroutine(FocusChange());
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
                if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (itemList[focused] == null) return;

                    slotInstance.SetActiveItemConfirm("사용", "버리기");
                    isItemSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
                {
                    if (isDungeonOpen)
                    {
                        isDungeonOpen = false;
                    }
                    canvasManager.CloseInGameMenu();
                }
            }
        }

        FocusMove();
    }

    #region shop 관련

    public void OpenInventory(TownUI_Shop _shop)   // 상점에서 인벤토리 열 때
    {
        isUIOn = true;
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
    public void FocusOn()
    {
        slotInstance = slot[focused].GetComponent<Slot>();
        cursorInvenSelect.SetActive(true);
        isThisWindowFocus = true;
    }
    IEnumerator FocusChange()
    {
        yield return new WaitForSeconds(0.01f);
        shop.FocusOn();
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
            DeleteItem(_focused);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[_focused]);
            canvasManager.CloseInGameMenu();
        }
    }
    public void UseItemInQuickSlot(int _focused)   // 퀵슬롯으로 아이템 사용시 ( 마을에서 사용시 창고도 비우기 or 마을에서 사용 x )
    {
        DungeonManager.instance.UseItemInDungeon(itemList[_focused]);
        DeleteItem(_focused);
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
            DeleteItem(i);
        }
        isUIOn = false;
    }

    #endregion

    public void OpenInventory(bool _isDungeon)     // I 키로 인벤토리 열 때
    {
        isUIOn = true;
        isDungeonOpen = _isDungeon;
        focused = 0;
        slotInstance = slot[focused].GetComponent<Slot>();
        cursorInvenSelect.SetActive(true);

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
        itemInformation.SetActive(false);
        isShopOpen = false;
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

    public void DeleteUseItem(int _focused)        // 사용된 아이템 인벤토리에서 제거
    {
        DeleteItem(_focused);

        for (int i = _focused; i < availableSlot - 1; ++i)
        {
            if (itemList[i] != null) break;

            for (int j = 1; j < availableSlot - i; ++i)
            {
                if (i + j == availableSlot)
                {
                    i = availableSlot - 1;
                    break;
                }

                if (itemList[i + j] != null)
                {
                    itemList[i] = itemList[i + j];
                    storageSelectedItem[i] = storageSelectedItem[i + j];

                    itemList[i + j] = null;
                    storageSelectedItem[i + j] = 99;
                    break;
                }
            }
        }
        InventorySet();
    }
    public void DeleteItem(int _focused)          // 아이템 한개 인벤토리에서 제거
    {
        itemList[_focused] = null;
        if(storageSelectedItem[_focused] != 99)
        {
            storage.DeleteItem(storageSelectedItem[_focused]);
        }
        storageSelectedItem[_focused] = 99;
        InventorySet();
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
        cursorInvenSelect.transform.position = Vector2.Lerp(cursorInvenSelect.transform.position, slot[focused].transform.position, Time.deltaTime * cursorSpd);
    }
    public override void FocusedSlot(int AdjustValue)
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
