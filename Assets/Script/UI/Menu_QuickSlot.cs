using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_QuickSlot : MonoBehaviour
{
    public static Menu_QuickSlot instance;

    public GameObject player;
    public CanvasManager menu;
    public Menu_Inventory inventory;
    public GameObject slots;
    public GameObject[] quickSlot;
    public Item[] inventoryItemlist;
    public bool onQuickSlot;
    public int addQuickInventory;

    public Sprite[] quickSlotImage;
    public GameObject quickSlotItemInfomation;

    public int focus;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        quickSlotImage = Resources.LoadAll<Sprite>("Graphic/UI/ui_quickSlot");
        onQuickSlot = false;
    }

    public void Update()
    {
        if (menu.GameMenuOnCheck()) return;

        slots.transform.position = new Vector3(Camera.main.WorldToScreenPoint(player.transform.position).x, Camera.main.WorldToScreenPoint(player.transform.position).y + 75f, 0f);
        quickSlotItemInfomation.transform.position = quickSlot[focus - addQuickInventory].transform.position;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!onQuickSlot)
            {
                onQuickSlot = true;
                focus = 0;
                addQuickInventory = 0;
                slots.SetActive(true);
                inventoryItemlist = inventory.GetItemList();
                SetQuickSlot(0, 5);
                quickSlot[focus].transform.GetChild(0).gameObject.SetActive(true);
                if(inventoryItemlist[focus] != null)
                {
                    quickSlotItemInfomation.SetActive(true);
                    quickSlotItemInfomation.transform.position = quickSlot[focus].transform.position;
                    quickSlotItemInfomation.GetComponent<ItemInfomation>().SetItemInformationQuickSlot(inventoryItemlist[0]);
                }
            }
            else if (onQuickSlot)
            {
                onQuickSlot = false;
                quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);
                quickSlotItemInfomation.SetActive(false);
                slots.SetActive(false);
            }
        }
        
        if (!onQuickSlot) return;
        
        if (Input.GetKeyDown(KeyCode.W)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (inventory.GetItem(focus) == null) return;
            inventory.UseItemInQuickSlot(focus);
            quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);
            quickSlotItemInfomation.SetActive(false);
            onQuickSlot = false;
            slots.SetActive(false);
        }
    }
    
    public void SetQuickSlot(int low, int high)
    {
        for(int i = low; i < high; ++i)
        {
            if (inventoryItemlist[i] != null)
            {
                quickSlot[i - low].SetActive(true);
                quickSlot[i - low].GetComponent<Image>().sprite = inventoryItemlist[i].sprite;
            }
            else
            {
                quickSlot[i - low].SetActive(false);
            }
        }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue < 0 || focus + AdjustValue > inventory.GetAvailableSlot() - 1) return;
        if (inventoryItemlist[focus + AdjustValue] == null) return;

        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);

        if (focus + AdjustValue > addQuickInventory + 4)
            ++addQuickInventory;
        else if (focus + AdjustValue < addQuickInventory)
            --addQuickInventory;

        SetQuickSlot(addQuickInventory, addQuickInventory + 5);

        focus += AdjustValue;
        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(true);

        if (inventoryItemlist[focus] != null)
        {
            quickSlotItemInfomation.SetActive(true);
            quickSlotItemInfomation.transform.position = quickSlot[focus - addQuickInventory].transform.position;
            quickSlotItemInfomation.GetComponent<ItemInfomation>().SetItemInformationQuickSlot(inventoryItemlist[focus]);
        }
        else
        {
            quickSlotItemInfomation.SetActive(false);
        }
    }
}