using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusInfo : FocusUI
{
    public GameObject[] equipmentSlot;
    public GameObject statusinformation;

    public GameObject ActiveSkill;
    public GameObject[] PassiveSkill;

    public GameObject statusEquipment;
    public Menu_Inventory inventory;

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

    private void Update()
    {
        if (isUIOn)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(3); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-3); }

            if (Input.GetKeyDown(KeyCode.X))    // 포커스 상점으로 변경
            {
                Invoke("PlayerStatusFocusChange", 0.01f);
            }
            if (Input.GetKeyDown(KeyCode.C))    // 포커스 상점으로 변경
            {
                Invoke("PlayerStatusFocusChange", 0.01f);
            }

            FocusMove();
        }
    }

    public void FocusOn()
    {
        focused = 0;
        isUIOn = true;
        EquipmentSlotSetting();
        statusEquipment.SetActive(true);
    }
    public void PlayerStatusFocusChange()
    {
        statusEquipment.SetActive(false);
        cursor.SetActive(false);
        isUIOn = false;
        inventory.FocusOn();
    }

    public void CloseStatusInfo()
    {
        statusEquipment.SetActive(false);
        cursor.SetActive(false);
        isUIOn = false;
    }

    public void EquipmentSkillOptionCheck(PlayerEquipment.Equipment[] _Equipment)
    {
        equipment = _Equipment;
        Dictionary<string, SkillList> skillList = Database_Game.instance.skillManager.skillList;
        passiveSkillList.Clear();

        for (int i = 0; i < 7; ++i)
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

                continue;
            }

            activeSkill = null;

        }
        passiveSkillList = passiveSkillList.Distinct().ToList();

        Debug.Log("EquipmentSkillOptionCheck : " + passiveSkillList.Count);
    }
    public void OnStatusMenu(Menu_Inventory _Inventory)
    {
        isUIOn = false;
        inventory = _Inventory;
        string skillDescription = "";

        for (int i = 0; i < 7; ++i)
        {
            equipmentSlot[i].transform.GetChild(0).gameObject.SetActive(true);
            equipmentSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = SpriteSet.itemSprite[i];
            if (equipment[i].itemCode != 0)
            {
                equipmentSlot[i].transform.GetChild(1).gameObject.SetActive(true);
                equipmentSlot[i].transform.GetChild(1).GetComponent<Image>().sprite = SpriteSet.enchantSlotImage[equipment[i].itemRarity];
            }
            else
            {
                equipmentSlot[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        if(activeSkill != null)
        {
            ActiveSkill.SetActive(true);
            ActiveSkill.GetComponent<Text>().text = activeSkill.skillDescription;
        }

        if(passiveSkillList.Count > 0)
        {
            for (int j = 0; j < passiveSkillList.Count; ++j)
            {
                if (passiveSkillList[j].skill.skillCode != 0)
                {
                    skillDescription = string.Format(
                        passiveSkillList[j].skill.skillDescription + "({0} / {1})",
                        passiveSkillList[j].skillStack, passiveSkillList[j].skill.skillStack);

                    PassiveSkill[j].SetActive(true);
                    PassiveSkill[j].GetComponent<Text>().text = skillDescription;
                }
            }
            for (int k = passiveSkillList.Count; k < 6; ++k)
            {
                PassiveSkill[k].SetActive(false);
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
    public void EquipmentSlotSetting()
    {
        cursor.transform.position = equipmentSlot[focused].transform.position;
        cursor.SetActive(true);
        statusEquipment.transform.position = equipmentSlot[focused].transform.position;

        if (equipment[focused].enchant)
        {
            if (equipment[focused].downStatus == 8)
            {
                statusEquipment.GetComponent<StatusEquipment>().EquipmentStatusInfoSet(
                    playerData.playerEquipment.GetStatusName(equipment[focused].upStatus, true),
                    playerData.playerEquipment.GetUpStatus(focused),
                    "",
                    "");
            }
            else
            {
                statusEquipment.GetComponent<StatusEquipment>().EquipmentStatusInfoSet(
                    playerData.playerEquipment.GetStatusName(equipment[focused].upStatus, true),
                    playerData.playerEquipment.GetUpStatus(focused),
                    playerData.playerEquipment.GetStatusName(equipment[focused].downStatus, false),
                    playerData.playerEquipment.GetDownStatus(focused)
                    );
            }

            if (equipment[focused].skillCode != 0)
            {
                Skill skill = Database_Game.instance.GetSkill(equipment[focused].skillCode);
                statusEquipment.GetComponent<StatusEquipment>().EquipmentSkillInfoSet(skill.skillName, skill.skillDescription);
            }
            else
            {
                statusEquipment.GetComponent<StatusEquipment>().EquipmentSkillInfoSet("", "");
            }
        }
        else
        {
            statusEquipment.GetComponent<StatusEquipment>().EquipmentStatusInfoSet("", "", "", "");
        }
    }

    public void FocusMove()
    {
        cursor.transform.position = Vector2.Lerp(cursor.transform.position, equipmentSlot[focused].transform.position, Time.deltaTime * cursorSpeed);
    }

    public new void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > MaxFocused) { return; }
        if (focused == 2) if (AdjustValue == 3) AdjustValue = 4;
        if (focused == MaxFocused) if (AdjustValue == -3) AdjustValue = -4;
        focused += AdjustValue;
        EquipmentSlotSetting();
    }
}
