using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Looting : MonoBehaviour
{

    Menu_Inventory inventory;
    Database_Game itemDatabase;
    int itemListCount;
    float speed;
    Item item;
    public GameObject eft_itemRoot;

    private void OnEnable()
    {
        itemDatabase = Database_Game.instance;
        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        itemListCount = itemDatabase.Item.Count;
        
        item = itemDatabase.Item[Random.Range(0, itemListCount)];
        speed = 1f;
        GetComponent<SpriteRenderer>().sprite = item.sprite;

        int[] skillList = itemDatabase.GetSkillRarityList(item.itemRarity);

        for(int i = 0; i < skillList.Length; ++i)
        {
            Debug.Log(skillList[i]);
        }

        item.SetSkillNum(skillList[Random.Range(0, skillList.Length)]);
    }

    private void Update()
    {
        speed += Time.deltaTime * 2f;
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(speed) * 0.002f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (inventory.GetKeyItem(item))
            {
                Instantiate(eft_itemRoot, collision.transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
