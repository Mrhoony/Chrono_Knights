using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Storage : MonoBehaviour
{
    public Menu_Inventory inventory;

    public Sprite[] keyItemBorderSprite;    // 키 레어도 테두리
    public Transform[] transforms;
    public GameObject slots;
    public GameObject[] slot;
    public int slotCount;
    public int focus;
    public int availableSlot;       // 사용 가능한 슬롯 수
    public bool onStorage;

    public Key[] storageKeyList;
    public bool[] isFull;

    public int boxFull;
    public int boxNum;              // 창고 번호 (*24)

    public int selectedKey;         // 선택 된 아이템
    public int selectCount;         // 선택된 아이템
    public bool[] isSelected;       // 선택된 슬롯
    public int[] selectedSlot;      // 선택된 슬롯 번호

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");

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
    }

    public void Update()
    {
        if (!onStorage) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isFull[focus - (boxNum * 24)]) return;

            if (!isSelected[focus - (boxNum * 24)])
            {
                ++selectedKey;
                if (selectedKey > selectCount)
                {
                    selectedKey = selectCount;
                    return;
                }
                isSelected[focus] = true;
            }
            else
            {
                --selectCount;
                if (selectedKey < 0)
                {
                    selectedKey = 0;
                    return;
                }
                isSelected[focus] = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseStorage();
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
                slot[i - (boxNum * 24)].GetComponent<Image>().sprite = storageKeyList[i].sprite;
                slot[i - (boxNum * 24)].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[11 - storageKeyList[i].keyRarity];
            }
            else
            {
                slot[i - (boxNum * 24)].GetComponent<Image>().sprite = null;
                slot[i - (boxNum * 24)].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[6];
            }
        }
        for(int i = boxFull; i < 24; ++i)
        {
            slot[i].transform.GetChild(1).GetComponent<Image>().sprite = keyItemBorderSprite[7];
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
    
    public void OpenStorage(GameObject _inventory)
    {
        boxNum = 0;
        focus = 0;
        onStorage = true;
        inventory = _inventory.GetComponent<Menu_Inventory>();
        selectedKey = inventory.seletedKey;
        selectCount = inventory.takeKeySlot;

        selectedSlot = new int[selectCount];
        StorageSet();
        slot[focus].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void CloseStorage()
    {
        onStorage = false;
        inventory.seletedKey = selectedKey;
        SetSelectedItemSlotNum();
        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);
        focus = 0;
        inventory.SetStorageLinkedItem(selectedSlot);
        transform.parent.GetComponent<MainUI_InGameMenu>().CloseStorage();
    }

    public void SetSelectedItemSlotNum()
    {
        int j = 0;
        for (int i = 0; i < availableSlot; ++i)
        {
            if (isSelected[i])
            {
                selectedSlot[j] = i;
                ++j;
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

        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(false);
        focus += AdjustValue;
        slot[focus - (boxNum * 24)].transform.GetChild(0).gameObject.SetActive(true);
    }
}
