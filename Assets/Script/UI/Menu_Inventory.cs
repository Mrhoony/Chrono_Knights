using UnityEngine;
using UnityEngine.UI;

public class Menu_Inventory : MonoBehaviour
{
    // 초기화 영역
    public CanvasManager canvasManager;
    public GameObject slots;
    public Menu_Storage storage;
    public GameObject itemInformation;
    private GameObject[] slot;           // 인벤토리 슬롯

    public int slotCount;
    public int availableSlot;
    public int inventoryItemCount;

    public int seletedItemCount;         // 창고에서 선택된 아이템 수
    public int takeItemSlot;             // 가져갈 수 있는 슬롯 수
    public Item[] inventoryItemList;      // 인벤토리 키 목록
    public bool[] isFull;               // 슬롯이 비었는지 아닌지

    public bool isInventoryOn;
    public bool isDungeonOpen;
    public int focused = 0;
    
    public void Init()
    {
        canvasManager = CanvasManager.instance;
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;

        slot = new GameObject[24];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
        inventoryItemList = new Item[24];
        isFull = new bool[24];
        for (int i = 0; i < slotCount; ++i)
        {
            inventoryItemList[i] = null;
            isFull[i] = false;
        }
        isInventoryOn = false;
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
            if (isDungeonOpen)
            {
                UseItemInDungeon(focused);
            }
            else
            {
                //DeleteItem(focused);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!isDungeonOpen)
            {
                DeleteItem(focused);
            }
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
    public void UseItemInDungeon(int focus)              // 던전 포탈 앞에서 퀵슬롯으로 아이템 사용시
    {
        if (DungeonManager.instance.useKeyInDungeon(inventoryItemList[focus]))
        {
            DeleteItem(focus);
            canvasManager.CloseInGameMenu();
        }
    }

    public void OpenInventory(bool _inDungeon)
    {
        isInventoryOn = true;
        isDungeonOpen = _inDungeon;
        InventorySet();

        focused = 0;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);

        if (inventoryItemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(inventoryItemList[0]);
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
            if (inventoryItemList[i] != null)
            {
                slot[i].GetComponent<Image>().sprite = inventoryItemList[i].sprite;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[inventoryItemList[i].itemRarity];
                isFull[i] = true;
            }
            else
            {
                slot[i].GetComponent<Image>().sprite = null;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[4];
                isFull[i] = false;
            }
        }
        for(int i = availableSlot; i < 24; ++i)
        {
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[0];
        }
    }
    public void CloseInventory()
    {
        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        itemInformation.SetActive(false);
        isInventoryOn = false;
    }

    public void SetSelectedItemCount(int value)
    {
        seletedItemCount = value;
    }
    public void SetInventoryItemList()
    {
        for(int i = 0; i < seletedItemCount; ++i)
        {
            inventoryItemList[i] = storage.GetSelectStorageItem(i);
            isFull[i] = true;
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
        Debug.Log("putInBoxInventory");
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
        InventoryClear();
    }
    public void InventoryClear()
    {
        for (int i = 0; i < 24; ++i)
        {
            inventoryItemList[i] = null;
            isFull[i] = false;
        }
        isInventoryOn = false;
        inventoryItemCount = 0;
    }
    
    public void UseIteminQuickSlot(int focus)             // 퀵슬롯으로 아이템 사용시 ( 마을에서 사용시 창고도비우기 or 마을에서 사용 x )
    {
        DeleteItem(focus);
    }
    public void DeleteItem(int focus)          // 사용된 아이템 인벤토리에서 제거
    {
        inventoryItemList[focus] = null;
        isFull[focus] = false;

        for (int i = focus; i < availableSlot - 1; ++i)
        {
            if (inventoryItemList[i] != null) continue;

            for (int j = 1; j < availableSlot - i; ++i)
            {
                if (inventoryItemList[i + j] != null)
                {
                    inventoryItemList[i] = inventoryItemList[i + j];
                    isFull[i] = true;

                    inventoryItemList[i + j] = null;
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
                        inventoryItemList[i] = null;
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
    public Item[] GetInventoryItemList()
    {
        return inventoryItemList;
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
                inventoryItemList[i] = _Item;
                isFull[i] = true;
                ++inventoryItemCount;
                return true;
            }
        }
        return false;
    }
    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot-1) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;

        if (inventoryItemList[focused] != null)
        {
            itemInformation.SetActive(true);
            itemInformation.GetComponent<ItemInfomation>().SetItemInventoryInformation(inventoryItemList[focused]);
        }
        else
        {
            itemInformation.SetActive(false);
        }
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
