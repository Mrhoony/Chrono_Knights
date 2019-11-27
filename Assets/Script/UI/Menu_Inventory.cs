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

    public GameObject[] slot;       // 인벤토리 슬롯
    public int slotCount;
    public bool[] isFull;           // 슬롯이 비었는지 아닌지
    public Key[] inventoryKeylist;  // 인벤토리 키 목록
    
    bool useItem;                   // 아이템 사용시
    public int focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length-1;
        useItem = false;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        
        inventoryKeylist = new Key[slotCount];
        slot = new GameObject[slotCount];

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            slot[i - 1].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!useItem)
            {
                slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.parent.GetComponent<MainUI_Menu>().CloseInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))    // 아이템 선택
        {
            if (useItem)
            {
                if (inventoryKeylist[focused] != null)
                {
                    slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                    useItem = false;
                    gameObject.transform.parent.GetComponent<MainUI_Menu>().CloseInventory(focused);
                }
            }
            else                            // 아이템 버프로 사용시
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.X))    // 아이템 선택 취소
        {
            if (useItem)
            {
                slot[focused].transform.GetChild(0).gameObject.SetActive(false);
                useItem = false;
                gameObject.transform.parent.GetComponent<MainUI_Menu>().CloseInventory();
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
    public void InventorySet()
    {
        for (int i = 0; i < slotCount; ++i)
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
    }           // 인벤토리 활성화시 아이템 세팅

    public void OpenInventory()
    {
        focused = 0;
        InventorySet();
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenUpgradeInventory()     // 인챈트, 업그레이드 시 체크
    {
        useItem = true;
        focused = 0;
        InventorySet();
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void EnchantedKey(int _focus)        // 인챈트, 업그레이드 성공시 키 아이템 인벤에서 제거
    {
        inventoryKeylist[_focus] = null;
        isFull[_focus] = false;
        slot[_focus].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
        slot[_focus].GetComponent<Image>().sprite = null;
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > 23) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
