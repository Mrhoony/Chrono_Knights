using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Looting : MonoBehaviour
{
    Menu_Inventory inventory;
    Item_Database itemDatabase;
    int itemListCount;
    Key key;
    List<Key> keyList = new List<Key>();

    SpriteRenderer spriteRenderer;
    public int rarity;

    private void Start()
    {
        itemDatabase = Item_Database.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();

        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        itemListCount = itemDatabase.keyItem.Count;

        for(int i=0; i < itemListCount; ++i)
        {
            if (itemDatabase.keyItem[i].keyRarity == rarity)
                keyList.Add(itemDatabase.keyItem[i]);
        }
        key = keyList[Random.Range(0, keyList.Count)];
        spriteRenderer.sprite = key.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (inventory.GetKeyItem(key)) Destroy(gameObject);
        }
    }
}
