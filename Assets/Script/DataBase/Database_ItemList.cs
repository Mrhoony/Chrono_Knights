using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database_ItemList : MonoBehaviour
{
    public static Database_ItemList instance;

    public List<Item> keyItem = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InputKeyItem();
    }

    void InputKeyItem()
    {                     // 이름, 효과, 등급, 아이템 코드, 장비화 이름, 
        keyItem.Add(new Item("커먼", 1, 7, "", ItemType.Number, ItemUsingType.health, 20));
        keyItem.Add(new Item("커먼", 1, 7, "", ItemType.ReturnTown, ItemUsingType.attack, 2));
        keyItem.Add(new Item("커먼", 1, 7, "", ItemType.RepeatThisFloor, ItemUsingType.defense, 1));
        keyItem.Add(new Item("매직", 2, 8, "", ItemType.Number, ItemUsingType.moveSpeed, 1));
        keyItem.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
        keyItem.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
        keyItem.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
    }

    public Item GetItem(int _itemCode)
    {
        for (int i = 0; i < keyItem.Count; i++)
        {
            if (keyItem[i].itemCode == _itemCode)
            {
                return keyItem[i];
            }
        }
        return null;
    }
}
