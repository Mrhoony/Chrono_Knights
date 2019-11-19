using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Database : MonoBehaviour
{
    public static Item_Database instance;
    
    public List<Key> keyItem = new List<Key>();
    public List<Equip> equip = new List<Equip>();
    public List<EquipSkill> equipSkill = new List<EquipSkill>();

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
    {
        keyItem.Add(new Key("커먼", 1, 1, 231));
        keyItem.Add(new Key("매직", 2, 3, 232));
        keyItem.Add(new Key("유니크", 3, 3, 233));
    }
}
