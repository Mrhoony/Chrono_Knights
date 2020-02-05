using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Looting : MonoBehaviour
{
    Menu_Inventory inventory;
    Database_ItemList itemDatabase;
    int itemListCount;
    Item item;
    List<Item> itemList = new List<Item>();

    SpriteRenderer spriteRenderer;
    public int rarity;

    private void Start()
    {
        itemDatabase = Database_ItemList.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();

        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        itemListCount = itemDatabase.keyItem.Count;

        for(int i=0; i < itemListCount; ++i)
        {
            if (itemDatabase.keyItem[i].itemRarity == rarity)
                itemList.Add(itemDatabase.keyItem[i]);
        }
        item = itemList[Random.Range(0, itemList.Count)];
        spriteRenderer.sprite = item.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (inventory.GetKeyItem(item)) Destroy(gameObject);
        }
    }
}
