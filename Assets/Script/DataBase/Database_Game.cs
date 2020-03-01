using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Number, ReturnTown, ReturnPreFloor, FreePassNextFloor, FreePassThisFloor, BossFloor, RepeatThisFloor
}
public enum ItemUsingType
{
    health, attack, defense, moveSpeed, bullet     // 1 회복, 2 공버프, 3 방버프, 4 공속버프, 5 이속버프
}
public enum SkillType
{
    Effect_Myself,
    Effect_Attack,
    Effect_Area
}

public class Skill
{
    public SkillType skillType;
    public string skillName;
    public int skillCode;
    public string skillDescription;
    public int skillRarity;
    public float skillCoolTime;
    public float skillTimeDuration;

    public Skill(SkillType _skillType, string _skillName, int _skillCode, int _skillRarity, string _skillDescription, float _skillCoolTime = 0, float _skillTimeDuration = 0)
    {
        skillType = _skillType;
        skillName = _skillName;
        skillCode = _skillCode;
        skillRarity = _skillRarity;
        skillDescription = _skillDescription;
        skillCoolTime = _skillCoolTime;
        skillTimeDuration = _skillTimeDuration;
    }
}

public class Item
{
    public ItemType itemType;
    public ItemUsingType usingType;
    public int usingStatus;

    public Sprite sprite;
    public string itemName;
    public int itemRarity;
    public int itemCode;
    public int Value;
    public string Description;
    public int skillCode;

    public Item(string _itemName, int _itemRarity, int _itemCode, string _Description, ItemType _itemType, ItemUsingType _usingType, int _usingStatus = 0, int _skillCode = 0)
    {
        itemName = _itemName;
        itemRarity = _itemRarity;
        itemCode = _itemCode;
        sprite = Resources.LoadAll<Sprite>("item/ui_itemset")[itemCode];
        Description = _Description;
        itemType = _itemType;
        switch (itemType)
        {
            case ItemType.Number:
                {
                    if (itemRarity < 2)
                    {
                        Value = Random.Range(1, 5);
                    }
                    else
                    {
                        Value = Random.Range(1, 4) * 5;
                    }
                }
                break;
        }
        usingType = _usingType;
        usingStatus = _usingStatus;
        skillCode = _skillCode;
    }

    public void SetSkillNum(int _skillCode)
    {
        skillCode = _skillCode;
    }
}

public class Database_Game : MonoBehaviour
{
    public static Database_Game instance;

    public List<Item> Item = new List<Item>();
    public List<Skill> skillList = new List<Skill>();

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
        InputItem();
        InputSkill();
    }

    void InputItem()
    {                     // 이름, 등급, 아이템 코드, 효과 
        Item.Add(new Item("커먼", 1, 7, "", ItemType.Number, ItemUsingType.health, 20));
        Item.Add(new Item("커먼", 1, 7, "", ItemType.ReturnTown, ItemUsingType.attack, 2));
        Item.Add(new Item("매직", 2, 8, "", ItemType.Number, ItemUsingType.moveSpeed, 1));
        Item.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
        Item.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
        Item.Add(new Item("유니크", 3, 9, "", ItemType.Number, ItemUsingType.attack, 5));
        Item.Add(new Item("유니크", 3, 7, "", ItemType.RepeatThisFloor, ItemUsingType.defense, 1));
    }

    void InputSkill()
    {
        skillList.Add(new Skill(SkillType.Effect_Myself, "Heal", 100, 1, "체력을 회복한다", 5f));
        skillList.Add(new Skill(SkillType.Effect_Myself, "Attack UP", 101, 1, "공격력을 증가시킨다", 5f, 5f));
        skillList.Add(new Skill(SkillType.Effect_Myself, "Defense UP", 102, 1, "방어력을 증가시킨다", 5f, 5f));
        skillList.Add(new Skill(SkillType.Effect_Myself, "Attack Speed UP", 103, 1, "공격속도를 증가시킨다", 5f, 5f));
    }

    public int[] GetSkillRarityList(int minRarity)
    {
        int skillListCount = skillList.Count;
        int count = 0;
        for (int i = 0; i < skillListCount; ++i)
        {
            Debug.Log(skillList[i].skillRarity);
            Debug.Log(minRarity);
            if (skillList[i].skillRarity <= minRarity) ++count;
        }
        int[] enableSkillList = new int[count];
        count = 0;
        for (int i = 0; i < skillListCount; ++i)
        {
            if (skillList[i].skillRarity <= minRarity)
            {
                enableSkillList[count] = skillList[i].skillCode;
                ++count;
            }
        }
        return enableSkillList;
    }

    public Item GetItem(int _itemCode)
    {
        for (int i = 0; i < Item.Count; i++)
        {
            if (Item[i].itemCode == _itemCode)
            {
                return Item[i];
            }
        }
        return null;
    }

    public Skill CheckSkill(int _skillCode)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].skillCode == _skillCode)
            {
                return skillList[i];
            }
        }
        return null;
    }
}
