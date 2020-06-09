using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_Shop : FocusUI
{
    public TownUI townUI;
    public readonly CanvasManager canvasManager = CanvasManager.instance;
    public Menu_Inventory inventory;
    public GameObject[] slot;
    public TownUI_ShopSlot slotInstance;
    
    public Item[] shopItemList = new Item[8];
    public int[] itemCost = new int[8];
    
    public bool isItemSelect;

    public void Awake()
    {
        slot = new GameObject[8];
        Transform[] transforms = transform.GetComponentsInChildren<Transform>();
        int slotCount = transforms.Length - 2;

        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
        }
    }

    public void Update()
    {
        if (!isUIOn || canvasManager.DialogBoxOn()) return;

        if (isItemSelect)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { slotInstance.ItemConfirmFocus(1); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { slotInstance.ItemConfirmFocus(-1); }

            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if(slotInstance.GetFocus() == 0)
                {
                    int shopInformation = inventory.BuyItem(shopItemList[focused], itemCost[focused]);

                    switch (shopInformation)
                    {
                        case 0:
                            Debug.Log("구매 완료");
                            BuyItem();
                            slotInstance.SetDisActiveItemConfirm();
                            isItemSelect = false;
                            break;
                        case 1:
                            Debug.Log("돈이 부족합니다.");
                            break;
                        case 2:
                            Debug.Log("인벤토리가 꽉찼습니다.");
                            break;
                    }
                }
                else
                {
                    slotInstance.SetDisActiveItemConfirm();
                    isItemSelect = false;
                }
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
            {
                slotInstance.SetDisActiveItemConfirm();
                isItemSelect = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { FocusedSlot(1); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { FocusedSlot(4); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { FocusedSlot(-4); }

            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                if (shopItemList[focused] == null) return;
                slotInstance.SetActiveItemConfirm("구매", "취소");
                isItemSelect = true;
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
            {
                canvasManager.CloseShopInventory();
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Jump"]))
            {
                StartCoroutine(FocusChange());
            }
            FocusMove(slot[focused]);
        }
    }

    IEnumerator FocusChange()
    {
        isUIOn = false;
        cursor.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        inventory.FocusOn();
        Debug.Log("Focus : inventory");
    }
    public void FocusOn()
    {
        focused = 0;
        slotInstance = slot[focused].GetComponent<TownUI_ShopSlot>();
        cursor.SetActive(true);
        isUIOn = true;
    }
    public void BuyItem()
    {
        shopItemList[focused] = null;
        slotInstance.SetItemSprite(null, 0);
        ShopItemDisplay();
    }

    public void OpenTownUIMenu(Menu_Inventory _inventory)
    {
        isUIOn = true;
        isItemSelect = false;
        inventory = _inventory;
        focused = 0;

        ShopItemListSet();
        shopItemList = DungeonManager.instance.GetShopItemList();
        itemCost = DungeonManager.instance.GetShopitemCostList();

        ShopItemDisplay();

        slotInstance = slot[0].GetComponent<TownUI_ShopSlot>();
        cursor.SetActive(true);
    }
    public void ShopItemDisplay()
    {
        int count = slot.Length;
        for (int i = 0; i < count; ++i)
        {
            if (shopItemList[i] != null)
                slot[i].GetComponent<TownUI_ShopSlot>().SetItemSprite(shopItemList[i], itemCost[i]);
            else
                slot[i].GetComponent<TownUI_ShopSlot>().SetItemSprite(null, 0);
        }
    }
    public void ShopItemListSet()
    {
        if (!DungeonManager.instance.GetShopRefill()) return;
        DungeonManager.instance.setShopRefill(false);

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
                itemCost[i] = 1;
            }
            else if (randomItemRarity > 7 && randomItemRarity <= 8)
            {
                shopItemList[i] = itemRarity2List[Random.Range(0, itemRarity2List.Count)];
                itemCost[i] = Random.Range(1, 3);
            }
            else
            {
                shopItemList[i] = itemRarity3List[Random.Range(0, itemRarity3List.Count)];
                itemCost[i] = Random.Range(4, 6);
            }
        }

        DungeonManager.instance.SetShopItemList(shopItemList, itemCost);
    }
    public void CloseTownUIMenu()
    {
        isUIOn = false;
        DungeonManager.instance.SetShopItemList(shopItemList, itemCost);
        cursor.SetActive(false);
    }
    public new void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > 7) { return; }

        focused += AdjustValue;
        
        slotInstance = slot[focused].GetComponent<TownUI_ShopSlot>();
    }
}
