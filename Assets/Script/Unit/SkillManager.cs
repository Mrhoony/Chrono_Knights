using System.Collections;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public PlayerStatus playerStatus;
    public Skill[] buffSkillList;
    public Skill skill;
    public IEnumerator[] skillBuffDurationCheck;
    public IEnumerator[] skillCoolTimeCheck;

    /*
     * 
     * 스킬의 이름과 타입, 스킬 코드는 클래스로 저장
     * 
     * 스킬 타입별, 번호별로 함수 만들기
     * 
     * 장비에 등록된 아이템 레어리티에 따라 효과 배율 저장
     * 
     * */

    public void Init()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        buffSkillList = new Skill[7];
        
        skillBuffDurationCheck = new IEnumerator[7];
        skillCoolTimeCheck = new IEnumerator[7];
        for (int i = 0; i < 7; ++i)
        {
            buffSkillList[i] = null;
        }
    }

    public void SkillCheck(PlayerEquipment.Equipment equipment)
    {
        if(equipment.isUsed) return;

        switch (equipment.skillCode)
        {
            case 100:
            case 101:
            case 102:
            case 103:
            case 104:
            case 105:
                SetBuffStatus(equipment);
                break;
        }
    }

    public void SetBuffStatus(PlayerEquipment.Equipment equipment)
    {
        for (int i = 0; i < 7; ++i)
        {
            if (buffSkillList[i] == skill)
            {
                BuffSkillSetting(i, equipment);
                break;
            }
        }
    }
    public void BuffSkillSetting(int i, PlayerEquipment.Equipment equipment)
    {
        buffSkillList[i] = skill;

        if(skillBuffDurationCheck[i] != null)
        {
            StopCoroutine(skillBuffDurationCheck[i]);
            StopCoroutine(skillCoolTimeCheck[i]);
            Debug.Log("실행중인 버프 종료");
        }

        skillBuffDurationCheck[i] = BuffDuration(skill.skillTimeDuration, i);
        StartCoroutine(skillBuffDurationCheck[i]);
        skillCoolTimeCheck[i] = SkillCoolTime(skill.skillCoolTime, equipment, i);
        StartCoroutine(skillCoolTimeCheck[i]);
        equipment.isUsed = true;
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
    public IEnumerator SkillCoolTime(float coolTime, PlayerEquipment.Equipment equipment, int i)
    {
        yield return new WaitForSeconds(coolTime);
        // 쿨타임 종료
        equipment.isUsed = false;
        Debug.Log("BuffSkillOn");
    }
}