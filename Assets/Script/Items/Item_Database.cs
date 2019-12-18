using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Database : MonoBehaviour
{
    public static Item_Database instance;

    public List<Key> keyItem = new List<Key>();

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
        keyItem.Add(new Key("커먼", 1, 1, 231, ItemType.Number, ""));
        keyItem.Add(new Key("커먼", 1, 1, 234, ItemType.ReturnTown, ""));
        keyItem.Add(new Key("커먼", 1, 1, 235, ItemType.RepeatThisFloor, ""));
        keyItem.Add(new Key("매직", 2, 2, 232, ItemType.Number, ""));
        keyItem.Add(new Key("유니크", 3, 3, 233, ItemType.Number, ""));
    }

    public Key GetItem(int _keyCode)
    {
        int i = 0;
        for (i = 0; i < keyItem.Count; i++)
        {
            if (keyItem[i].keyCode == _keyCode)
            {
                return keyItem[i];
            }
        }
        return null;
    }
}
