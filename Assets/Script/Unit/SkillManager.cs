using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public PlayerStatus playerStatus;
    public Skill[] buffSkillList;
    public Skill[] equipSkillList;
    public IEnumerator[] skillBuffDurationCheck;
    public IEnumerator[] skillCoolTimeCheck;

    public void Init()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();

        equipSkillList = new Skill[7];
        buffSkillList = new Skill[7];
        skillBuffDurationCheck = new IEnumerator[7];
        skillCoolTimeCheck = new IEnumerator[7];
        for (int i = 0; i < 7; ++i)
        {
            equipSkillList[i] = null;
            buffSkillList[i] = null;
        }
    }

    public bool UseActiveSkill()
    {
        switch (equipSkillList[2].skillCode)
        {
            case 101:
                ActiveAuraSpearBuff();
                break;
            case 102:
                LaserAttack();
                break;
            case 103:
                AreaAttacksquare();
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
                Blink();
                break;
            case 110:
                break;
        }
        return true;
    }

    #region 스킬 코드
    public void ActiveAuraSpearBuff()
    {

    }
    public void LaserAttack()
    {

    }
    public void AreaAttacksquare()
    {

    }
    public void ChargingAttackBuff()
    {

    }
    public void ImmortalBuff()
    {

    }
    public void ActiveShield()
    {

    }
    public void ResupplyBullet()
    {

    }
    public void Healling()
    {

    }
    public void Blink()
    {

    }

    public void EmergencyEscape()
    {

    }
    public void AutoDefense()
    {

    }
    public void FirstAidKit()
    {

    }
    #endregion
    public void SkillSetting(Skill _skill)
    {
        buffSkillList[0] = _skill;

        if(skillBuffDurationCheck[0] != null)
        {
            StopCoroutine(skillBuffDurationCheck[0]);
            StopCoroutine(skillCoolTimeCheck[0]);
            Debug.Log("실행중인 버프 종료");
        }

        skillBuffDurationCheck[0] = BuffDuration(_skill.skillTimeDuration, 0);
        StartCoroutine(skillBuffDurationCheck[0]);
        skillCoolTimeCheck[0] = SkillCoolTime(_skill.skillCoolTime, 0);
        StartCoroutine(skillCoolTimeCheck[0]);
        //equipment.isUsed = true;
    }
    public IEnumerator BuffDuration(float duraionTime, int i)
    {
        //버프 활성화
        Debug.Log("BuffSkillUse");
        yield return new WaitForSeconds(duraionTime);
        //버프 종료
        buffSkillList[i] = null;
        Debug.Log("BuffSkillEnd");
    }
    public IEnumerator SkillCoolTime(float coolTime, int i)
    {
        yield return new WaitForSeconds(coolTime);
        // 쿨타임 종료
        //equipment.isUsed = false;
        Debug.Log("BuffSkillOn");
    }
}