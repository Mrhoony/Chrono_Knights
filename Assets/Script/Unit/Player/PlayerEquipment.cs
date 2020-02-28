using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEquipment
{
    [Serializable]
    public struct Equipment
    {
        public string name;
        public float[] addStatus;    // 0 addAtk 1 addDefense 2 addmoveSpeed 3 addAttackSpeed 4 addDashDistance 5 addRecovery 6 addJumpCount
        public int itemCode;
        public int itemRarity;
        public int upStatus;
        public int downStatus;
        public float[] max;
        public float[] min;
        public int skillCode;
        public int skillRarity;
        public bool isUsed;
        public bool enchant;

        public void Init(string _name, float[] _addStatus)
        {
            name = _name;
            addStatus = _addStatus;
            upStatus = 8;
            downStatus = 8;
            max = new float[6];
            min = new float[6];
            isUsed = false;
            enchant = false;
            itemCode = 0;
            skillCode = 0;
        }
        public void EquipmentItemSetting(Item _item)
        {
            name = _item.itemName;
            itemCode = _item.itemCode;
            itemRarity = _item.itemRarity;
            skillCode = _item.skillCode;
            skillRarity = itemRarity;
            if (itemRarity != 0)
            {
                switch (itemRarity)
                {
                    case 1:
                        LimitUpgradeSet(4f);
                        break;
                    case 2:
                        LimitUpgradeSet(8f);
                        break;
                    case 3:
                        LimitUpgradeSet(15f, -10f);
                        break;
                }
            }
            enchant = true;
        }
        public void EquipmentStatusEnchant(int _status, float _addStatus, bool _upgrade)
        {
            if (_upgrade)
            {
                upStatus = _status;
                switch (upStatus)
                {
                    case 0:
                    case 1:
                        addStatus[upStatus] = _addStatus;
                        if (addStatus[upStatus] > max[upStatus]) addStatus[upStatus] = max[upStatus];
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        addStatus[_status] = _addStatus * 0.1f;
                        if (addStatus[upStatus] > max[upStatus]) addStatus[upStatus] = max[upStatus];
                        break;
                }
            }
            else
            {
                downStatus = _status;
                switch (downStatus)
                {
                    case 0:
                    case 1:
                        addStatus[downStatus] = _addStatus;
                        if (addStatus[downStatus] < min[downStatus]) addStatus[downStatus] = min[downStatus];
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        addStatus[downStatus] = _addStatus * 0.1f;
                        if (addStatus[downStatus] < min[downStatus]) addStatus[downStatus] = min[downStatus];
                        break;
                }
            }
        }
        public void EquipmentStatusUpgrade(int _status, float _addStatus, bool _upgrade)
        {
            if (_upgrade)
            {
                switch (_status)
                {
                    case 0:
                    case 1:
                        addStatus[_status] += _addStatus;
                        if (addStatus[_status] > max[_status]) addStatus[_status] = max[_status];
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        addStatus[_status] += _addStatus * 0.1f;
                        if (addStatus[_status] > max[_status]) addStatus[_status] = max[_status];
                        break;
                }
            }
            else
            {
                switch (_status)
                {
                    case 0:
                    case 1:
                        addStatus[_status] += _addStatus;
                        if (addStatus[_status] < min[_status]) addStatus[_status] = min[_status];
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        addStatus[_status] += _addStatus * 0.1f;
                        if (addStatus[_status] < min[_status]) addStatus[_status] = min[_status];
                        break;
                }
            }
        }
        public void LimitUpgradeSet(float _max, float _min = 0)
        {
            max[0] = _max;
            max[1] = _max;
            max[2] = _max * 0.1f;
            max[3] = _max * 0.1f;
            max[4] = _max * 0.1f;
            max[5] = _max * 0.1f;

            min[0] = _min;
            min[1] = _min;
            min[2] = _min * 0.1f;
            min[3] = _min * 0.1f;
            min[4] = _min * 0.1f;
            min[5] = _min * 0.1f;
        }
    }


    public Equipment[] equipment;      // 0 bell, 1 armor, 2 spear, 3 gun, 4 shoes, 5 gloves, 6 activeEquip
    
    public void PlayerEquipmentInit()
    {
        float[] addStatus = {0,0,0,0,0,0};
        equipment = new Equipment[7];

        equipment[0].Init("종", addStatus);
        equipment[1].Init("창", addStatus);
        equipment[2].Init("총", addStatus);
        equipment[3].Init("흰 옷", addStatus);
        equipment[4].Init("가죽 신발", addStatus);
        equipment[5].Init("맨 손", addStatus);
        equipment[6].Init("액티브", addStatus);

        Debug.Log("equipment Init");
    }
    public void Init(int num)
    {
        float[] addStatus = { 0, 0, 0, 0, 0, 0, 0 };
        switch (num)
        {
            case 0:
                equipment[0].Init("종", addStatus);
                break;
            case 1:
                equipment[1].Init("창", addStatus);
                break;
            case 2:
                equipment[2].Init("총", addStatus);
                break;
            case 3:
                equipment[3].Init("흰 옷", addStatus);
                break;
            case 4:
                equipment[4].Init("가죽 신발", addStatus);
                break;
            case 5:
                equipment[5].Init("맨 손", addStatus);
                break;
            case 6:
                equipment[6].Init("", addStatus);
                break;
        }
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
                case 8:
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
                case 8:
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
            StatusString = equipment[slotNum].addStatus[equipment[slotNum].upStatus].ToString("N2");
        else
            StatusString = "";

        return StatusString;
    }
    public string GetDownStatus(int slotNum)
    {
        string StatusString;

        if (equipment[slotNum].downStatus != 8)
            StatusString = equipment[slotNum].addStatus[equipment[slotNum].downStatus].ToString("N2");
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
