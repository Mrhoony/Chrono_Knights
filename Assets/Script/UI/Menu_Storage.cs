using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Storage : MonoBehaviour
{
    public MainUI_InGameMenu menu;
    public Menu_Inventory inventory;
    public GameObject slots;
    public GameObject[] slot;

    private Sprite[] keyItemBorderSprite;    // 키 레어도 테두리

    private Transform[] transforms;
    private int slotCount;
    private bool onStorage;
    private bool upgradeItem;               // 아이템 강화 사용할 때

    // 창고 슬롯
    private int availableSlot;       // 사용 가능한 슬롯 수
    private int boxFull;
    private int boxNum;              // 창고 번호 (*24)

    // 한 슬롯 변수
    private int focus;
    private Key[] storageKeyList;
    private bool[] isFull;
    public bool[] isSelected;       // 선택된 슬롯
    public int selectSlotNum;         // 선택된 슬롯 번호
    public int[] selectedSlot;      // 선택된 슬롯 번호 목록

    public int selectedItemCount;
    public int takeItemCount;
    
    private void Awake()
    {
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");

        transforms = slots.transform.GetComponentsInChildren<Transform>();
        menu = transform.parent.GetComponent<MainUI_InGameMenu>();
        inventory = menu.Menus[0].GetComponent<Menu_Inventory>();
        slotCount = transforms.Length - 1;

        slot = new GameObject[slotCount];
        isFull = new bool[72];
        isSelected = new bool[72];

        onStorage = false;

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
        }

        for (int i = 0; i < 72; ++i)
        {
            isFull[i] = false;
        }

        selectedSlot = new int[inventory.GetTakeItemSlot()];
    }

    public void Update()
    {
        if (menu.InventoryOn || menu.cancelOn) return;
        
        if (!onStorage) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
        {
            if (upgradeItem)
            {
                if (storageKeyList[focus] != null)
                {
                    upgradeItem = false;
                    CloseStorageWithUpgrade(true);
                }
            }
            else
            {
                if (storageKeyList[focus] == null) return;
                
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

    public void StorageSet()           // 창고 활성화시 초기화
    {
        if (availableSlot - (boxNum * 24) > 24)
        {
            boxFull = 24;
        }
        else
        {
            boxFull = availableSlot - (boxNum * 24);
        }

        for (int i = boxNum * 24; i < boxFull; ++i)
        {
            if (storageKeyList[i] != null)
            {
                isFull[i] = true;
                slot[i - (boxNum * 24)].GetComponent<Image>().sprite = storageKeyList[i].sprite;
                slot[i - (boxNum * 24)].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[11 - storageKeyList[i].keyRarity];
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
    public void PutInBox(Key[] key)
    {
        int i;
        for (i = 0; i < availableSlot; ++i)
        {
            if (storageKeyList[i] == null)
                break;
        }
        for(int j = 0; j < key.Length; ++j)
        {
            if(i < availableSlot)
            {
                storageKeyList[i] = key[j];
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
    }
    public void CloseStorageWithUpgrade(bool isSelect)
    {
        onStorage = false;
        upgradeItem = false;

        if (isSelect)
        {
            slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.GetComponent<MainUI_InGameMenu>().CloseUpgradeStorage(focus);
        }
        else
        {
            gameObject.SetActive(false);
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
        transform.parent.GetComponent<MainUI_InGameMenu>().CloseStorage();
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

            storageKeyList[selectedSlot[i]] = null;
            isFull[selectedSlot[i]] = false;
            isSelected[selectedSlot[i]] = false;
            slot[selectedSlot[i]].transform.GetChild(2).gameObject.SetActive(false);
            selectedSlot[i] = 99;
        }
        StorageSlotSort(0);
    }

    public Key GetSelectStorageItem(int _focus)
    {
        return storageKeyList[selectedSlot[_focus]];
    }
    public Key GetStorageItem(int _focus)
    {
        return storageKeyList[_focus];
    }
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 아이템 창고에서 제거
    {
        storageKeyList[_focus] = null;
        int count = selectedSlot.Length;
        
        if (isSelected[_focus])      // 인벤토리 선택된 아이템 제거
        {
            for (int i = 0; i < count; ++i)
            {
                if (selectedSlot[i] != _focus) continue;

                if (i + 1 == count) selectedSlot[i + 1] = 99;
                else selectedSlot[i] = selectedSlot[i + 1] - 1;
            }
            --selectedItemCount;
        }
        StorageSlotSort(_focus);
        inventory.SetSelectedItemCount(selectedItemCount);
        inventory.SetInventoryItemList();
    }
    public void StorageSlotSort(int _focus)
    {
        for (int i = _focus; i < availableSlot-1; ++i)
        {
            for (int j = 1; j < availableSlot - i; ++j)
            {
                if (storageKeyList[i] != null) break;

                if (storageKeyList[i + j] != null)
                {
                    storageKeyList[i] = storageKeyList[i + j];
                    isFull[i] = isFull[i + j];
                    isSelected[i] = isSelected[i + j];
                    slot[i].transform.GetChild(2).gameObject.SetActive(slot[i + j].transform.GetChild(2).gameObject.activeInHierarchy);
                    
                    if (i + j != availableSlot)
                    {
                        storageKeyList[i + j] = null;
                        isFull[i + j] = false;
                        isSelected[i + j] = false;
                        slot[i + j].transform.GetChild(2).gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
    }
    
    public void SetStorageData(Key[] _key, int _availableSlot)
    {
        storageKeyList = _key;
        // 키 입력 받아서 창고 초기화 함수 추가
        availableSlot = _availableSlot;
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
        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(true);
    }
}
