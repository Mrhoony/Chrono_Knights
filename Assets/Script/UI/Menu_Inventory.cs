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

    public GameObject[] slot;       // 인벤토리 슬롯
    public int slotCount;
    public int availableSlot;
    public int takeKeySlot;
    public bool[] isFull;           // 슬롯이 비었는지 아닌지
    public Key[] inventoryKeylist;  // 인벤토리 키 목록

    public int[] selectedStorageKey;
    
    bool upgradeItem;               // 아이템 강화 사용할 때
    bool takeItem;               // 창고에서 꺼낼 때
    public int focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length-1;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        inventoryKeylist = new Key[slotCount];

        slot = new GameObject[slotCount];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        upgradeItem = false;
        takeItem = false;
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
            }else if(takeItem)
            {
                slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                takeItem = false;
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
    public void TakeKeySlotUpgrade(int upgrade)
    {
        takeKeySlot += upgrade;
    }
    public void AvailableKeySlotUpgrade(int upgrade)
    {
        availableSlot += upgrade;
    }

    public void AvailableKeySlotSetting(int slotNum)
    {
        for (int i = slotNum-1; i < slotCount; ++i)
        {
            isFull[i] = true;
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[7];
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
        AvailableKeySlotSetting(availableSlot);
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
    public void OpenTakeKeyInventory()
    {
        takeItem = true;
        OpenInventory();
    }
    
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 키 아이템 인벤에서 제거
    {
        inventoryKeylist[_focus] = null;
        isFull[_focus] = false;
        slot[_focus].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        slot[_focus].GetComponent<Image>().sprite = null;
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

    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > availableSlot) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
