using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Storage : MonoBehaviour
{
    public CanvasManager canvasManager;
    public Menu_Inventory inventory;
    public GameObject slots;
    public GameObject[] slot;
    public GameObject itemInformation;

    public Sprite[] keyItemBorderSprite;    // 키 레어도 테두리

    public Transform[] transforms;
    public int slotCount;
    public bool onStorage;
    public bool upgradeItem;               // 아이템 강화 사용할 때

    // 창고 슬롯
    public int availableSlot;       // 사용 가능한 슬롯 수
    public int boxFull;
    public int boxNum;              // 창고 번호 (*24)

    // 한 슬롯 변수
    public int focus;
    public Item[] storageItemList;
    public int[] storageItemCodeList;
    public int[] storageItemSkillCodeList;
    public bool[] isFull;
    public bool[] isSelected;       // 선택된 슬롯
    public int selectSlotNum;         // 선택된 슬롯 번호
    public int[] selectedSlot;      // 선택된 슬롯 번호 목록

    public int selectedItemCount;
    public int takeItemCount;
    
    public void Init()
    {
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;
        
        slot = new GameObject[72];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
        }

        storageItemList = new Item[72];
        storageItemCodeList = new int[72];
        storageItemSkillCodeList = new int[72];
        isFull = new bool[72];
        isSelected = new bool[72];
        SaveStorageClear();

        onStorage = false;
    }

    public void Update()
    {
        if (canvasManager.isInventoryOn || canvasManager.isCancelOn) return;
        if (!onStorage) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
        {
            if (upgradeItem)
            {
                if (storageItemList[focus] != null)
                {
                    upgradeItem = false;
                    CloseStorageWithUpgrade(true);
                }
            }
            else
            {
                if (storageItemList[focus] == null) return;
                
                if (!isSelected[focus])
                {
                    ++selectedItemCount;
                    if (selectedItemCount > takeItemCount)
                    {
                        selectedItemCount = takeItemCount;
                        return;
                    }
                    isSelected[focus] = true;
                }
                else
                {
                    --selectedItemCount;
                    if (selectedItemCount < 0)
                    {
                        selectedItemCount = 0;
                        return;
                    }
                    isSelected[focus] = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
        {
            if (upgradeItem)
            {
                slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);
                upgradeItem = false;
                CloseStorageWithUpgrade(false);
            }
            else
            {
                CloseStorage();
            }
        }
    }

    public void StorageSet()           // 창고 활성화시 UI 초기화
    {
        if (availableSlot - (boxNum * 24) > 24)
        {
            boxFull = 24;
        }
        else
        {
            boxFull = availableSlot - (boxNum * 24);
        }

        for (int i = boxNum * 24; i < boxFull + (boxNum * 24); ++i)
        {
            if (storageItemList[i] != null)
            {
                isFull[i] = true;
                slot[i - (boxNum * 24)].GetComponent<Image>().sprite = storageItemList[i].sprite;
                slot[i - (boxNum * 24)].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[11 - storageItemList[i].itemRarity];
                if (isSelected[i])
                    slot[i - (boxNum * 24)].transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                isFull[i] = false;
                slot[i - (boxNum * 24)].GetComponent<Image>().sprite = null;
                slot[i - (boxNum * 24)].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
                slot[i - (boxNum * 24)].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        for(int i = boxFull; i < 24; ++i)
        {
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[7];
        }
    }
    public void PutInBox(Item[] item)
    {
        Debug.Log("putInBoxStorage");
        int i;
        for (i = 0; i < availableSlot; ++i)
        {
            if (storageItemList[i] == null) break;
        }
        for (int j = 0; j < item.Length; ++j)
        {
            if (item[j] == null) continue;

            if (i < availableSlot)
            {
                storageItemList[i] = item[j];
                ++i;
            }
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

    public void OpenStorageWithUpgrade()                 // 강화에서 창고를 열었을 때
    {
        onStorage = true;
        upgradeItem = true;
        boxNum = 0;
        focus = 0;
        
        StorageSet();
        slot[focus].transform.GetChild(0).gameObject.SetActive(true);
        if (storageItemList[focus] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(storageItemList[focus]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
    }
    public void CloseStorageWithUpgrade(bool isSelect)
    {
        onStorage = false;
        upgradeItem = false;

        itemInformation.SetActive(false);
        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);

        if (isSelect)
        {
            canvasManager.CloseUpgradeStorage(focus);
        }
        else
        {
            canvasManager.CloseUpgradeStorage();
        }
    }

    public void OpenStorage()       // 일반적으로 창고를 열었을 때
    {
        boxNum = 0;
        focus = 0;
        onStorage = true;
        selectedItemCount = inventory.GetSelectedItemCount();
        takeItemCount = inventory.GetTakeItemSlot();
        selectedSlot = new int[inventory.GetTakeItemSlot()];
        StorageSet();

        if (storageItemList[focus] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(storageItemList[focus]);
        }
        else
        {
            itemInformation.SetActive(false);
        }

        slot[focus].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void CloseStorage()      
    {
        onStorage = false;
        SetSelectedItemSlotNum();
        inventory.SetSelectedItemCount(selectedItemCount);
        inventory.SetInventoryItemList();
        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);
        focus = 0;
        itemInformation.SetActive(false);
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

    public void DeleteStorageSlotItem()     // 던전 입장시 인벤토리 설정한 키 창고에서 제거
    {
        int count = selectedSlot.Length;

        for(int i = 0; i < count; ++i)
        {
            if (selectedSlot[i] > availableSlot) return;

            storageItemList[selectedSlot[i]] = null;
            isFull[selectedSlot[i]] = false;
            isSelected[selectedSlot[i]] = false;
            slot[selectedSlot[i]].transform.GetChild(2).gameObject.SetActive(false);
            selectedSlot[i] = 99;
        }
        StorageSlotSort(0);
    }

    public Item GetSelectStorageItem(int _focus)
    {
        return storageItemList[selectedSlot[_focus]];
    }
    public Item GetStorageItem(int _focus)
    {
        return storageItemList[_focus];
    }
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 아이템 창고에서 제거
    {
        storageItemList[_focus] = null;
        int count = selectedSlot.Length;
        
        if (isSelected[_focus])      // 인벤토리 선택 취소
        {
            for (int i = 0; i < count; ++i)
            {
                if (selectedSlot[i] != _focus) continue;

                if(i == count-1)
                {
                    selectedSlot[i] = 99;
                }
                else
                {
                    selectedSlot[i] = selectedSlot[i + 1] - 1;
                }
            }
            --selectedItemCount;
            inventory.SetSelectedItemCount(selectedItemCount);
            inventory.SetInventoryItemList();
        }
        StorageSlotSort(_focus);
    }

    public void StorageSlotSort(int _focus)
    {
        for (int i = _focus; i < availableSlot - 1; ++i)
        {
            for (int j = 1; j < availableSlot - i; ++j)
            {
                if (storageItemList[i] != null) break;

                if (storageItemList[i+j] != null)
                {
                    storageItemList[i] = storageItemList[i + j];
                    isFull[i] = isFull[i + j];
                    isSelected[i] = isSelected[i + j];
                    slot[i].transform.GetChild(2).gameObject.SetActive(true);
                    
                    if (i + j != availableSlot)
                    {
                        storageItemList[i + j] = null;
                        isFull[i + j] = false;
                        isSelected[i + j] = false;
                        slot[i + j].transform.GetChild(2).gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
    }
    
    public void LoadStorageData(int[] _itemCode, int[] _itemSkillCodeList, int _availableSlot)
    {
        for (int i = 0; i < 72; ++i)
        {
            storageItemList[i] = null;
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
            storageItemList[i] = Database_Game.instance.GetItem(storageItemCodeList[i]);
        }
    }
    public int[] GetStorageItemCodeList()
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (storageItemList[i] == null)
            {
                storageItemCodeList[i] = 0;
                continue;
            }
            storageItemCodeList[i] = storageItemList[i].itemCode;
        }
        return storageItemCodeList;
    }
    public int[] GetStorageItemSkillCodeList()
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (storageItemList[i] == null)
            {
                storageItemSkillCodeList[i] = 0;
                continue;
            }
            storageItemSkillCodeList[i] = storageItemList[i].skillCode;
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
            storageItemList[i] = null;
            storageItemCodeList[i] = 0;
            storageItemSkillCodeList[i] = 0;
            isFull[i] = false;
            isSelected[i] = false;
        }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue > availableSlot - 1) return;
        if (focus + AdjustValue < 0) return;

        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);

        if (focus + AdjustValue < boxNum * 24)
        {
            --boxNum;
            if (boxNum < 0)
            {
                boxNum = 0;
                return;
            }
            StorageSet();
        }
        else if (focus + AdjustValue > (boxNum + 1) * 24 - 1)
        {
            ++boxNum;
            if (boxNum > 3)
            {
                boxNum = 3;
                return;
            }
            StorageSet();
        }
        focus += AdjustValue;

        if (storageItemList[focus] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(storageItemList[focus]);
        }
        else
        {
            itemInformation.SetActive(false);
        }

        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(true);
    }
}
