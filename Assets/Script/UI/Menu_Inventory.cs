using UnityEngine;

public class Menu_Inventory : Menu_InGameMenu
{
    public Menu_Storage storage;

    public int inventoryItemCount;

    public int seletedItemCount;         // 창고에서 선택된 아이템 수
    public int takeItemSlot;             // 가져갈 수 있는 슬롯 수

    public bool isDungeonOpen;

    public override void Init()
    {
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;

        slot = new GameObject[24];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
        }
        itemList = new Item[24];
        isFull = new bool[24];
        for (int i = 0; i < slotCount; ++i)
        {
            itemList[i] = null;
            isFull[i] = false;
        }
        isUIOn = false;
        isItemSelect = false;
        inventoryItemCount = 0;
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
                    }
                }
                else
                {
                    DeleteItem(focused);
                    slotInstance.SetActiveItemConfirm(false);
                }
                isItemSelect = false;
            }
            if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
            {
                slotInstance.SetActiveItemConfirm(false);
                isItemSelect = false;
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

                isItemSelect = true;
                slotInstance.SetActiveItemConfirm(true);
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

    public void UseKeyInDungeon(int _focused)              // 던전 포탈 앞에서 아이템 사용시
    {
        if (DungeonManager.instance.UseKeyInDungeon(itemList[_focused]))
        {
            DeleteItem(_focused);
            canvasManager.CloseInGameMenu();
        }
    }
    public void UseItemInQuickSlot(int _focused)             // 퀵슬롯으로 아이템 사용시 ( 마을에서 사용시 창고도 비우기 or 마을에서 사용 x )
    {
        DungeonManager.instance.UseItemInDungeon(itemList[_focused]);
        DeleteItem(_focused);
    }

    public void OpenInventory(bool _inDungeon)
    {
        isUIOn = true;
        isDungeonOpen = _inDungeon;

        focused = 0;
        slotInstance = slot[focused].GetComponent<Menu_Slot>();
        slotInstance.SetActiveFocus(true);

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
    public void InventorySet()           // 인벤토리 활성화시 아이템 세팅
    {
        Sprite[] keyItemBorderSprite = canvasManager.keyItemBorderSprite;

        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] != null)
            {
                slot[i].GetComponent<Menu_Slot>().SetItemSprite(itemList[i].sprite, keyItemBorderSprite[itemList[i].itemRarity], true);
                isFull[i] = true;
            }
            else
            {
                slot[i].GetComponent<Menu_Slot>().SetItemSprite(null, keyItemBorderSprite[4], false);
                isFull[i] = false;
            }
        }
        for(int i = availableSlot; i < 24; ++i)
        {
            slot[i].GetComponent<Menu_Slot>().SetOverSlot(keyItemBorderSprite[0]);
        }
    }
    public void CloseInventory()
    {
        slotInstance.SetActiveFocus(false);
        itemInformation.SetActive(false);
        isUIOn = false;
    }

    public void SetSelectedItemCount(int value)
    {
        seletedItemCount = value;
    }
    public void SetItemList()
    {
        for(int i = 0; i < seletedItemCount; ++i)
        {
            itemList[i] = storage.GetSelectStorageItem(i);
            isFull[i] = true;
        }
        for(int i = seletedItemCount; i < availableSlot; ++i)
        {
            itemList[i] = null;
            isFull[i] = false;
        }
    }
    public void takeItemSlotUpgrade(int upgrade)
    {
        takeItemSlot += upgrade;
    }
    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
        for (int i = availableSlot; i < slotCount; ++i)
        {
            isFull[i] = true;
        }
    }

    public void PutInBox(bool isDead)           // 던전에서 복귀할 때 창고에 키 넣기
    {
        if (isDead)
        {
            for(int i = takeItemSlot; i < availableSlot; ++i)
            {
                itemList[i] = null;
            }
            storage.GetComponent<Menu_Storage>().PutInBox(itemList);
        }
        else
        {
            storage.GetComponent<Menu_Storage>().PutInBox(itemList);
        }
        InventoryClear();
    }
    public void InventoryClear()
    {
        for (int i = 0; i < 24; ++i)
        {
            itemList[i] = null;
            isFull[i] = false;
        }
        isUIOn = false;
        inventoryItemCount = 0;
    }
    public void DeleteItem(int _focused)          // 사용된 아이템 인벤토리에서 제거
    {
        itemList[_focused] = null;
        isFull[_focused] = false;

        for (int i = _focused; i < availableSlot - 1; ++i)
        {
            if (itemList[i] != null) continue;

            for (int j = 1; j < availableSlot - i; ++i)
            {
                if (itemList[i + j] != null)
                {
                    itemList[i] = itemList[i + j];
                    isFull[i] = true;

                    itemList[i + j] = null;
                    isFull[i + j] = false;

                    if (i + j == availableSlot)
                    {
                        i = availableSlot - 1;
                    }
                    break;
                }
                else
                {
                    if (i + j == availableSlot)
                    {
                        itemList[i] = null;
                        isFull[i] = false;
                        i = availableSlot - 1;
                        break;
                    }
                }
            }
        }
        --inventoryItemCount;
        InventorySet();
    }
    public void DeleteStorageItem()             // 던전 진입할 때 들고있는 키 창고에서 삭제
    {
        for (int i = 0; i < seletedItemCount; ++i)
        {
            itemList[i] = storage.GetSelectStorageItem(i);
            isFull[i] = true;
        }
        for (int i = seletedItemCount; i < availableSlot; ++i)    // 창고에서 꺼낸 키 외에는 빈슬롯
        {
            itemList[i] = null;
            isFull[i] = false;
        }
        storage.DeleteStorageSlotItem();
        seletedItemCount = 0;
    }

    #region get, set, save, load
    public int GetSelectedItemCount()
    {
        return seletedItemCount;
    }
    public int GetAvailableSlot()
    {
        return availableSlot;
    }
    public int GetTakeItemSlot()
    {
        return takeItemSlot;
    }
    public Item[] GetItemList()
    {
        return itemList;
    }

    public void LoadInventoryData(int _takeItemSlot, int _availableSlot)
    {
        takeItemSlot = _takeItemSlot;
        availableSlot = _availableSlot;
    }
    #endregion

    public bool GetKeyItem(Item _Item)        // 아이템 획득시 인벤토리 등록
    {
        for (int i = 0; i < availableSlot; i++)
        {
            if (!isFull[i])
            {
                itemList[i] = _Item;
                isFull[i] = true;
                ++inventoryItemCount;
                return true;
            }
        }
        return false;
    }

    public override void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot-1) { return; }

        slotInstance.SetActiveFocus(false);
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
        slotInstance = slot[focused].GetComponent<Menu_Slot>();
        slotInstance.SetActiveFocus(true);
    }
}
