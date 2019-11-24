using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 필드 드랍 아이템 프리팹에 스크립트 추가
 * */

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
        inventory = GameObject.Find("UI").transform.Find("Menus").transform.Find("Inventory").GetComponent<Menu_Inventory>();
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
            for (int i=0; i< inventory.slot.Length; i++)
            {
                if (!inventory.isFull[i])
                {
                    inventory.isFull[i] = true;
                    inventory.slot[i].GetComponent<Image>().sprite = spriteRenderer.sprite;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
