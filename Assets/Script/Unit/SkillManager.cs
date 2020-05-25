using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SkillList
{
    public Skill skill;
    public int skillStack;
    public bool isUsed;

    public SkillList(Skill _Skill, int _SkillStack)
    {
        skill = _Skill;
        skillStack = _SkillStack;
        isUsed = false;
    }
}
public class SkillManager : MonoBehaviour
{
    public Dictionary<string, SkillList> skillList;
    public string activeSkillName;

    public PlayerStatus playerStatus;
    public PlayerEquipment equipment;

    #region SkillClassList
    
    AuraSpear skill_AuraSpear = new AuraSpear();
    ChargingAttack skill_ChargingAttack = new ChargingAttack();
    ImmortalBuff skill_ImmortalBuff = new ImmortalBuff();
    ActiveShield skill_ActiveShield = new ActiveShield();

    #endregion

    public void Init(PlayerStatus _playerStatus)
    {
        playerStatus = _playerStatus;
        equipment = playerStatus.playerData.playerEquipment;
        skillList = new Dictionary<string, SkillList>();
        
        SkillListInit();
    }
    public void SkillListInit()
    {
        skillList.Clear();
        activeSkillName = "";

        Debug.Log("skill list init");
    }
    public void EquipmentSkillSetting(PlayerEquipment.Equipment[] _Equipment)
    {
        SkillListInit();
        Skill skill;

        for (int i = 0; i < 7; ++i)
        {
            skill = Database_Game.instance.GetSkill(_Equipment[i].skillCode);

            if (skill == null) continue;

            if (i == 2)
            {
                activeSkillName = skill.skillName;
                continue;
            }

            if (skillList.ContainsKey(skill.skillName))
            {
                ++skillList[skill.skillName].skillStack;
                if (skillList[skill.skillName].skillStack > skillList[skill.skillName].skill.skillStack)
                {
                    skillList[skill.skillName].skillStack = skillList[skill.skillName].skill.skillStack;
                }
            }
            else
            {
                skillList.Add(skill.skillName, new SkillList(skill, 1));
            }
            Debug.Log("skill list init" + skillList.Count);
        }

    }
    
    public void SkillCoolTimeReset()
    {
        StopCoroutine("LiveTimeCheckCoroutine");
        StopCoroutine("CoolTimeCheckCoroutine");

        foreach (string key in skillList.Keys)
        {
            skillList[key].isUsed = false;
        }
    }
    public void ActiveSkillUse(GameObject _Player)
    {
        
        if (!skillList.ContainsKey(activeSkillName)) return;
        if (skillList[activeSkillName].isUsed) return;

        switch (skillList[activeSkillName].skill.skillCode)
        {
            case 101:
                ActiveAuraSpearBuff(skillList[activeSkillName]);
                break;
            case 102:
                LaserAttack(skillList[activeSkillName], _Player);
                break;
            case 103:
                AreaAttackSquare(skillList[activeSkillName], _Player);
                break;
            case 104:
                ChargingAttackBuff(skillList[activeSkillName]);
                break;
            case 105:
                ImmortalBuff(skillList[activeSkillName]);
                break;
            case 106:
                ActiveShield(skillList[activeSkillName]);
                break;
            case 107:
                ResupplyBullet(skillList[activeSkillName]);
                break;
            case 108:
                Healling(skillList[activeSkillName]);
                break;
            case 109:
                Blink(skillList[activeSkillName], _Player);
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
    public void LaserAttack(SkillList _ActiveSkill, GameObject _Player)
    {
        // laserattack 에서 오브젝트 온
        ContinuousSkillSetting(_ActiveSkill);
    }
    public void AreaAttackSquare(SkillList _ActiveSkill, GameObject _Player)
    {
        ContinuousSkillSetting(_ActiveSkill);
    }
    public void ResupplyBullet(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
        playerStatus.Resupply_Ammo(5);
    }
    public void Healling(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
        playerStatus.IncreaseHP(10);
    }
    public void Blink(SkillList _ActiveSkill, GameObject _Player)
    {
        ContinuousSkillSetting(_ActiveSkill);
    }
    #endregion
    #region 지속 스킬 코드
    public void ActiveAuraSpearBuff(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
        skill_AuraSpear.OnSkill(playerStatus);
    }
    public void ChargingAttackBuff(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
        skill_ChargingAttack.OnSkill(playerStatus);
    }
    public void ImmortalBuff(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
        skill_ImmortalBuff.OnSkill(playerStatus);
    }
    public void ActiveShield(SkillList _ActiveSkill)
    {
        ContinuousSkillSetting(_ActiveSkill);
    }
    #endregion
    #region 패시브 스킬 코드
    public void FirstAidKit()
    {
        playerStatus.IncreaseHP(10 * GetPassiveSkillStack("First aid"));
        Debug.Log("응급처치");
    }
    public int EmergencyEscape()
    {
        Debug.Log("긴급 회피 스택" + GetPassiveSkillStack("Emergency Escape"));
        return GetPassiveSkillStack("Emergency Escape");
    }
    public int AutoDefense()
    {
        Debug.Log(GetPassiveSkillStack("오토 가드 스택" + "Auto defense"));
        return GetPassiveSkillStack("Auto defense");
    }
    #endregion

    public void ImmediateActivateSkillSetting(SkillList _skill)
    {
        StartCoroutine(CoolTimeCheckCoroutine(_skill));
    }
    public void ContinuousSkillSetting(SkillList _skill)
    {
        StartCoroutine(LiveTimeCheckCoroutine(_skill));
        StartCoroutine(CoolTimeCheckCoroutine(_skill));
    }

    public int GetPassiveSkillStack(string _PassiveSkillName)
    {
        int passiveSkillStack;
        if (skillList.ContainsKey(_PassiveSkillName))
        {
            passiveSkillStack = skillList[_PassiveSkillName].skillStack;
            return passiveSkillStack;
        }
        return 0;
    }
    
    IEnumerator LiveTimeCheckCoroutine(SkillList _skill)
    {
        float currentTime = 0f;
        while (true)
        {
            if (DungeonManager.instance.isSceneLoading) continue;

            currentTime += Time.deltaTime;
            if(currentTime > _skill.skill.skillTimeDuration)
            {
                SkillReset(_skill.skill.skillCode);
                break;
            }
            yield return null;
        }
    }
    IEnumerator CoolTimeCheckCoroutine(SkillList _skill)
    {
        float currentTime = 0f;
        while (true)
        {
            if (DungeonManager.instance.isSceneLoading) continue;

            currentTime += Time.deltaTime;
            if (currentTime > _skill.skill.skillCoolTime)
            {
                _skill.isUsed = false;
                break;
            }
            yield return null;
        }
    }
}