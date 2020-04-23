using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public PlayerEquipment equipment;
    public Skill[] equipSkillList;

    public Skill[] liveTimeSkillList;
    public Skill[] coolTimeSkillList;
    public float[] skillLiveTime;
    public float[] skillCoolTime;

    #region SkillClassList

    FirstAidKit skill_FirstAidKit = new FirstAidKit();
    AuraSpear skill_AuraSpear = new AuraSpear();
    ChargingAttack skill_ChargingAttack = new ChargingAttack();
    ImmortalBuff skill_ImmortalBuff = new ImmortalBuff();

    #endregion

    public void Init(PlayerStatus _playerStatus)
    {
        playerStatus = _playerStatus;
        equipment = playerStatus.playerData.playerEquipment;
        equipSkillList = new Skill[7];

        liveTimeSkillList = new Skill[7];
        coolTimeSkillList = new Skill[7];
        skillLiveTime = new float[7];
        skillCoolTime = new float[7];

        SkillListInit();
    }
    public void SkillListInit()
    {
        for (int i = 0; i < 7; ++i)
        {
            liveTimeSkillList[i] = null;
            coolTimeSkillList[i] = null;
            skillLiveTime[i] = 0f;
            skillCoolTime[i] = 0f;
            equipSkillList[i] = null;
        }

        Debug.Log("skill list init");
    }
    public void EquipmentSkillSetting(int _EquipmentNum, Skill _Skill)
    {
        equipSkillList[_EquipmentNum] = _Skill;
    }


    public void SkillCoolTimeReset()
    {
        for (int i = 0; i < 7; ++i)
        {
            // 버프중인 스킬 제거
            if (liveTimeSkillList[i] != null)
            {
                SkillReset(liveTimeSkillList[i].skillCode);
                liveTimeSkillList[i] = null;
            }
            // 쿨타임 초기화
            if (coolTimeSkillList[i] != null)
            {
                coolTimeSkillList[i] = null;
                equipment.equipment[i].isUsed = false;
            }
            skillLiveTime[i] = 0f;
            skillCoolTime[i] = 0f;
        }
    }

    private void Update()
    {
        if (DungeonManager.instance.isSceneLoading) return;

        // 쿨타임 관리
        for(int i = 0; i < 7; ++i)
        {
            //
            if (liveTimeSkillList[i] != null)
            {
                skillLiveTime[i] -= Time.deltaTime;
                if (skillLiveTime[i] < 0)
                {
                    skillLiveTime[i] = 0f;
                    SkillReset(liveTimeSkillList[i].skillCode);
                    liveTimeSkillList[i] = null;
                }
            }

            if (coolTimeSkillList[i] != null)
            {
                skillCoolTime[i] -= Time.deltaTime;
                if (skillCoolTime[i] < 0)
                {
                    skillCoolTime[i] = 0f;
                    coolTimeSkillList[i] = null;
                    equipment.equipment[i].isUsed = false;
                }
            }
        }
    }

    public void ActiveSkillUse(GameObject _Player)
    {
        if (equipSkillList[2] == null || equipment.equipment[2].isUsed) return;
        equipment.equipment[2].isUsed = true;

        switch (equipSkillList[2].skillCode)
        {
            case 101:
                ActiveAuraSpearBuff();
                break;
            case 102:
                LaserAttack(_Player);
                break;
            case 103:
                AreaAttackSquare(_Player);
                break;
            case 104:
                ChargingAttackBuff();
                break;
            case 105:
                ImmortalBuff();
                break;
            case 106:
                ActiveShield();
                break;
            case 107:
                ResupplyBullet();
                break;
            case 108:
                Healling();
                break;
            case 109:
                Blink(_Player);
                break;
            case 110:
                break;
        }
    }
    public void SkillReset(int _SkillCode)
    {
        switch (_SkillCode)
        {
            case 101:
                skill_AuraSpear.OffSKill(playerStatus);
                break;
            case 104:
                skill_ChargingAttack.OffSKill(playerStatus);
                break;
            case 105:
                skill_ImmortalBuff.OffSKill(playerStatus);
                break;
            case 106:
                playerStatus.ShieldReset();
                break;
            default:
                break;
        }
    }

    #region 즉발 스킬 코드
    public void LaserAttack(GameObject _Player)
    {
        // laserattack 에서 오브젝트 온
        ActiveSkillSetting(equipment.equipment[2], equipSkillList[2]);
    }
    public void AreaAttackSquare(GameObject _Player)
    {
        ActiveSkillSetting(equipment.equipment[2], equipSkillList[2]);
    }
    public void ResupplyBullet()
    {
        ActiveSkillSetting(equipment.equipment[2], equipSkillList[2]);
        playerStatus.Resupply_Ammo(5);
    }
    public void Healling()
    {
        ActiveSkillSetting(equipment.equipment[2], equipSkillList[2]);
        playerStatus.IncreaseHP(10);
    }
    public void Blink(GameObject _Player)
    {
        ActiveSkillSetting(equipment.equipment[2], equipSkillList[2]);
    }
    #endregion
    #region 지속 스킬 코드
    public void ActiveAuraSpearBuff()
    {
        BuffSkillSetting(equipment.equipment[2], equipSkillList[2]);
        skill_FirstAidKit.OnSkill(playerStatus);
    }
    public void ChargingAttackBuff()
    {
        BuffSkillSetting(equipment.equipment[2], equipSkillList[2]);
        skill_ChargingAttack.OnSkill(playerStatus);
    }
    public void ImmortalBuff()
    {
        BuffSkillSetting(equipment.equipment[2], equipSkillList[2]);
        skill_ImmortalBuff.OnSkill(playerStatus);
    }
    public void ActiveShield()
    {
        BuffSkillSetting(equipment.equipment[2], equipSkillList[2]);
        skill_ImmortalBuff.OnSkill(playerStatus);
    }
    #endregion
    #region 패시브 스킬 코드
    public void FirstAidKit()
    {
        if (equipSkillList[(int)EquipmentType.Gloves].skillName.CompareTo("First aid") == 0 && equipment.equipment[(int)EquipmentType.Gloves].isUsed != true)
        {
            skill_FirstAidKit.OnSkill(playerStatus);
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Gloves], equipSkillList[(int)EquipmentType.Gloves]);
        }
        else if (equipSkillList[(int)EquipmentType.Shoes].skillName.CompareTo("First aid") == 0 && equipment.equipment[(int)EquipmentType.Shoes].isUsed != true)
        {
            skill_FirstAidKit.OnSkill(playerStatus);
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Shoes], equipSkillList[(int)EquipmentType.Shoes]);
        }
    }
    public bool EmergencyEscape()
    {
        if (equipSkillList[(int)EquipmentType.Gloves].skillName.CompareTo("Emergency Escape") == 0 && equipment.equipment[(int)EquipmentType.Gloves].isUsed != true)
        {
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Gloves], equipSkillList[(int)EquipmentType.Gloves]);
            return true;
        }
        else if (equipSkillList[(int)EquipmentType.Shoes].skillName.CompareTo("Emergency Escape") == 0 && equipment.equipment[(int)EquipmentType.Shoes].isUsed != true)
        {
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Shoes], equipSkillList[(int)EquipmentType.Shoes]);
            return true;
        }
        return false;
    }
    public bool AutoDefense()
    {
        if (equipSkillList[(int)EquipmentType.Gloves].skillName.CompareTo("Auto defense") == 0 && equipment.equipment[(int)EquipmentType.Gloves].isUsed != true)
        {
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Gloves], equipSkillList[(int)EquipmentType.Gloves]);
            return true;
        }
        else if (equipSkillList[(int)EquipmentType.Shoes].skillName.CompareTo("Auto defense") == 0 && equipment.equipment[(int)EquipmentType.Shoes].isUsed != true)
        {
            ActiveSkillSetting(equipment.equipment[(int)EquipmentType.Shoes], equipSkillList[(int)EquipmentType.Shoes]);
            return true;
        }
        return false;
    }
    #endregion

    public void ActiveSkillSetting(PlayerEquipment.Equipment _equipment, Skill _skill)
    {
        coolTimeSkillList[(int)_equipment.equipmentType] = _skill;
        skillCoolTime[(int)_equipment.equipmentType] = _skill.skillCoolTime;
        _equipment.isUsed = true;
    }
    public void BuffSkillSetting(PlayerEquipment.Equipment _equipment, Skill _skill)
    {
        liveTimeSkillList[(int)_equipment.equipmentType] = _skill;
        coolTimeSkillList[(int)_equipment.equipmentType] = _skill;
        skillLiveTime[(int)_equipment.equipmentType] = _skill.skillTimeDuration;
        skillCoolTime[(int)_equipment.equipmentType] = _skill.skillCoolTime;
        _equipment.isUsed = true;
    }
}