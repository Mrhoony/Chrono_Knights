using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEquipment : MonoBehaviour
{
    public Text plusName;
    public Text plusStat;
    public Text minusName;
    public Text minusStat;
    public Text skillName;
    public Text skillDescription;
    
    public void EquipmentStatusInfoSet(string _PlusName, string _PlusStat, string _MinusName, string _MinusStat)
    {
        plusName.text = _PlusName;
        plusStat.text = _PlusStat;
        minusName.text = _MinusName;
        minusStat.text = _MinusStat;
    }
    public void EquipmentSkillInfoSet(string _SkillName, string _SkillDescription)
    {
        skillName.text = _SkillName;
        skillDescription.text = _SkillDescription;
    }
}
