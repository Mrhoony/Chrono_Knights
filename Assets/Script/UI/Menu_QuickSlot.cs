using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_QuickSlot : MonoBehaviour
{
    public GameObject player;
    public CanvasManager menu;
    public Menu_Inventory inventory;
    public GameObject quickSlot;
    public GameObject[] quickSlots;
    public Item[] inventoryItemlist;
    public bool onQuickSlot = false;
    public int addQuickInventory;

    public IEnumerator quickSlotOpenTime;

    public GameObject quickSlotItemInfomation;

    public int focus;

    public void Update()
    {
        if (menu.GameMenuOnCheck() || menu.TownUIOnCheck()) return;

        if (onQuickSlot)
        {
            if (Input.GetKeyDown(KeyCode.Q)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(1); }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (inventory.GetItem(focus) == null) return;

                inventory.UseItemInQuickSlot(focus);
                CloseQuickSlot();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                OpenQuickSlot();
            }
        }
    }

    public void OpenQuickSlot()
    {
        onQuickSlot = true;
        focus = 0;
        addQuickInventory = 0;
        quickSlot.SetActive(true);
        inventoryItemlist = inventory.GetItemList();
        SetQuickSlot(0, 5);
        ItemInformationSet(focus);
        quickSlotOpenTime = QuickSlotOpenTime();
        StartCoroutine(quickSlotOpenTime);
        Debug.Log("Open");
    }

    public void CloseQuickSlot()
    {
        StopCoroutine(quickSlotOpenTime);
        quickSlots[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);
        quickSlotItemInfomation.SetActive(false);
        quickSlot.SetActive(false);
        onQuickSlot = false;
        Debug.Log("Close");
    }

    public IEnumerator QuickSlotOpenTime()
    {
        yield return new WaitForSeconds(3f);
        CloseQuickSlot();
    }
    
    public void SetQuickSlot(int low, int high)
    {
        for(int i = low; i < high; ++i)
        {
            if (inventoryItemlist[i] != null)
            {
                quickSlots[i - low].SetActive(true);
                quickSlots[i - low].GetComponent<Image>().sprite = inventoryItemlist[i].sprite;
            }
            else
            {
                quickSlots[i - low].SetActive(false);
            }
        }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue < 0 || focus + AdjustValue > inventory.GetAvailableSlot() - 1) return;

        quickSlots[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);

        if (focus + AdjustValue > addQuickInventory + 4)
            ++addQuickInventory;
        else if (focus + AdjustValue < addQuickInventory)
            --addQuickInventory;

        focus += AdjustValue;

        SetQuickSlot(addQuickInventory, addQuickInventory + 5);
        ItemInformationSet(focus - addQuickInventory);

        StopCoroutine(quickSlotOpenTime);
        quickSlotOpenTime = QuickSlotOpenTime();
        StartCoroutine(quickSlotOpenTime);
    }

    public void ItemInformationSet(int _Focused)
    {
        if (inventoryItemlist[focus] != null)
        {
            quickSlots[_Focused].transform.GetChild(0).gameObject.SetActive(true);
            quickSlotItemInfomation.SetActive(true);
            quickSlotItemInfomation.transform.position = quickSlots[_Focused].transform.position;
            quickSlotItemInfomation.GetComponent<ItemInfomation>().SetItemInformationQuickSlot(inventoryItemlist[focus]);
        }
        else
        {
            quickSlots[_Focused].transform.GetChild(0).gameObject.SetActive(false);
            quickSlotItemInfomation.SetActive(false);
        }
    }
}