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
            Destroy(gameObject);
        InputKeyItem();
    }

    void InputKeyItem()
    {                     // 이름, 효과, 등급, 아이템 코드, 장비화 이름, 
        keyItem.Add(new Key("커먼", 1, 1, 231 , "흰색", 0, 1, 0));
        keyItem.Add(new Key("매직", 2, 2, 232 , "회색", 3, 1, 1));
        keyItem.Add(new Key("유니크", 3, 3, 233, "검은색", 5, 1, 2));
    }
}
