using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_QuickSlot : MonoBehaviour
{
    public GameObject player;
    public MainUI_InGameMenu menu;
    public Menu_Inventory inventory;
    public GameObject slots;
    public GameObject[] quickSlot;
    public Key[] inventoryKeylist;
    public bool onQuickSlot;
    public int addQuickInventory;

    public int focus;

    private void Awake()
    {
        menu = GameObject.Find("UI/Menus").GetComponent<MainUI_InGameMenu>();
        inventory = GameObject.Find("UI/Menus/Inventory").GetComponent<Menu_Inventory>();
        onQuickSlot = false;
    }

    public void Update()
    {
        if (menu.InventoryOn || menu.cancelOn || menu.storageOn) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!onQuickSlot)
            {
                onQuickSlot = true;
                focus = 0;
                addQuickInventory = 0;
                slots.SetActive(true);
                quickSlot[0].transform.GetChild(0).gameObject.SetActive(true);
                SetQuickSlot();
            }
            else if (onQuickSlot)
            {
                onQuickSlot = false;
                quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);
                slots.SetActive(false);
            }
        }

        transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.1f);

        if (!onQuickSlot) return;
        
        if (Input.GetKeyDown(KeyCode.W)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(DungeonManager.instance.useTeleportSystem == 8)
            {

            }
            inventory.quickSlotUseItem(focus);
        }
    }

    public void SetQuickSlot()
    {
        inventoryKeylist = inventory.inventoryKeylist;
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue < 0 || focus + AdjustValue > inventoryKeylist.Length) { return; }

        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(false);

        if (focus + AdjustValue > addQuickInventory + 4)
            ++addQuickInventory;
        else if (focus + AdjustValue < addQuickInventory)
            --addQuickInventory;

        focus += AdjustValue;
        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(true);
    }
}