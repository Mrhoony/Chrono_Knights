using System;
using UnityEngine;

public enum EquipmentType
{
    Spear = 0,
    Gun,
    Active,
    Bell,
    Gloves,
    Shoes,
    Bag
}

[Serializable]
public class PlayerEquipment
{
    [Serializable]
    public struct Equipment
    {
        public EquipmentType equipmentType;
        public string name;
        public float[] addStatus;    // 0 addAtk 1 addDefense 2 addmoveSpeed 3 addAttackSpeed 4 addDashDistance 5 addRecovery 6 addJumpCount
        public int itemCode;
        public int itemRarity;
        public int upStatus;
        public int downStatus;
        public float[] max;
        public float[] min;
        public bool enchant;
        public int skillCode;

        public void Init(string _name, float[] _addStatus, EquipmentType _equipmentType)
        {
            name = _name;
            addStatus = _addStatus;
            upStatus = 8;
            downStatus = 8;
            max = new float[6];
            min = new float[6];
            enchant = false;
            itemCode = 0;
            itemRarity = 0;
            skillCode = 0;
            equipmentType = _equipmentType;

            LimitUpgradeSet(0);
        }
        public void LimitUpgradeSet(float _max, float _min = 0)
        {
            max[0] = _max;
            max[1] = _max;
            max[2] = _max;
            max[3] = _max;
            max[4] = _max;
            max[5] = _max;

            min[0] = _min;
            min[1] = _min;
            min[2] = _min;
            min[3] = _min;
            min[4] = _min;
            min[5] = _min;
        }

        public void EquipmentItemSetting(Item _item)
        {
            name = _item.itemName;
            itemCode = _item.itemCode;
            itemRarity = _item.itemRarity;
            EquipmentUpgradeLimit();
            enchant = true;
        }
        public void EquipmentUpgradeLimit()
        {
            if (itemRarity != 0)
            {
                switch (itemRarity)
                {
                    case 1:
                        LimitUpgradeSet(0.8f);
                        break;
                    case 2:
                        LimitUpgradeSet(1f);
                        break;
                    case 3:
                        LimitUpgradeSet(1.5f, -0.2f);
                        break;
                }
            }
            else
            {
                LimitUpgradeSet(0);
            }
        }
        public void EquipmentStatusEnchant(int _status, float _addStatus, bool _upgrade)
        {
            if (_upgrade)
            {
                upStatus = _status;
                addStatus[upStatus] = _addStatus * 0.01f;
                if (addStatus[upStatus] > max[upStatus]) addStatus[upStatus] = max[upStatus];
            }
            else
            {
                downStatus = _status;
                addStatus[downStatus] = _addStatus * -0.01f;
                if (addStatus[downStatus] > min[downStatus]) addStatus[downStatus] = min[downStatus];
            }
        }
        public void EquipmentStatusUpgrade(int _status, float _addStatus, bool _upgrade)
        {
            if (_upgrade)
            {
                addStatus[_status] += _addStatus * 0.01f;
                if (addStatus[_status] > max[_status]) addStatus[_status] = max[_status];
            }
            else
            {
                addStatus[_status] += _addStatus * 0.01f;
                if (addStatus[_status] > min[_status]) addStatus[_status] = min[_status];
            }
        }
        public void EquipmentSkillCheck()
        {
            if (!enchant || itemRarity < 3)
            {
                skillCode = 0;
                return;
            }

            // 레어리티 3이상일 때
            if (skillCode == 0) // 스킬이 없으면
            {
                skillCode = Database_Game.instance.SkillSetting(equipmentType).skillCode;
                return;
            }

            if (Database_Game.instance.GetSkill(skillCode) != null)
                skillCode = Database_Game.instance.GetSkill(skillCode).skillCode;
        }
    }
    public Equipment[] equipment;      // 0 spear, 1 gun, 2 activeEquip, 3 tankTop, 4 shoes, 5 gloves, 6 bell

    public void Init()
    {
        float[] addStatus = {0,0,0,0,0,0};
        equipment = new Equipment[7];

        equipment[0].Init("창", addStatus, EquipmentType.Spear);
        equipment[1].Init("총", addStatus, EquipmentType.Gun);
        equipment[2].Init("총알", addStatus, EquipmentType.Active);
        equipment[3].Init("종 보호대", addStatus, EquipmentType.Bell);
        equipment[4].Init("맨 손", addStatus, EquipmentType.Gloves);
        equipment[5].Init("가죽 신발", addStatus, EquipmentType.Shoes);
        equipment[6].Init("가방", addStatus, EquipmentType.Bag);

        Debug.Log("equipment Init");
    }
    public void PlayerEquipmentInit(int num)
    {
        float[] addStatus = { 0, 0, 0, 0, 0, 0 };
        switch (num)
        {
            case 0:
                equipment[0].Init("창", addStatus, EquipmentType.Spear);
                break;
            case 1:
                equipment[1].Init("총", addStatus, EquipmentType.Gun);
                break;
            case 2:
                equipment[2].Init("총알", addStatus, EquipmentType.Active);
                break;
            case 3:
                equipment[3].Init("종 보호대", addStatus, EquipmentType.Bell);
                break;
            case 4:
                equipment[4].Init("맨 손", addStatus, EquipmentType.Gloves);
                break;
            case 5:
                equipment[5].Init("가죽 신발", addStatus, EquipmentType.Shoes);
                break;
            case 6:
                equipment[6].Init("가방", addStatus, EquipmentType.Bag);
                break;
        }
    }
    public void EquipmentLimitUpgrade()
    {
        for(int i = 0; i < 7; ++i)
        {
            equipment[i].EquipmentUpgradeLimit();
        }
    }
    public void EquipmentSkillSetting(int _EquipNumber, int _SkillCode)
    {
        equipment[_EquipNumber].skillCode = _SkillCode;
        Database_Game.instance.skillManager.EquipmentSkillSetting(equipment);
    }
    public void EquipmentSkillCheck(PlayerStatus _PlayerStatus)
    {
        for (int i = 0; i < 7; ++i)
        {
            equipment[i].EquipmentSkillCheck();
        }
        Database_Game.instance.skillManager.Init(_PlayerStatus);
        Database_Game.instance.skillManager.EquipmentSkillSetting(equipment);
    }
    
    public string GetStatusName(int slotNum, bool upDown)
    {
        string statusName = "";

        if (upDown)
        {
            switch (equipment[slotNum].upStatus)
            {
                case 0:
                    statusName = "공격력";
                    break;
                case 1:
                    statusName = "방어력";
                    break;
                case 2:
                    statusName = "이동 속도";
                    break;
                case 3:
                    statusName = "공격 속도";
                    break;
                case 4:
                    statusName = "돌진 거리";
                    break;
                case 5:
                    statusName = "회복력";
                    break;
                default:
                    statusName = "";
                    break;
            }
        }
        else
        {
            switch (equipment[slotNum].downStatus)
            {
                case 0:
                    statusName = "공격력";
                    break;
                case 1:
                    statusName = "방어력";
                    break;
                case 2:
                    statusName = "이동 속도";
                    break;
                case 3:
                    statusName = "공격 속도";
                    break;
                case 4:
                    statusName = "돌진 거리";
                    break;
                case 5:
                    statusName = "회복력";
                    break;
                default:
                    statusName = "";
                    break;
            }
        }
        return statusName;
    }
    public string GetUpStatus(int slotNum)
    {
        string StatusString;

        if (equipment[slotNum].upStatus != 8)
            StatusString = ((int)(equipment[slotNum].addStatus[equipment[slotNum].upStatus] * 100)).ToString();
        else
            StatusString = "";

        return StatusString;
    }
    public string GetDownStatus(int slotNum)
    {
        string StatusString;

        if (equipment[slotNum].downStatus != 8)
            StatusString = ((int)(equipment[slotNum].addStatus[equipment[slotNum].downStatus] * 100)).ToString();
        else
            StatusString = "";

        return StatusString;
    }
    public float GetStatusValue(int statusNum)
    {
        float value = 0;

        for (int i = 0; i < 7; ++i)
        {
            value += equipment[i].addStatus[statusNum];
        }

        return value;
    }
}
