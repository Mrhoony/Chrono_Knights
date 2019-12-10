using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Inventory : MonoBehaviour
{
    // 초기화 영역
    public GameObject player;
    public GameObject slots;
    private Menu_Storage storage;

    private Transform[] transforms;
    private GameObject[] slot;           // 인벤토리 슬롯

    private Sprite[] keyItemBorderSprite;    // 키 레어도 테두리

    private int slotCount;
    private int availableSlot;
    
    public int inventoryItemCount;

    private int seletedItemCount;         // 창고에서 선택된 아이템 수
    private int takeItemSlot;             // 가져갈 수 있는 슬롯 수
    private bool[] isFull;               // 슬롯이 비었는지 아닌지
    private Key[] inventoryItemList;      // 인벤토리 키 목록

    private bool isInventoryOn;
    private int focused = 0;

    private void Awake()
    {
        storage = GameObject.Find("UI/Menus/Storage").GetComponent<Menu_Storage>();
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length-1;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");

        slot = new GameObject[slotCount];
        isFull = new bool[slotCount];
        inventoryItemList = new Key[slotCount];

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
            isFull[i - 1] = false;
        }
        isInventoryOn = false;
        seletedItemCount = 0;
        takeItemSlot = 3;
        availableSlot = 6;
        inventoryItemCount = 0;
    }
    private void Update()
    {
        if (!isInventoryOn) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 키 위치 변경
        }
    }
    public bool GetKeyItem(Key _key)        // 아이템 획득시 인벤토리 등록
    {
        for (int i = 0; i < availableSlot; i++)
        {
            if (!isFull[i])
            {
                isFull[i] = true;
                inventoryItemList[i] = _key;
                ++inventoryItemCount;

                return true;
            }
        }
        return false;
    }

    public void OpenInventory()
    {
        isInventoryOn = true;
        InventorySet();

        focused = 0;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void InventorySet()           // 인벤토리 활성화시 아이템 세팅
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (inventoryItemList[i] != null)
            {
                slot[i].GetComponent<Image>().sprite = inventoryItemList[i].sprite;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[11 - inventoryItemList[i].keyRarity];
                isFull[i] = true;
            }
            else
            {
                slot[i].GetComponent<Image>().sprite = null;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
                isFull[i] = false;
            }
        }
        for(int i = availableSlot; i < 24; ++i)
        {
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[7];
        }
    }
    public void CloseInventory()
    {
        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetInventoryItemList()
    {
        for(int i = 0; i < seletedItemCount; ++i)
        {
            inventoryItemList[i] = storage.GetStorageItem(i);
            isFull[i] = true;
            ++inventoryItemCount;
        }
        for(int i = seletedItemCount; i < availableSlot; ++i)
        {
            inventoryItemList[i] = null;
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
                inventoryItemList[i] = null;
            }
            storage.GetComponent<Menu_Storage>().PutInBox(inventoryItemList);
        }
        else
        {
            storage.GetComponent<Menu_Storage>().PutInBox(inventoryItemList);
        }
        DeleteInventorySlotItem();
    }
    public void DeleteInventorySlotItem()       // 던전에서 복귀할 때 인벤토리 비우기
    {
        for(int i = 0; i < availableSlot; ++i)
        {
            inventoryItemList[i] = null;
            isFull[i] = false;
        }
        seletedItemCount = 0;
        inventoryItemCount = 0;
    }

    public void QuickSlotUseItem(int focus)
    {
        if (DungeonManager.instance.useKeyInDungeon(inventoryItemList[focus]))
        {
            inventoryItemList[focus] = null;
            Debug.Log("Item use");
            QuickSlotDeleteItem(focus);
        }
    }
    public void QuickSlotDeleteItem(int focus)
    {
        for (int i = focus; i < availableSlot - 1; ++i)
        {
            if (inventoryItemList[i] != null) continue;

            for (int j = 1; j < availableSlot - i; ++i)
            {
                if (inventoryItemList[i + j] != null)
                {
                    inventoryItemList[i] = inventoryItemList[i + j];
                    isFull[i] = true;

                    if (i + j == availableSlot)
                    {
                        inventoryItemList[i + j] = null;
                        isFull[i + j] = false;
                    }
                    break;
                }
            }
        }
        --inventoryItemCount;
    }
    
    public void DeleteStorageItem()             // 던전 진입할 때 들고있는 키 창고에서 삭제
    {
        for (int i = 0; i < seletedItemCount; ++i)
        {
            inventoryItemList[i] = storage.GetSelectStorageItem(i);
            isFull[i] = true;
        }
        for (int i = seletedItemCount; i < availableSlot; ++i)    // 창고에서 꺼낸 키 외에는 빈슬롯
        {
            inventoryItemList[i] = null;
            isFull[i] = false;
        }
        storage.DeleteStorageSlotItem();
        seletedItemCount = 0;
    }

    #region get, set, save, load
    public void SetSelectedItemCount(int value)
    {
        seletedItemCount = value;
    }
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
    public Key[] GetInventoryItemList()
    {
        return inventoryItemList;
    }

    public void LoadInventoryData(int _takeItemSlot, int _availableSlot)
    {
        takeItemSlot = _takeItemSlot;
        availableSlot = _availableSlot;
    }
    public void SaveInventoryData(DataBase db)
    {
        db.SaveInventoryData(takeItemSlot, availableSlot);
    }

    #endregion


    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot-1) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
