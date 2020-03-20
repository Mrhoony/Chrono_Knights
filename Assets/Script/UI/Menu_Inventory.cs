﻿using UnityEngine;

public class Menu_Inventory : Menu_InGameMenu
{
    public Menu_Storage storage;
    public int seletedItemCount;         // 창고에서 선택된 아이템 수
    public int[] storageSelectedItem;
    public bool isDungeonOpen;
    public bool isShopOpen;

    public override void Init()
    {
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;

        slot = new GameObject[24];
        itemList = new Item[24];
        isFull = new bool[24];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            itemList[i - 1] = null;
            isFull[i - 1] = false;
        }
        isUIOn = false;
        isItemSelect = false;
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

    public void OpenInventory()                     // 상점에서 인벤토리 열 때
    {
        isUIOn = true;
        isShopOpen = true;
        focused = 0;
        slotInstance = slot[focused].GetComponent<Slot>();
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
    public void OpenInventory(bool _isDungeon)       // I 키로 인벤토리 열 때
    {
        isUIOn = true;
        isDungeonOpen = _isDungeon;
        focused = 0;
        slotInstance = slot[focused].GetComponent<Slot>();
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
        Sprite[] keyItemBorderSprite = SpriteSet.keyItemBorderSprite;

        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] != null)
            {
                slot[i].GetComponent<Slot>().SetItemSprite(itemList[i].sprite, keyItemBorderSprite[itemList[i].itemRarity], true);
                isFull[i] = true;
            }
            else
            {
                slot[i].GetComponent<Slot>().SetItemSprite(null, keyItemBorderSprite[4], false);
                isFull[i] = false;
            }
        }
        for(int i = availableSlot; i < 24; ++i)
        {
            slot[i].GetComponent<Slot>().SetOverSlot(keyItemBorderSprite[0]);
        }
    }
    public void CloseInventory()
    {
        slotInstance.SetActiveFocus(false);
        itemInformation.SetActive(false);
        isShopOpen = false;
        isUIOn = false;
    }

    public void SetSelectedItem(int _value, int[] _selectedSlot)
    {
        seletedItemCount = _value;
        int count = 0;

        for (int i = 0; i < availableSlot; ++i)
        {
            itemList[i] = storage.GetSelectStorageItem(_selectedSlot[count]);
            isFull[i] = true;
            ++count;

            if (_selectedSlot[count] == 99) break;
            if (count > seletedItemCount) break;
        }
        for (int i = seletedItemCount; i < availableSlot; ++i)
        {
            itemList[i] = null;
            isFull[i] = false;
        }
    }

    // 키 삭제 관련
    public void PutInBox(bool isDead)           // 던전에서 복귀할 때 창고에 키 넣기
    {
        if (isDead)
        {
            for(int i = availableSlot / 2; i < availableSlot; ++i)
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
    }             // 창고에 아이템 넣을시 정리
    public void DeleteItem(int _focused)        // 사용된 아이템 인벤토리에서 제거
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
        InventorySet();
    }
    public void DeleteStorageItem()             // 던전 진입할 때 들고있는 키 창고에서 삭제
    {
        storage.DeleteStorageSlotItem();
        seletedItemCount = 0;
    }

    #region get, set, save, load
    public int GetSelectedItemCount()
    {
        return seletedItemCount;
    }       // 선택된 아이템 수 반환
    public int GetAvailableSlot()
    {
        return availableSlot;
    }           // 사용 가능한 슬롯 수 반환
    public Item[] GetItemList()
    {
        return itemList;
    }             // 인벤토리 아이템 리스트 반환
    public Item GetItem(int _focus)
    {
        return itemList[_focus];
    }

    public void LoadInventoryData(int _availableSlot)
    {
        availableSlot = _availableSlot;
    }
    #endregion
    
    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
        for (int i = availableSlot; i < slotCount; ++i)
        {
            isFull[i] = true;
        }
    }       // 창고 크기 확장
    public bool GetKeyItem(Item _Item)        // 아이템 획득시 인벤토리 등록
    {
        for (int i = 0; i < availableSlot; i++)
        {
            if (!isFull[i])
            {
                itemList[i] = _Item;
                isFull[i] = true;
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
        slotInstance = slot[focused].GetComponent<Slot>();
        slotInstance.SetActiveFocus(true);
    }
}
