using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Inventory : MonoBehaviour
{
    // 초기화 영역
    public GameObject player;
    public GameObject slots;
    public Transform[] transforms;
    public Sprite[] keyItemBorderSprite;    // 키 레어도 테두리
    public Menu_Storage storage;

    public GameObject[] slot;           // 인벤토리 슬롯
    public int slotCount;

    public int availableSlot;

    public int seletedKeyCount;         // 창고에서 선택된 아이템 수
    public int takeKeySlot;             // 가져갈 수 있는 슬롯 수
    public bool[] isFull;               // 슬롯이 비었는지 아닌지
    public Key[] inventoryKeylist;      // 인벤토리 키 목록

    public int[] selectedStorageKey;
    public bool onInventory;
    
    public int focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length-1;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        storage = GameObject.Find("UI/Menus/Storage").GetComponent<Menu_Storage>();

        slot = new GameObject[slotCount];
        isFull = new bool[slotCount];
        inventoryKeylist = new Key[slotCount];

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
            isFull[i - 1] = false;
        }
        onInventory = false;
        seletedKeyCount = 0;
        takeKeySlot = 3;
        availableSlot = 6;
        selectedStorageKey = new int[takeKeySlot];
    }
    private void Update()
    {
        if (!onInventory) return;

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
                inventoryKeylist[i] = _key;

                return true;
            }
        }
        return false;
    }

    public void OpenInventory()
    {
        onInventory = true;
        int itemCount = 0;
        while(inventoryKeylist[itemCount] != null)
        {
            ++itemCount;
        }
        Debug.Log(itemCount);

        if (seletedKeyCount > 0)            // 창고에 링크된 키 등록
        {
            for(int i = 0; i < seletedKeyCount; ++i)
            {
                inventoryKeylist[itemCount] = storage.storageKeyList[storage.selectedSlot[i]];
                isFull[itemCount] = true;
                ++itemCount;
            }
        }
        for(int i = itemCount + seletedKeyCount; i < availableSlot; ++i)    // 창고에서 꺼낸 키 외에는 빈슬롯
        {
            inventoryKeylist[i] = null;
            isFull[i] = false;
        }
        InventorySet();

        focused = 0;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void InventorySet()           // 인벤토리 활성화시 아이템 세팅
    {
        for (int i = 0; i < availableSlot; ++i)
        {
            if (inventoryKeylist[i] != null)
            {
                slot[i].GetComponent<Image>().sprite = inventoryKeylist[i].sprite;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[11 - inventoryKeylist[i].keyRarity];
            }
            else
            {
                slot[i].GetComponent<Image>().sprite = null;
                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
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
    
    public void TakeKeySlotUpgrade(int upgrade)
    {
        takeKeySlot += upgrade;
        selectedStorageKey = new int[takeKeySlot];
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
            // 죽었을 때 일정 수 만큼 저장
        }
        else
        {
            storage.GetComponent<Menu_Storage>().PutInBox(inventoryKeylist);
        }
        DeleteInventorySlotItem();
    }
    public void DeleteInventorySlotItem()       // 던전에서 복귀할 때 인벤토리 비우기
    {
        for(int i = 0; i < availableSlot; ++i)
        {
            inventoryKeylist[i] = null;
            slot[i].GetComponent<Image>().sprite = null;
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
            isFull[i] = false;
            seletedKeyCount = 0;
        }
    }
    public void DeleteStorageItem()             // 던전 진입할 때 들고있는 키 창고에서 삭제
    {
        for (int i = 0; i < seletedKeyCount; ++i)
        {
            inventoryKeylist[i] = storage.storageKeyList[selectedStorageKey[i]];
            isFull[i] = true;
        }
        for (int i = seletedKeyCount; i < availableSlot; ++i)    // 창고에서 꺼낸 키 외에는 빈슬롯
        {
            inventoryKeylist[i] = null;
            isFull[i] = false;
        }
        storage.DeleteStorageSlotItem();
        selectedStorageKey = new int[takeKeySlot];
        seletedKeyCount = 0;
    }

    public void quickSlotUseItem(int focus)
    {
        for (int i = 0; i < availableSlot - 1; ++i)
        {
            for (int j = 1; j < availableSlot - i; ++i)
            {
                if (inventoryKeylist[i] != null) continue;
                if (inventoryKeylist[i + j] != null)
                {
                    inventoryKeylist[i] = inventoryKeylist[i + j];
                    isFull[i] = true;

                    if(i + j == availableSlot)
                    {
                        inventoryKeylist[i + j] = null;
                        isFull[i + j] = false;
                    }
                    break;
                }
            }
        }
    }

    public void SetInventoryData(int _takeKeySlot, int _availableSlot)
    {
        takeKeySlot = _takeKeySlot;
        availableSlot = _availableSlot;
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot-1) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
