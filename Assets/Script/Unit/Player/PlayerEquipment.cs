using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerEquipment
{
    struct Equipment
    {
        string name;
        int Atk;
        float attackSpeed;
        float moveSpeed;
        int JumpCount;         // 점프 횟수
        int Defense;           // 안정성(방어력)
        float Recovery;        // 회복력
        float DashDistance;    // 대시거리

        float addAtk;
        float addAttackSpeed;
        float addmoveSpeed;
        float addJumpCount;
        float addDefense;
        float addRecovery;
        float addDashDistance;
        
        public void Init(Key key)
        {
            name = key.equipName;
            Atk = key.equipAtk;
            attackSpeed = key.equipAttackSpeed;
            moveSpeed = key.equipMoveSpeed;
            JumpCount = key.equipJumpCount;
            Defense = key.equipDefense;
            Recovery = key.equipRecovery;
            DashDistance = key.equipDashDistance;

            switch (key.keyRarity)
            {
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
            }
        }
    }

    Equipment bell;
    Equipment armor;
    Equipment weapon;
    Equipment shoes;
    Equipment gloves;
    Equipment activeEquip;
    


}
