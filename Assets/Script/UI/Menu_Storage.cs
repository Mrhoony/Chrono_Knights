using UnityEngine;

public class Menu_Storage : Menu_InGameMenu
{
    public Menu_Inventory inventory;
    
    public bool upgradeItem;               // 아이템 강화 사용할 때

    // 창고 슬롯
    public int boxFull;
    public int boxNum;              // 창고 번호 (*24)

    // 한 슬롯 변수
    public int[] storageItemCodeList;
    public int[] storageItemSkillCodeList;
    public bool[] isSelected;       // 슬롯이 선택 되었는지
    public int selectSlotNum;         // 선택된 슬롯 번호
    public int[] selectedSlot;      // 선택된 슬롯 번호 목록
    
    public int selectedItemCount;
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
        storageItemSkillCodeList = new int[72];
        isFull = new bool[72];
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
                        slotInstance.SetActiveFocus(false);
                        upgradeItem = false;
                        CloseStorageWithUpgrade(true);
                    }

                    if (!isSelected[focused])                   // 선택되지 않은 상태면
                    {
                        ++selectedItemCount;
                        if (selectedItemCount > takeItemCount)
                        {
                            selectedItemCount = takeItemCount;
                            Debug.Log("아이템이 꽉 찼습니다.");
                            return;
                        }
                        isSelected[focused] = slotInstance.SetItemConfirm(isSelected[focused]);
                    }
                    else                                        // 선택된 상태면
                    {
                        --selectedItemCount;
                        if (selectedItemCount < 0)
                        {
                            selectedItemCount = 0;
                            Debug.Log("-1 개를 선택할 수 없습니다.");
                            return;
                        }
                        isSelected[focused] = slotInstance.SetItemConfirm(isSelected[focused]);
                    }
                }
                else
                {
                    slotInstance.SetDisActiveItemConfirm();
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
                    slotInstance.SetActiveFocus(false);
                    upgradeItem = false;
                    CloseStorageWithUpgrade(false);
                }
                else
                {
                    CloseStorage();
                }
            }
        }
    }

    public void StorageSet()           // 창고 활성화시 UI 초기화
    {
        Sprite[] keyItemBorderSprite = SpriteSet.keyItemBorderSprite;

        if (availableSlot - (boxNum * 24) > 24) boxFull = 24;
        else                                    boxFull = availableSlot - (boxNum * 24);

        for (int i = boxNum * 24; i < boxFull + (boxNum * 24); ++i)
        {
            if (itemList[i] != null)
            {
                isFull[i] = true;
                slot[i - (boxNum * 24)].GetComponent<Menu_InGameSlot>().SetItemSprite(itemList[i], isSelected[i]);
            }
            else
            {
                isFull[i] = false;
                slot[i - (boxNum * 24)].GetComponent<Menu_InGameSlot>().SetItemSprite(null, isSelected[i]);
            }
        }
        for(int i = boxFull; i < 24; ++i)
        {
            slot[i].GetComponent<Menu_InGameSlot>().SetOverSlot(keyItemBorderSprite[0]);
        }
    }
    public void PutInBox(Item[] item)
    {
        int i , count = item.Length;
        for (i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] == null) break;
        }
        for (int j = 0; j < count; ++j)
        {
            if (item[j] == null) continue;

            if (i < availableSlot)
            {
                itemList[i] = item[j];
                ++i;
            }
            else break;
        }
    }

    public void OpenStorageWithUpgrade()                 // 강화에서 창고를 열었을 때
    {
        isUIOn = true;
        upgradeItem = true;
        boxNum = 0;
        boxFull = 0;
        focused = 0;
        StorageSet();
        ItemInformationSetting(focused);

        slotInstance = slot[focused].GetComponent<Slot>();
        slotInstance.SetActiveFocus(true);
    }
    public void CloseStorageWithUpgrade(bool isSelect)
    {
        itemInformation.SetActive(false);
        slotInstance.SetActiveFocus(false);
        isUIOn = false;

        if (isSelect)
        {
            canvasManager.CloseUpgradeStorage(focused);
        }
        else
        {
            canvasManager.CloseUpgradeStorage();
        }
    }

    public void OpenStorage()       // 일반적으로 창고를 열었을 때
    {
        isUIOn = true;
        boxNum = 0;
        boxFull = 0;
        focused = 0;
        selectedItemCount = inventory.GetSelectedItemCount();
        takeItemCount = inventory.GetAvailableSlot();
        selectedSlot = new int[takeItemCount];

        StorageSet();
        ItemInformationSetting(focused);

        slotInstance = slot[focused].GetComponent<Slot>();
        slotInstance.SetActiveFocus(true);
    }
    public void CloseStorage()      
    {
        SetSelectedItemSlotNum();
        if(selectedItemCount > 0)
            inventory.SetSelectedItem(selectedItemCount, selectedSlot);
        slotInstance.SetActiveFocus(true);
        focused = 0;
        itemInformation.SetActive(false);
        isUIOn = false;

        canvasManager.CloseStorage();
    }
    public void SetSelectedItemSlotNum()    // 선택된 아이템 슬롯 번호를 저장
    {
        selectSlotNum = 0;
        for (int i = 0; i < availableSlot; ++i)
        {
            if (isSelected[i])
            {
                selectedSlot[selectSlotNum] = i;
                ++selectSlotNum;
            }
        }
        for(int i = selectSlotNum; i < selectedSlot.Length; ++i)
        {
            selectedSlot[i] = 99;
        }
    }

    public void DeleteItem()
    {
        if (isSelected[focused])
        {
            --selectedItemCount;
            if (selectedItemCount < 0)
            {
                selectedItemCount = 0;
                return;
            }
            isSelected[focused] = slotInstance.SetItemConfirm(isSelected[focused]);
        }
        itemList[focused] = null;
        isFull[focused] = false;

        slotInstance.SetActiveFocus(false);
        
        StorageSlotSort(0);
    }
    public void DeleteStorageSlotItem()     // 던전 입장시 인벤토리 설정한 키 창고에서 제거
    {
        int count = selectedSlot.Length;

        for(int i = 0; i < count; ++i)
        {
            if (selectedSlot[i] > availableSlot) break;

            itemList[selectedSlot[i]] = null;
            isFull[selectedSlot[i]] = false;
            isSelected[selectedSlot[i]] = false;
            selectedSlot[i] = 99;
        }
        StorageSlotSort(0);
    }
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 아이템 창고에서 제거
    {
        itemList[_focus] = null;
        isFull[_focus] = false;
        StorageSlotSort(_focus);
    }
    public void StorageSlotSort(int _focus)
    {
        for (int i = _focus; i < availableSlot - 1; ++i)
        {
            for (int j = 1; j < availableSlot - i; ++j)
            {
                if (itemList[i] != null) break;

                if (itemList[i + j] != null)
                {
                    itemList[i] = itemList[i + j];
                    isFull[i] = isFull[i + j];
                    isSelected[i] = isSelected[i + j];

                    if (i + j != availableSlot)
                    {
                        itemList[i + j] = null;
                        isFull[i + j] = false;
                        isSelected[i + j] = false;
                    }
                    break;
                }
            }
        }
        StorageSet();
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

    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
        AvailableKeySlotSetting(availableSlot);
    }
    public void AvailableKeySlotSetting(int slotNum)
    {
        for (int i = slotNum - 1; i < 72; ++i)
        {
            isFull[i] = true;
        }
    }

    public Item GetSelectStorageItem(int _focus)
    {
        return itemList[_focus];
    }
    public Item GetStorageItem(int _focus)
    {
        return itemList[_focus];
    }
    
    public void LoadStorageData(int[] _itemCode, int[] _itemSkillCodeList, int _availableSlot)
    {
        for (int i = 0; i < 72; ++i)
        {
            itemList[i] = null;
            storageItemCodeList[i] = 0;
            storageItemSkillCodeList[i] = 0;
            isFull[i] = false;
            isSelected[i] = false;
        }

        storageItemCodeList = _itemCode;
        storageItemSkillCodeList = _itemSkillCodeList;
        availableSlot = _availableSlot;

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
    public int[] GetStorageItemSkillCodeList()
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (itemList[i] == null)
            {
                storageItemSkillCodeList[i] = 0;
                continue;
            }
            storageItemSkillCodeList[i] = itemList[i].skillCode;
        }
        return storageItemSkillCodeList;
    }
    public int GetStorageAvailableSlot()
    {
        return availableSlot;
    }
    public void SaveStorageClear()
    {
        for (int i = 0; i < 72; ++i)
        {
            itemList[i] = null;
            storageItemCodeList[i] = 0;
            storageItemSkillCodeList[i] = 0;
            isFull[i] = false;
            isSelected[i] = false;
        }
    }

    public override void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue > availableSlot - 1 || focused + AdjustValue < 0) return;

        slotInstance.SetActiveFocus(false);

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
        slotInstance.SetActiveFocus(true);
    }
}
