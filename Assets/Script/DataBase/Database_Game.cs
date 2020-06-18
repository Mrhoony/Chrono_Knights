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
    Health, Bullet, Attack, Defense, MoveSpeed, ReturnTown, FreePassThisFloor
}
public enum SkillType
{
    Active,
    Passive
}
public enum EventType
{
    None, Select, Answer, Camera, CameraFocus, CameraScroll
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
            sprite = SpriteSet.itemSprite[12];
        }
        else if (itemRarity == 3)
        {
            value = Random.Range(1, 4) * 3;
            sprite = SpriteSet.itemSprite[13];
        }

        switch (itemType)
        {
            case ItemType.Number:
                Description = _Description + "\r\n" + value.ToString() + " - 탑의 기운";
                break;
            case ItemType.ReturnTown:
                Description = _Description + "\r\n" + value.ToString() + "\r\n돌아가고 싶어진다.";
                sprite = SpriteSet.itemSprite[14];
                break;
            case ItemType.RepeatThisFloor:
                Description = _Description + "\r\n" + value.ToString() + "\r\n알 수 없는 힘이 느껴진다.";
                sprite = SpriteSet.itemSprite[11];
                break;
            case ItemType.FreePassThisFloor:
                Description = _Description + "\r\n" + value.ToString() + "\r\n알 수 없는 힘이 느껴진다.";
                sprite = SpriteSet.itemSprite[15];
                break;
            case ItemType.FreePassNextFloor:
                Description = _Description + "\r\n" + value.ToString() + "\r\n알 수 없는 힘이 느껴진다.";
                sprite = SpriteSet.itemSprite[16];
                break;
            case ItemType.SetBossFloor:
                Description = _Description + "\r\n" + value.ToString() + "\r\n알 수 없는 힘이 느껴진다.";
                sprite = SpriteSet.itemSprite[17];
                break;
            case ItemType.ReturnPreFloor:
                Description = _Description + "\r\n" + value.ToString() + "\r\n알 수 없는 힘이 느껴진다.";
                sprite = SpriteSet.itemSprite[18];
                break;
        }
        usingStatus = _usingStatus;
    }
}
public class Skill
{
    public SkillType skillType;
    public int skillCode;
    public string skillName;
    public float skillMultiply;
    public int skillValue;
    public string skillDescription;
    public float skillCoolTime;
    public float skillTimeDuration;
    public int skillStack;

    public Skill(SkillType _skillType, int _skillCode, string _skillName, float _skillMultiply, int _skillValue, string _skillDescription, float _skillCoolTime, float _skillTimeDuration, int _skillStack)
    {
        skillType = _skillType;
        skillCode = _skillCode;
        skillName = _skillName;
        skillMultiply = _skillMultiply;
        skillValue = _skillValue;
        skillDescription = _skillDescription;
        skillCoolTime = _skillCoolTime;
        skillTimeDuration = _skillTimeDuration;
        skillStack = _skillStack;
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
    public int monsterWeight;

    public Monster(int _monsterCode, string _monsterName, int _monsterHP, float _monstermoveSpeed, float _monsterAttackSpeed, float _monsterAttackRange, int _monsterAttack, int _monsterDefense, int _monsterPopChance, int _monsterWeight)
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
        monsterWeight = _monsterWeight;
    }
}
public class PlayerAttack
{
    public AtkType attackType;
    public float attackXPoint;
    public float attackYPoint;
    public float attackXRange;
    public float attackYRange;
    public float attackMultiply;
    public float distanceMultiply;
    public int knockBack;
    public int attackMultiHit;

    public PlayerAttack(AtkType _attackType, float _attackXPoint, float _attackYPoint, float _attackXRange, float _attackYRange, float _attackMultiply, float _distanceMultiply, int _knockBack, int _attackMultiHit)
    {
        attackType = _attackType;
        attackXPoint = _attackXPoint;
        attackYPoint = _attackYPoint;
        attackXRange = _attackXRange;
        attackYRange = _attackYRange;
        attackMultiply = _attackMultiply;
        distanceMultiply = _distanceMultiply;
        knockBack = _knockBack;
        attackMultiHit = _attackMultiHit;
    }
}
public class EventDialog
{
    public int eventCount;
    public EventType eventType;
    public string NPCName;
    public string content;

    public EventDialog(EventType _EventType)
    {
        eventType = _EventType;
    }
    public EventDialog(EventType _EventType, string _NPCName)
    {
        eventType = _EventType;
        NPCName = _NPCName;
    }
    public EventDialog(int _EventCount, EventType _EventType, string _NPCName, string _Content)
    {
        eventCount = _EventCount;
        eventType = _EventType;
        NPCName = _NPCName;
        content = _Content;
    }
}

public class RepeatDialog
{
    public string NPCName;
    public string content;

    public RepeatDialog(string _NPCName, string _Content)
    {
        NPCName = _NPCName;
        content = _Content;
    }
}
public class RepeatEventDialog
{
    public int eventNumber;
    public List<RepeatDialog> eventDialog;

    public RepeatEventDialog(int _EventNumber, List<RepeatDialog> _EventDialog)
    {
        eventNumber = _EventNumber;
        eventDialog = _EventDialog;
    }
}
public class EventTalkBox
{
    public int scenarioNumber;
    public string content;

    public EventTalkBox(int _ScenarioNumber, string _Content)
    {
        scenarioNumber = _ScenarioNumber;
        content = _Content;
    }
}

public class Database_Game : MonoBehaviour
{
    public static Database_Game instance;
    public SkillManager skillManager;

    public List<Item> Item = new List<Item>();
    public List<Skill> activeSkillList = new List<Skill>();
    public List<Skill> passiveSkillList = new List<Skill>();
    public List<Monster> monsterList = new List<Monster>();
    public List<PlayerAttack> playerAttack = new List<PlayerAttack>();
    public List<string> mainScenarioName = new List<string>();
    
    readonly string itemXMLFileName = "ItemDataBase";
    readonly string skillXMLFileName = "SkillDataBase";
    readonly string MonsterXMLFileName = "MonsterDataBase";
    readonly string playerAttackXMLFileName = "PlayerAttackDataBase";
    readonly string EventListFileName = "EventListDataBase";
    readonly string RepeatTalkFileName = "RepeatTalkDataBase";
    readonly string NPCTalkBoxFileName = "NPCTalkBoxDataBase";

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
        InputPlayerAttack(playerAttackXMLFileName);
        InputEventData(EventListFileName);
        InputNPCDialogData(RepeatTalkFileName);
        INputNPCTalkBoxData(NPCTalkBoxFileName);

        skillManager = GetComponent<SkillManager>();
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
                    switch ((SkillType)System.Enum.Parse(typeof(SkillType), data.Attributes.GetNamedItem("skillType").Value))
                    {
                        case SkillType.Active:
                            activeSkillList.Add(new Skill(
                                (SkillType)System.Enum.Parse(typeof(SkillType), data.Attributes.GetNamedItem("skillType").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillCode").Value),
                                data.Attributes.GetNamedItem("skillName").Value,
                                float.Parse(data.Attributes.GetNamedItem("skillMultiply").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillValue").Value),
                                data.Attributes.GetNamedItem("skillDescription").Value,
                                int.Parse(data.Attributes.GetNamedItem("skillOnCount").Value),
                                float.Parse(data.Attributes.GetNamedItem("skillTimeDuration").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillMaxStack").Value)));
                            break;
                        case SkillType.Passive:
                            passiveSkillList.Add(new Skill(
                                (SkillType)System.Enum.Parse(typeof(SkillType), data.Attributes.GetNamedItem("skillType").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillCode").Value),
                                data.Attributes.GetNamedItem("skillName").Value,
                                float.Parse(data.Attributes.GetNamedItem("skillMultiply").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillValue").Value),
                                data.Attributes.GetNamedItem("skillDescription").Value,
                                int.Parse(data.Attributes.GetNamedItem("skillOnCount").Value),
                                float.Parse(data.Attributes.GetNamedItem("skillTimeDuration").Value),
                                int.Parse(data.Attributes.GetNamedItem("skillMaxStack").Value)));
                            break;
                    }
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
                        int.Parse(data.Attributes.GetNamedItem("monsterPopChance").Value),
                        int.Parse(data.Attributes.GetNamedItem("monsterWeight").Value)
                        ));
                }
            }
        }
        Debug.Log("Input MonsterData");
    }
    void InputPlayerAttack(string _playerAttackFileName)
    {
        XmlNodeList nodelist = XmlNodeReturn(_playerAttackFileName);
        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_playerAttackFileName) && node.HasChildNodes)
            {
                foreach (XmlNode data in node)
                {
                    playerAttack.Add(new PlayerAttack(
                        (AtkType)System.Enum.Parse(typeof(AtkType), data.Attributes.GetNamedItem("attackType").Value),
                        float.Parse(data.Attributes.GetNamedItem("attackXPoint").Value),
                        float.Parse(data.Attributes.GetNamedItem("attackYPoint").Value),
                        float.Parse(data.Attributes.GetNamedItem("attackXRange").Value),
                        float.Parse(data.Attributes.GetNamedItem("attackYRange").Value),
                        float.Parse(data.Attributes.GetNamedItem("attackMultiply").Value),
                        float.Parse(data.Attributes.GetNamedItem("distanceMultiply").Value),
                        int.Parse(data.Attributes.GetNamedItem("knockBack").Value),
                        int.Parse(data.Attributes.GetNamedItem("multiHit").Value)
                        ));
                }
            }
        }
        Debug.Log("Input attackData");
    }
    void InputEventData(string _EventListFileName)
    {
        Dictionary<string, int> eventList = new Dictionary<string, int>();
        Dictionary<string, List<EventDialog>> eventContent = new Dictionary<string, List<EventDialog>>();
        List<EventDialog> eventDialog;

        XmlNodeList nodelist = XmlNodeReturn(_EventListFileName);
        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_EventListFileName) && node.HasChildNodes)
            {
                foreach (XmlNode _Event in node)
                {
                    switch (_Event.Name)
                    {
                        case "EventList":

                            eventDialog = new List<EventDialog>();

                            string eventName = _Event.SelectSingleNode("EventName").InnerText;
                            XmlNodeList _DialogList = _Event.SelectNodes("Dialog");

                            foreach (XmlNode _Dialog in _DialogList)
                            {
                                if(_Dialog.SelectSingleNode("EventType").InnerText == "Camera")
                                {
                                    eventDialog.Add(new EventDialog(
                                           (EventType)System.Enum.Parse(typeof(EventType), _Dialog.SelectSingleNode("EventType").InnerText)
                                           ));
                                }
                                else if (_Dialog.SelectSingleNode("EventType").InnerText == "CameraScroll")
                                {
                                    eventDialog.Add(new EventDialog(
                                           (EventType)System.Enum.Parse(typeof(EventType), _Dialog.SelectSingleNode("EventType").InnerText),
                                           _Dialog.SelectSingleNode("NPCName").InnerText
                                           ));
                                }
                                else
                                {
                                    eventDialog.Add(new EventDialog(
                                        int.Parse(_Dialog.SelectSingleNode("EventCount").InnerText),
                                        (EventType)System.Enum.Parse(typeof(EventType), _Dialog.SelectSingleNode("EventType").InnerText),
                                        _Dialog.SelectSingleNode("NPCName").InnerText,
                                        _Dialog.SelectSingleNode("Content").InnerText
                                        ));
                                }
                            }

                            if (eventList.ContainsKey(eventName))
                            {
                                eventContent[eventName].AddRange(eventDialog);
                                break;
                            }

                            mainScenarioName.Add(eventName);
                            eventList.Add(eventName, int.Parse(_Event.SelectSingleNode("EventNumber").InnerText));
                            eventContent.Add(eventName, eventDialog);
                            break;
                    }
                }
            }
        }
        DungeonManager.instance.scenarioManager.SetEventList(eventList, eventContent);

        Debug.Log("Input NPCDialogData");
    }
    void InputNPCDialogData(string _NPCDialogFileName)
    {
        Dictionary<int, List<RepeatEventDialog>> repeatEventList = new Dictionary<int, List<RepeatEventDialog>>();
        List<RepeatEventDialog> repeatEventDialog;

        XmlNodeList nodelist = XmlNodeReturn(_NPCDialogFileName);
        foreach (XmlNode node in nodelist)
        {
            Debug.Log(node.Name);
            if (node.Name.Equals(_NPCDialogFileName) && node.HasChildNodes)
            {
                foreach (XmlNode _Event in node)
                {
                    repeatEventDialog = new List<RepeatEventDialog>();

                    if (!repeatEventList.ContainsKey(int.Parse(_Event.SelectSingleNode("NPCCode").InnerText)))
                        repeatEventList.Add(int.Parse(_Event.SelectSingleNode("NPCCode").InnerText), repeatEventDialog);

                    List<RepeatDialog> temp = new List<RepeatDialog>();
                    XmlNodeList _RepeatDialogList = _Event.SelectNodes("Dialog");
                    foreach (XmlNode _Dialog in _RepeatDialogList)
                    {
                        temp.Add(new RepeatDialog(
                        _Dialog.SelectSingleNode("NPCName").InnerText,
                        _Dialog.SelectSingleNode("Content").InnerText));
                    }
                    repeatEventList[int.Parse(_Event.SelectSingleNode("NPCCode").InnerText)].Add(
                        new RepeatEventDialog(int.Parse(_Event.SelectSingleNode("EventNumber").InnerText), temp));
                }
            }
        }
        DungeonManager.instance.scenarioManager.SetEventList(repeatEventList);

        Debug.Log("Input NPCDialogData");
    }
    void INputNPCTalkBoxData(string _NPCTalkBoxFileName)
    {
        Dictionary<int, List<EventTalkBox>> eventContent = new Dictionary<int, List<EventTalkBox>>();
        List<EventTalkBox> eventTalkBox = new List<EventTalkBox>();

        XmlNodeList nodelist = XmlNodeReturn(_NPCTalkBoxFileName);

        foreach (XmlNode node in nodelist)
        {
            if (node.Name.Equals(_NPCTalkBoxFileName) && node.HasChildNodes)
            {
                foreach (XmlNode NPC in node)
                {
                    eventTalkBox = new List<EventTalkBox>();
                    XmlNodeList _DialogList = NPC.SelectNodes("TalkBox");
                    foreach (XmlNode _Dialog in _DialogList)
                    {
                        // 여기
                        eventTalkBox.Add(new EventTalkBox(
                            int.Parse(_Dialog.SelectSingleNode("ScenarioNumber").InnerText),
                            _Dialog.SelectSingleNode("Content").InnerText
                            ));
                    }
                    eventContent.Add(int.Parse(NPC.SelectSingleNode("NPCCode").InnerText), eventTalkBox);
                }
            }
        }
        DungeonManager.instance.scenarioManager.SetEventTalkBoxList(eventContent);
        Debug.Log("Input NPCTalkBoxData");
    }

    public Item ItemSetting()
    {
        int count = Item.Count;
        return Item[Random.Range(0, count)];   
    }
    public Skill SkillSetting(EquipmentType _EquipmentType)
    {
        Skill _skill = null;
        int listCount = 0;
        if(_EquipmentType == EquipmentType.Active)
        {
            listCount = activeSkillList.Count;
            _skill = activeSkillList[Random.Range(0, listCount)];
        }
        else
        {
            listCount = passiveSkillList.Count;
            _skill = passiveSkillList[Random.Range(0, listCount)];
        }

        return _skill;
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
    public Skill GetSkill(int _skillCode)
    {
        for (int i = 0; i < activeSkillList.Count; i++)
        {
            if (activeSkillList[i].skillCode == _skillCode)
            {
                return activeSkillList[i];
            }
        }
        for (int i = 0; i < passiveSkillList.Count; i++)
        {
            if (passiveSkillList[i].skillCode == _skillCode)
            {
                return passiveSkillList[i];
            }
        }
        return null;
    }
    public PlayerAttack GetPlayerAttackInformation(AtkType _atkType)
    {
        int count = playerAttack.Count;
        for (int i = 0; i < count; ++i)
        {
            if(playerAttack[i].attackType == _atkType)
            {
                return playerAttack[i];
            }
        }
        return null;
    }
}
