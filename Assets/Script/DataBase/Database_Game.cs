using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public enum ItemType
{
    Number, ReturnTown, ReturnPreFloor, FreePassNextFloor, FreePassThisFloor, SetBossFloor, RepeatThisFloor
}
public enum ItemUsingType
{
    Health, Bullet, Attack, Defense, MoveSpeed, ReturnTown
}
public enum SkillType
{
    Active,
    Passive,
    Unlock
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
    public int value;
    public string Description;
    public int skillCode;

    public Item(string _itemName, int _itemRarity, int _itemCode, string _Description, ItemType _itemType, ItemUsingType _usingType, int _usingStatus = 0, int _skillCode = 0)
    {
        itemName = _itemName;
        itemRarity = _itemRarity;
        itemCode = _itemCode;
        itemType = _itemType;

        if (itemRarity == 1)
        {
            value = Random.Range(1, 5);
            sprite = SpriteSet.itemSprite[value + 6];
        }
        else if (itemRarity == 2)
        {
            value = Random.Range(1, 4) * 2;
            sprite = SpriteSet.itemSprite[11];
        }
        else if (itemRarity == 3)
        {
            value = Random.Range(1, 4) * 3;
            sprite = SpriteSet.itemSprite[13];
        }

        switch (itemType)
        {
            case ItemType.Number:
                break;
            case ItemType.ReturnPreFloor:
                sprite = SpriteSet.itemSprite[14];
                break;
            case ItemType.FreePassThisFloor:
                sprite = SpriteSet.itemSprite[15];
                break;
            case ItemType.FreePassNextFloor:
                sprite = SpriteSet.itemSprite[16];
                break;
            case ItemType.SetBossFloor:
                sprite = SpriteSet.itemSprite[17];
                break;
            case ItemType.RepeatThisFloor:
                sprite = SpriteSet.itemSprite[18];
                break;
            default:
                value = 1;
                break;
        }
        usingType = _usingType;
        if (usingType == ItemUsingType.ReturnTown)
            sprite = SpriteSet.itemSprite[14];

        Description = _Description + "\r\n" + value.ToString() + " 표시";
        usingStatus = _usingStatus;
        skillCode = _skillCode;
    }
    public void SetSkillNum(int _skillCode)
    {
        skillCode = _skillCode;
    }
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
public class Monster
{
    public int monsterCode;
    public string monsterName;
    public int monsterHP;
    public float monsterMoveSpeed;
    public float monsterAttackSpeed;
    public float monsterAttackRange;
    public int monsterAttack;
    public int monsterDefense;
    public int monsterPopChance;

    public Monster(int _monsterCode, string _monsterName, int _monsterHP, float _monstermoveSpeed, float _monsterAttackSpeed, float _monsterAttackRange, int _monsterAttack, int _monsterDefense, int _monsterPopChance)
    {
        monsterCode = _monsterCode;
        monsterName = _monsterName;
        monsterHP = _monsterHP;
        monsterMoveSpeed = _monstermoveSpeed;
        monsterAttackSpeed = _monsterAttackSpeed;
        monsterAttackRange = _monsterAttackRange;
        monsterAttack = _monsterAttack;
        monsterDefense = _monsterDefense;
        monsterPopChance = _monsterPopChance;
    }
}

public class Database_Game : MonoBehaviour
{
    public static Database_Game instance;

    public List<Item> Item = new List<Item>();
    public List<Skill> skillList = new List<Skill>();
    public List<Monster> monsterList = new List<Monster>();

    readonly string itemXMLFileName = "ItemDataBase";
    readonly string skillXMLFileName = "SkillDataBase";
    readonly string MonsterXMLFileName = "MonsterDataBase";

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
        InputItemData(itemXMLFileName);
        InputSkillData(skillXMLFileName);
        InputMonsterData(MonsterXMLFileName);
    }

    XmlNodeList XmlNodeReturn(string filePass)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/" + filePass);
        XmlDocument XMLFile = new XmlDocument();
        XMLFile.LoadXml(textAsset.text);
        
        XmlNodeList nodelist = XMLFile.SelectNodes(filePass);
        return nodelist;
    }

    void InputItemData(string _itemFileName)
    {
        XmlNodeList nodelist = XmlNodeReturn(_itemFileName);
        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_itemFileName) && node.HasChildNodes)
            {
                foreach (XmlNode data in node)
                {
                    Item.Add(new Item(
                        data.Attributes.GetNamedItem("itemName").Value,
                        int.Parse(data.Attributes.GetNamedItem("itemRarity").Value),
                        int.Parse(data.Attributes.GetNamedItem("itemCode").Value),
                        data.Attributes.GetNamedItem("itemDescription").Value,
                        (ItemType)System.Enum.Parse(typeof(ItemType), data.Attributes.GetNamedItem("itemType").Value),
                        (ItemUsingType)System.Enum.Parse(typeof(ItemUsingType), data.Attributes.GetNamedItem("itemUsingType").Value),
                        int.Parse(data.Attributes.GetNamedItem("itemValue").Value)));
                }
            }
        }
        Debug.Log("Input ItemData");
    }
    void InputSkillData(string _skillFileName)
    {
        XmlNodeList nodelist = XmlNodeReturn(_skillFileName);

        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_skillFileName) && node.HasChildNodes)
            {
                foreach (XmlNode data in node)
                {
                    skillList.Add(new Skill(
                        (SkillType)System.Enum.Parse(typeof(SkillType), data.Attributes.GetNamedItem("skillType").Value),
                        data.Attributes.GetNamedItem("skillName").Value,
                        int.Parse(data.Attributes.GetNamedItem("skillCode").Value),
                        int.Parse(data.Attributes.GetNamedItem("skillRarity").Value),
                        data.Attributes.GetNamedItem("skillDescription").Value,
                        int.Parse(data.Attributes.GetNamedItem("skillOnCount").Value),
                        float.Parse(data.Attributes.GetNamedItem("skillTimeDuration").Value)));
                }
            }
        }
        Debug.Log("Input skillData");
    }
    void InputMonsterData(string _monsterFileName)
    {
        XmlNodeList nodelist = XmlNodeReturn(_monsterFileName);

        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_monsterFileName) && node.HasChildNodes)
            {
                foreach (XmlNode data in node)
                {
                    monsterList.Add(new Monster(
                        int.Parse(data.Attributes.GetNamedItem("monsterCode").Value),
                        data.Attributes.GetNamedItem("monsterName").Value,
                        int.Parse(data.Attributes.GetNamedItem("monsterHP").Value),
                        float.Parse(data.Attributes.GetNamedItem("monsterMoveSpeed").Value),
                        float.Parse(data.Attributes.GetNamedItem("monsterAttackSpeed").Value),
                        float.Parse(data.Attributes.GetNamedItem("monsterAttackRange").Value),
                        int.Parse(data.Attributes.GetNamedItem("monsterAttack").Value),
                        int.Parse(data.Attributes.GetNamedItem("monsterDefense").Value),
                        int.Parse(data.Attributes.GetNamedItem("monsterPopChance").Value)
                        ));
                }
            }
        }
        Debug.Log("Input MonsterData");
    }

    public Monster GetMonsterStatus(int _monsterCode)
    {
        int count = monsterList.Count;
        for(int i = 0; i<count; ++i)
        {
            if(monsterList[i].monsterCode == _monsterCode)
                return monsterList[i];
        }
        Debug.Log("몬스터가 존재하지 않음");
        return null;
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
        int count = Item.Count;
        for (int i = 0; i < count; i++)
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
