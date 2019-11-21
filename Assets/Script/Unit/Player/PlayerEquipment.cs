using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEquipment
{
    public struct Equipment
    {
        string name;
        
        public float[] addStatus;   // 0 addmoveSpeed 1 addAtk 2 addAttackSpeed 3 addDefense 4 addJumpCount 5 addRecovery 6 addDashDistance

        public void Init(string _Name, float[] _addStatus)
        {
            name = _Name;
            addStatus = _addStatus;
        }

        public void SettingEquipmentAddStatus(string _Name, float[] _addStat)
        {
            name = _Name;
            addStatus = _addStat;
        }

        public float[] GetAddStatus()
        {
            return addStatus;
        }
    }

    public Equipment[] equipment;      // 0 bell, 1 armor, 2 weapon, 3 shoes, 4 gloves, 5 activeEquip
    
    public void Init()
    {
        float[] addStatus = {0,0,0,0,0,0,0};
        equipment = new Equipment[6];

        equipment[0].Init("종", addStatus);
        equipment[1].Init("흰 옷", addStatus);
        equipment[2].Init("창", addStatus);
        equipment[3].Init("가죽 신발", addStatus);
        equipment[4].Init("맨 손", addStatus);
        equipment[5].Init("", addStatus);
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
                equipment[1].Init("흰 옷", addStatus);
                break;
            case 2:
                equipment[2].Init("창", addStatus);
                break;
            case 3:
                equipment[3].Init("가죽 신발", addStatus);
                break;
            case 4:
                equipment[4].Init("맨 손", addStatus);
                break;
            case 5:
                equipment[5].Init("", addStatus);
                break;
        }
    }

    public void SetEquipOption(int equipNum, string equipName, float[] stat)
    {
        equipment[equipNum].SettingEquipmentAddStatus(equipName, stat);
    }

    public float[] GetEquipAddStat(int equipNum)
    {
        return equipment[equipNum].addStatus;
    }

    public float GetStatusValue(int num)
    {
        float value = 0f;
        int length = equipment.Length;

        for(int i = 0; i < length; i++)
        {
            value += equipment[i].addStatus[num];
        }
        return value;
    }

    public Equipment GetEquipment(int equipNum)
    {
        return equipment[equipNum];
    }
}
