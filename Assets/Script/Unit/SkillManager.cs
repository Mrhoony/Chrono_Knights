using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public int[] buffList;
    public int[] buffRarity;
    public float[] buffTime;
    public float[] buffCoolTime;
    public Skill skill;
    public IEnumerator[] skillCoolTimeCheck;

    /*
     * 스킬의 이름과 타입, 스킬 코드는 클래스로 저장
     * 
     * 스킬 타입별, 번호별로 함수 만들기
     * 
     * 장비에 등록된 아이템 레어리티에 따라 효과 배율 저장
     * 
     * 1 ~ 7 입력으로 장비에 저장된 스킬 사용
     * */

    public void Init()
    {
        buffList = new int[7];
        buffTime = new float[7];
        buffCoolTime = new float[7];
        skillCoolTimeCheck = new IEnumerator[7];
        for (int i = 0; i < 7; ++i)
        {
            buffList[i] = 0;
            buffTime[i] = 0;
            buffCoolTime[i] = 0;
        }
    }

    public void SkillCheck(PlayerEquipment.Equipment equipment)
    {
        switch (equipment.skillCode)
        {
            case 100:
            case 101:
            case 102:
            case 103:
            case 104:
            case 105:
            case 106:
                SetStatBuff(equipment);
                break;
        }
    }

    public void SetStatBuff(PlayerEquipment.Equipment equipment)
    {
        skill = Database_Game.instance.CheckSkill(equipment.skillCode);
        for (int i = 0; i < 7; ++i)
        {
            if (buffList[i] == skill.skillCode)
            {
                if (buffRarity[i] <= equipment.skillRarity)
                {
                    BuffSkillSetting(i, equipment);
                    break;
                }
            }
            else if (buffList[i] == 0)
            {
                BuffSkillSetting(i, equipment);
                break;
            }
        }
    }

    public void BuffSkillSetting(int i, PlayerEquipment.Equipment equipment)
    {
        buffList[i] = skill.skillCode;
        buffRarity[i] = equipment.skillRarity;
        skillCoolTimeCheck[i] = BuffDuration(skill.skillTimeDuration, skill.skillCoolTime, equipment);
        StartCoroutine(skillCoolTimeCheck[i]);
        equipment.isUsed = true;
    }

    public IEnumerator BuffDuration(float duraionTime, float coolTime, PlayerEquipment.Equipment equipment)
    {
        //버프 활성화
        Debug.Log("BuffSkillUse");
        yield return new WaitForSeconds(duraionTime);
        //버프 종료
        Debug.Log("BuffSkillEnd");
        yield return new WaitForSeconds(coolTime - duraionTime);
        // 쿨타임 종료
        equipment.isUsed = false;
        Debug.Log("BuffSkillOn");
    }
}