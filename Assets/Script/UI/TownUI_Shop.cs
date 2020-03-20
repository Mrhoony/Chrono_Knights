using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_Shop : MonoBehaviour
{
    public TownUI townUI;
    public CanvasManager canvasManager;
    public Menu_Inventory inventory;
    public GameObject[] slot;
    public Slot slotInstance;
    public bool isTownMenuOn = false;
    public Item[] shopItemList = new Item[8];
    
    public int focused;

    public int slotCount;

    public void Update()
    {
        if (!isTownMenuOn) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(4); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-4); }

        if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    public void OpenTownUIMenu(Menu_Inventory _inventory)
    {
        isTownMenuOn = true;
        inventory = _inventory;
        Transform[] transforms = transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;

        ShopItemListSet();
        shopItemList = DungeonManager.instance.GetShopItemList();

        slot = new GameObject[8];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            if(shopItemList[i - 1] != null)
                slot[i - 1].GetComponent<TownUI_ShopSlot>().SetItemSprite(shopItemList[i], true);
            else
                slot[i - 1].GetComponent<TownUI_ShopSlot>().SetItemSprite(null, false);
        }
        slotInstance = slot[0].GetComponent<TownUI_ShopSlot>();
    }
    public void ShopItemListSet()
    {
        if (!DungeonManager.instance.NewDayCheckShop()) return;

        int randomItemRarity;
        int itemListCount = Database_Game.instance.Item.Count;
        List<Item> itemRarity1List = new List<Item>();
        List<Item> itemRarity2List = new List<Item>();
        List<Item> itemRarity3List = new List<Item>();

        for (int i = 0; i < itemListCount; ++i)
        {
            switch (Database_Game.instance.Item[i].itemRarity)
            {
                case 1:
                    itemRarity1List.Add(Database_Game.instance.Item[i]);
                    break;
                case 2:
                    itemRarity2List.Add(Database_Game.instance.Item[i]);
                    break;
                case 3:
                    itemRarity3List.Add(Database_Game.instance.Item[i]);
                    break;
            }
        }
        for (int i = 0; i < 8; ++i)
        {
            randomItemRarity = Random.Range(0, 10);
            if (randomItemRarity <= 7)
            {
                shopItemList[i] = itemRarity1List[Random.Range(0, itemRarity1List.Count)];
            }
            else if (randomItemRarity > 7 && randomItemRarity <= 8)
            {
                shopItemList[i] = itemRarity2List[Random.Range(0, itemRarity2List.Count)];
            }
            else
            {
                shopItemList[i] = itemRarity3List[Random.Range(0, itemRarity3List.Count)];
            }
        }

        DungeonManager.instance.SetShopItemList(shopItemList);
    }
    public void CloseTownUIMenu()
    {
        DungeonManager.instance.SetShopItemList(shopItemList);
        isTownMenuOn = false;
        townUI.CloseShopMenu();
    }

    public void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > 8) { return; }
        slotInstance.SetActiveFocus(false);

        focused += AdjustValue;
        
        slotInstance = slot[focused].GetComponent<Slot>();
        slotInstance.SetActiveFocus(true);
    }
}
