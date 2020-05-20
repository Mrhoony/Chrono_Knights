using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusInfo : MonoBehaviour
{
    public GameObject[] equipmentSlot;
    public GameObject statusinformation;

    public GameObject ActiveSkill;
    public GameObject[] PassiveSkill;

    public Skill activeSkill;
    public List<SkillList> passiveSkillList = new List<SkillList>();

    PlayerStatus playerStatus;
    PlayerData playerData;
    public PlayerEquipment.Equipment[] equipment;

    private void Awake()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStatus.playerData;
    }

    public void EquipmentSkillOptionCheck(PlayerEquipment.Equipment[] _Equipment)
    {
        equipment = _Equipment;
        Dictionary<string, SkillList> skillList = Database_Game.instance.skillManager.skillList;
        for(int i = 0; i < 7; ++i)
        {
            if(equipment[i].skillCode != 0)
            {
                Skill skill = Database_Game.instance.GetSkill(equipment[i].skillCode);
                if(i == 2)
                {
                    activeSkill = skill;
                    continue;
                }

                passiveSkillList.Add(skillList[skill.skillName]);
            }
        }
        passiveSkillList = passiveSkillList.Distinct().ToList();
    }

    public void OnStatusMenu()
    {
        for (int i = 0; i < 7; ++i)
        {
            equipmentSlot[i].transform.GetChild(0).gameObject.SetActive(true);
            equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = SpriteSet.itemSprite[i];
            if (equipment[i].itemCode != 0)
            {
                equipmentSlot[i].transform.GetChild(1).gameObject.SetActive(true);
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.enchantSlotImage[equipment[i].itemRarity];
                if(equipment[i].skillCode != 0)
                {
                    if(i == 2)
                    {
                        ActiveSkill.SetActive(true);
                        ActiveSkill.GetComponent<Text>().text = activeSkill.skillDescription;
                    }
                    else
                    {
                        string skillDescription = "";
                        Skill skill = Database_Game.instance.GetSkill(equipment[i].skillCode);
                        
                        for(int j = 0; j < passiveSkillList.Count; ++j)
                        {
                            skillDescription = string.Format(passiveSkillList[j].skill.skillDescription + "({0} / {1})", passiveSkillList[j].skillStack, passiveSkillList[j].skill.skillStack);

                            PassiveSkill[j].SetActive(true);
                            PassiveSkill[j].GetComponent<Text>().text = skillDescription;
                        }
                    }
                }
            }
            else
            {
                equipmentSlot[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        statusinformation.transform.GetChild(0).GetComponent<Text>().text = playerStatus.GetAttack_Result().ToString();
        statusinformation.transform.GetChild(1).GetComponent<Text>().text = playerStatus.GetDefence_Result().ToString();
        statusinformation.transform.GetChild(2).GetComponent<Text>().text = playerStatus.GetMoveSpeed_Result().ToString();
        statusinformation.transform.GetChild(3).GetComponent<Text>().text = playerStatus.GetAttackSpeed_Result().ToString();
        statusinformation.transform.GetChild(4).GetComponent<Text>().text = playerStatus.GetDashDistance_Result().ToString();
        statusinformation.transform.GetChild(5).GetComponent<Text>().text = playerStatus.GetRecovery_Result().ToString();
        statusinformation.transform.GetChild(6).GetComponent<Text>().text = playerStatus.jumpCount.ToString();
        statusinformation.transform.GetChild(7).GetComponent<Text>().text = playerData.GetStatus(7).ToString();
    }
}
