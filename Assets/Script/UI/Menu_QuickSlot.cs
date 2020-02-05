using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        quickSlotImage = Resources.LoadAll<Sprite>("UI/ui_quickSlot");
        onQuickSlot = false;
    }

    public void Update()
    {
        if (menu.GameMenuOnCheck()) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!onQuickSlot)
            {
                onQuickSlot = true;
                focus = 0;
                addQuickInventory = 0;
                slots.SetActive(true);
                quickSlot[focus].transform.GetChild(0).gameObject.SetActive(true);
                SetQuickSlot(0, 5);
            }
            else if (onQuickSlot)
            {
                onQuickSlot = false;
                quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);
                slots.SetActive(false);
            }
        }

        transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);

        if (!onQuickSlot) return;
        
        if (Input.GetKeyDown(KeyCode.W)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(DungeonManager.instance.useTeleportSystem == 8)
            {
                inventory.QuickSlotUseKey(focus);
            }
            else
            {
                inventory.QuickSlotUseItem(focus);
            }
            SetQuickSlot(0, 5);
        }
    }

    public void SetQuickSlot(int low, int high)
    {
        inventoryItemlist = inventory.GetInventoryItemList();
        for(int i = low; i < high; ++i)
        {
            if (inventoryItemlist[i] != null)
            {
                quickSlot[i].GetComponent<SpriteRenderer>().sprite = inventoryItemlist[i].sprite;
            }
            else
            {
                quickSlot[i].GetComponent<SpriteRenderer>().sprite = quickSlotImage[1];
            }
        }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue < 0 || focus + AdjustValue > inventory.GetAvailableSlot()) { return; }

        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);

        if (focus + AdjustValue > addQuickInventory + 4)
            ++addQuickInventory;
        else if (focus + AdjustValue < addQuickInventory)
            --addQuickInventory;
        SetQuickSlot(addQuickInventory, addQuickInventory + 5);
        focus += AdjustValue;
        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(true);
    }
}