using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Inventory : MonoBehaviour
{
    // 초기화 영역
    GameObject _Player;
    public GameObject slots;        
    public Transform[] transforms;
    public Sprite[] keyItemBorderSprite;    // 키 레어도 테두리
    public GameObject storage;

    public GameObject[] slot;           // 인벤토리 슬롯
    public int slotCount;
    public int availableSlot;
    public int seletedKey;              // 창고에서 선택된 아이템
    public int takeKeySlot;             // 가져갈 수 있는 슬롯 수
    public bool[] isFull;               // 슬롯이 비었는지 아닌지
    public Key[] inventoryKeylist;      // 인벤토리 키 목록

    public int[] selectedStorageKey;    
    
    bool upgradeItem;               // 아이템 강화 사용할 때
    public int focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length-1;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        inventoryKeylist = new Key[slotCount];

        slot = new GameObject[slotCount];
        isFull = new bool[slotCount];

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
            isFull[i - 1] = false;
        }
        upgradeItem = false;
        seletedKey = 0;
        takeKeySlot = 3;
        availableSlot = 6;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!upgradeItem)
            {
                slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.parent.GetComponent<MainUI_InGameMenu>().CloseInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
        {
            if (upgradeItem)
            {
                if (inventoryKeylist[focused] != null)
                {
                    slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                    upgradeItem = false;
                    gameObject.transform.parent.GetComponent<MainUI_InGameMenu>().CloseInventory(focused);
                }
            }
            else                            // 아이템 버프로 사용시
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
        {
            if (upgradeItem)
            {
                slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                upgradeItem = false;
                gameObject.transform.parent.GetComponent<MainUI_InGameMenu>().CloseInventory();
            }
        }
    }
    public void GetKeyItem(Key _key)        // 아이템 획득시 인벤토리 등록
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (!isFull[i])
            {
                isFull[i] = true;
                inventoryKeylist[i] = _key;
                break;
            }
        }
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

    public void OpenInventory()
    {
        focused = 0;
        InventorySet();
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenUpgradeInventory()     // 인챈트, 업그레이드 시 체크
    {
        upgradeItem = true;
        OpenInventory();
    }
    
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 키 아이템 인벤에서 제거
    {
        inventoryKeylist[_focus] = null;
        isFull[_focus] = false;
        slot[_focus].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        slot[_focus].GetComponent<Image>().sprite = null;
    }

    public void SetStorageLinkedItem(int[] seletedKeySlot)
    {
        selectedStorageKey = seletedKeySlot;
        for(int i = 0; i < seletedKey; ++i)
        {
        }
    }

    public void TakeKeySlotUpgrade(int upgrade)
    {
        takeKeySlot += upgrade;
    }
    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
        AvailableKeySlotSetting(availableSlot);
    }
    public void AvailableKeySlotSetting(int slotNum)
    {
        for (int i = slotNum; i < slotCount; ++i)
        {
            isFull[i] = true;
        }
    }


    public void PutInBox(bool isDead)
    {
        if (isDead)
        {

        }
        else
        {

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
