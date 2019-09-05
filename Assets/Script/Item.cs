using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item instance;
    
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
