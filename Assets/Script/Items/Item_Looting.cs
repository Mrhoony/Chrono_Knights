using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Looting : MonoBehaviour
{
    Menu_Inventory inventory;
    Database_ItemList itemDatabase;
    int itemListCount;
    Item item;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        itemDatabase = Database_ItemList.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        itemListCount = itemDatabase.keyItem.Count;
        
        item = itemDatabase.keyItem[Random.Range(0, itemListCount)];
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
