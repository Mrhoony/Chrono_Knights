using UnityEngine;

public class Menu_Storage : Menu_InGameMenu
{
    public Menu_Inventory inventory;

    public GameObject cursorStorageSelect;
    public float cursorSpd;

    public bool upgradeItem;               // 아이템 강화 사용할 때

    // 창고 슬롯
    public int boxFull;
    public int boxNum;              // 창고 번호 (*24)

    // 한 슬롯 변수
    public int[] storageItemCodeList;
    public bool[] isSelected;       // 슬롯이 선택 되었는지
    public int selectedItemCount;       // 선택된 슬롯 갯수
    public int[] selectedSlot;      // 선택된 슬롯 번호 목록
    
    public int takeItemCount;

    public override void Init()
    {
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;
        
        slot = new GameObject[72];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
        }

        itemList = new Item[72];
        storageItemCodeList = new int[72];
        isSelected = new bool[72];
        SaveStorageClear();

        isItemSelect = false;
        isUIOn = false;
    }
    public void Update()
    {
        if (canvasManager.isInventoryOn || canvasManager.isCancelOn) return;
        if (!isUIOn) return;

        if (isItemSelect)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { slotInstance.ItemConfirmFocus(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { slotInstance.ItemConfirmFocus(-1); }

            if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
            {
                if(slotInstance.GetFocus() == 0)
                {
                    if (upgradeItem)
                    {
                        slotInstance.SetDisActiveItemConfirm();
                        cursorStorageSelect.SetActive(false);
                        upgradeItem = false;
                        isItemSelect = false;
                        CloseStorageWithUpgrade(true);
                    }
                    else
                    {
                        if (!isSelected[focused])                   // 선택되지 않은 상태면
                        {
                            ++selectedItemCount;
                            if (selectedItemCount > takeItemCount)
                            {
                                selectedItemCount = takeItemCount;
                                Debug.Log("아이템이 꽉 찼습니다.");
                                return;
                            }
                            slotInstance.SetDisActiveItemConfirm();
                            isItemSelect = false;
                            isSelected[focused] = slotInstance.SetItemConfirm(isSelected[focused]);
                        }
                        else                                        // 선택된 상태면
                        {
                            --selectedItemCount;
                            if (selectedItemCount < 0)
                            {
                                selectedItemCount = 0;
                                Debug.Log("꺼낸 아이템이 없습니다.");
                                return;
                            }
                            slotInstance.SetDisActiveItemConfirm();
                            isItemSelect = false;
                            isSelected[focused] = slotInstance.SetItemConfirm(isSelected[focused]);
                        }
                    }
                }
                else
                {
                    isItemSelect = false;
                    slotInstance.SetDisActiveItemConfirm();
                }
            }

            if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
            {
                isItemSelect = false;
                slotInstance.SetDisActiveItemConfirm();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

            if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
            {
                if (itemList[focused] == null) return;

                isItemSelect = true;
                if (upgradeItem)
                {
                    slotInstance.SetActiveItemConfirm("사용", "취소");
                }
                else
                {
                    slotInstance.SetActiveItemConfirm("꺼내기", "취소");
                }
            }

            if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
            {
                if (upgradeItem)
                {
                    upgradeItem = false;
                    cursorStorageSelect.SetActive(false);
                    CloseStorageWithUpgrade(false);
                }
                else
                {
                    cursorStorageSelect.SetActive(false);
                    CloseStorage();
                }
            }
        }

        FocusMove();
    }

    public void StorageSet()           // 창고 활성화시 UI 초기화
    {
        if (availableSlot - (boxNum * 24) > 24) boxFull = 24;
        else                                    boxFull = availableSlot - (boxNum * 24);

        for (int i = boxNum * 24; i < boxFull + (boxNum * 24); ++i)
        {
            if (itemList[i] != null)
            {
                slot[i - (boxNum * 24)].GetComponent<Menu_InGameSlot>().SetItemSprite(itemList[i], isSelected[i]);
            }
            else
            {
                slot[i - (boxNum * 24)].GetComponent<Menu_InGameSlot>().SetItemSprite(null, isSelected[i]);
            }
        }
        for(int i = boxFull; i < 24; ++i)
        {
            slot[i].GetComponent<Menu_InGameSlot>().SetOverSlot();
        }
    }
    public int PutInBox(Item _Item, bool _IsBuy)        // 창고에 아이템 넣기
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] == null)
            {
                itemList[i] = _Item;
                if (_IsBuy)
                {
                    ++selectedItemCount;
                    isSelected[i] = true;
                    return i;
                }
                else
                {
                    isSelected[i] = false;
                    return 99;
                }
            }
        }
        return 99;
    }
    public void OpenStorage(bool _UpgradeEquipment)       // 창고를 열었을 때
    {
        isUIOn = true;
        upgradeItem = _UpgradeEquipment;
        boxNum = 0;
        boxFull = 0;
        focused = 0;
        selectedItemCount = inventory.GetSelectedItemCount();
        takeItemCount = inventory.GetAvailableSlot();
        selectedSlot = new int[takeItemCount];

        StorageSet();
        ItemInformationSetting(focused);
        slotInstance = slot[focused].GetComponent<Slot>();
        cursorStorageSelect.transform.position = slot[focused].transform.position;
        cursorStorageSelect.SetActive(true);
    }
    public void CloseStorageWithUpgrade(bool _SelectedItem)
    {
        itemInformation.SetActive(false);
        cursorStorageSelect.SetActive(false);
        isUIOn = false;

        if (_SelectedItem)
        {
            canvasManager.CloseUpgradeStorage(focused);
        }
        else
        {
            canvasManager.CloseUpgradeStorage();
        }
    }
    public void CloseStorage()
    {
        SetSelectedItemSlotNum();
        inventory.SetSelectedItem(selectedItemCount, selectedSlot);

        itemInformation.SetActive(false);
        cursorStorageSelect.SetActive(false);
        isUIOn = false;

        canvasManager.CloseStorage();
    }
    public void SetSelectedItemSlotNum()                // 선택된 아이템 슬롯 번호를 저장
    {
        selectedItemCount = 0;
        for (int i = 0; i < availableSlot; ++i)
        {
            if (isSelected[i])
            {
                selectedSlot[selectedItemCount] = i;
                ++selectedItemCount;
            }
        }
        for (int i = selectedItemCount; i < selectedSlot.Length; ++i)
        {
            selectedSlot[i] = 99;
        }
    }
    
    public void DeleteItem(int _focused)                // 아이템 삭제될 시
    {
        itemList[_focused] = null;
        if (isSelected[_focused])
        {
            int count = selectedSlot.Length;
            for(int i = 0; i < count; ++i)
            {
                if (selectedSlot[i] == _focused)
                    inventory.DeleteItem(i);
            }
            --selectedItemCount;
            if (selectedItemCount < 0)
            {
                selectedItemCount = 0;
                return;
            }
            isSelected[_focused] = false;
        }
        StorageSlotSort(_focused);
    }

    public void InventorySelectCancel(int _focused)       // 인벤토리에서 아이템 선택 취소시
    {
        --selectedItemCount;
        if (selectedItemCount < 0)
        {
            selectedItemCount = 0;
            return;
        }
        isSelected[_focused] = false;
        StorageSlotSort(_focused);
    }
    public void StorageSlotSort(int _focus)             // 창고 정렬
    {
        for (int i = _focus; i < availableSlot - 1; ++i)
        {
            if (itemList[i] != null) continue;

            for (int j = 1; j < availableSlot - i; ++j)
            {
                if (i + j == availableSlot)
                {
                    i = availableSlot - 1;
                    break;
                }

                if (itemList[i + j] != null)
                {
                    itemList[i] = itemList[i + j];
                    isSelected[i] = isSelected[i + j];

                    itemList[i + j] = null;
                    isSelected[i + j] = false;

                    break;
                }
            }
        }
        StorageSet();
    }
    
    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
    }

    public Item GetSelectStorageItem(int _focus)
    {
        return itemList[_focus];
    }
    public Item GetStorageItem(int _focus)
    {
        return itemList[_focus];
    }
    
    public void LoadStorageData(int[] _itemCode, int _BagRarity)
    {
        for (int i = 0; i < 72; ++i)
        {
            itemList[i] = null;
            storageItemCodeList[i] = 0;
            isSelected[i] = false;
        }

        storageItemCodeList = _itemCode;
        SetAvailableSlot(_BagRarity);

        for (int i = 0; i < availableSlot; ++i)
        {
            if (Database_Game.instance.GetItem(storageItemCodeList[i]) == null) continue;
            itemList[i] = Database_Game.instance.GetItem(storageItemCodeList[i]);
        }
    }
    public int[] GetStorageItemCodeList()
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] == null)
            {
                storageItemCodeList[i] = 0;
                continue;
            }
            storageItemCodeList[i] = itemList[i].itemCode;
        }
        return storageItemCodeList;
    }
    public void SaveStorageClear()
    {
        for (int i = 0; i < 72; ++i)
        {
            itemList[i] = null;
            storageItemCodeList[i] = 0;
            isSelected[i] = false;
        }
        selectedItemCount = 0;
    }
    public void SetAvailableSlot(int _BagRarity)
    {
        int _availableSlot = 12 * (3 + _BagRarity);

        for (int i = _availableSlot; i < 72; ++i)
        {
            if (itemList[i] != null)
            {
                itemList[i] = null;
                storageItemCodeList[i] = 0;
                isSelected[i] = false;
            }
        }

        availableSlot = _availableSlot;
    }

    public void ItemInformationSetting(int _focus)
    {
        if (itemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(itemList[focused]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
    }

    public void FocusMove()
    {
        cursorStorageSelect.transform.position = Vector2.Lerp(cursorStorageSelect.transform.position, slot[focused - (boxNum * 24)].transform.position, Time.deltaTime * cursorSpd);
    }
    public override void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue > availableSlot - 1 || focused + AdjustValue < 0) return;
        
        if (focused + AdjustValue < boxNum * 24)
        {
            --boxNum;
            if (boxNum < 0)
            {
                boxNum = 0;
                return;
            }
            StorageSet();
        }
        else if (focused + AdjustValue > (boxNum + 1) * 24 - 1)
        {
            ++boxNum;
            if (boxNum > 3)
            {
                boxNum = 3;
                return;
            }
            StorageSet();
        }
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

        slotInstance = slot[focused - (boxNum * 24)].GetComponent<Slot>();
    }
}
