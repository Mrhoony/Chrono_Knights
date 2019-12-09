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
    public Key[] inventoryKeylist;
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
        inventory = GameObject.Find("UI/Menus/Inventory").GetComponent<Menu_Inventory>();
        quickSlotImage = Resources.LoadAll<Sprite>("UI/ui_quickSlot");
        onQuickSlot = false;
    }

    public void Update()
    {
        if (menu.isInventoryOn || menu.isCancelOn || menu.isStorageOn) return;

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

        transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);

        if (!onQuickSlot) return;
        
        if (Input.GetKeyDown(KeyCode.W)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(DungeonManager.instance.useTeleportSystem == 8)
            {

            }
            else
            {
                inventory.quickSlotUseItem(focus);
                SetQuickSlot();
            }
        }
    }

    public void SetQuickSlot()
    {
        inventoryKeylist = inventory.GetInventoryItemList();
        int i = 0;
        while(inventoryKeylist[i] != null)
        {
            if (inventoryKeylist[i] != null)
            {
                quickSlot[i].GetComponent<SpriteRenderer>().sprite = inventoryKeylist[i].sprite;
            }
            else
            {
                quickSlot[i].GetComponent<SpriteRenderer>().sprite = quickSlotImage[1];
            }
            ++i;
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

        focus += AdjustValue;
        quickSlot[focus - addQuickInventory].transform.GetChild(0).gameObject.SetActive(true);
    }
}