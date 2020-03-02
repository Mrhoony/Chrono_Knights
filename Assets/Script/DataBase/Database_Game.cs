using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

                }
            }
        }
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
