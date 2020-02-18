using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Looting : MonoBehaviour
{
    Menu_Inventory inventory;
    Database_Game itemDatabase;
    int itemListCount;
    Item item;

    private void Start()
    {
        itemDatabase = Database_Game.instance;
        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        itemListCount = itemDatabase.Item.Count;
        
        item = itemDatabase.Item[Random.Range(0, itemListCount)];
        GetComponent<Image>().sprite = item.sprite;

        int[] skillList = itemDatabase.GetSkillRarityList(item.itemRarity);

        for(int i = 0; i < skillList.Length; ++i)
        {
            Debug.Log(skillList[i]);
        }

        item.SetSkillNum(skillList[Random.Range(0, skillList.Length)]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (inventory.GetKeyItem(item)) gameObject.SetActive(false);
        }
    }
}
