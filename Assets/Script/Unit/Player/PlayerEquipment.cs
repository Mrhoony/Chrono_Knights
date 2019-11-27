using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEquipment
{
    public struct Equipment
    {
        private string _name;
        private float[] _addStatus;
        private Key _key;
        private int _upStatus;
        private int _downStatus;
        private bool _enchant;
        private float _Max;
        private float _Min;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public float[] addStatus    // 0 addmoveSpeed 1 addAtk 2 addAttackSpeed 3 addDefense 4 addJumpCount 5 addRecovery 6 addDashDistance
        {
            get { return _addStatus; }
            set { _addStatus = value; }
        }
        public Key key
        {
            get{ return _key; }
            set
            {
                _key = value;
                if(_key != null)
                    switch (_key.keyRarity)
                    {
                        case 1:
                            _Max = 0.8f;
                            break;
                        case 2:
                            _Max = 1f;
                            break;
                        case 3:
                            _Max = 1.5f;
                            _Min = -1f;
                            break;
                    }
            }
        }
        public bool enchant
        {
            get { return _enchant; }
            set { _enchant = value; }
        }
        public int upStatus
        {
            get { return _upStatus; }
            set { _upStatus = value; }
        }
        public int downStatus
        {
            get { return _downStatus; }
            set { _downStatus = value; }
        }
        public float max
        {
            get { return _Max; }
        }
        public float min
        {
            get { return _Min; }
        }

        public void Init(string _name, float[] _addStatus, Key _key = null)
        {
            name = _name;
            addStatus = _addStatus;
            key = _key;
            upStatus = 8;
            downStatus = 8;
            enchant = false;
        }
    }

    public Equipment[] equipment;      // 0 bell, 1 armor, 2 spear, 3 gun, 4 shoes, 5 gloves, 6 activeEquip
    
    public void Init()
    {
        float[] addStatus = {0,0,0,0,0,0,0};
        equipment = new Equipment[7];

        equipment[0].Init("종", addStatus);
        equipment[1].Init("창", addStatus);
        equipment[2].Init("총", addStatus);
        equipment[3].Init("흰 옷", addStatus);
        equipment[4].Init("가죽 신발", addStatus);
        equipment[5].Init("맨 손", addStatus);
        equipment[6].Init("", addStatus);
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

    public void SetEquipOption(int equipNum, string equipName, float[] status)
    {
        equipment[equipNum].name = equipName;
        equipment[equipNum].addStatus = status;
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
