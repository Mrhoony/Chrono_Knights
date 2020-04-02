using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonTrialSlot : MonoBehaviour
{
    int statusRandom;
    int valueRandom;
    public Text trialName;

    public void SetTrialCard()
    {
        statusRandom = Random.Range(0, 3);
        valueRandom = Random.Range(1, 3);

        switch (statusRandom)
        {
            case 0:
                trialName.text = "몬스터 공격력 " + valueRandom + " 증가";
                break;
            case 1:
                trialName.text = "몬스터 방어력 " + valueRandom + " 증가";
                break;
            case 2:
                trialName.text = "몬스터 체력 " + valueRandom + "배 증가";
                break;
        }
    }

    public void SelectThisCard()
    {
        DungeonManager.instance.SetTrialStack(statusRandom, valueRandom);
    }
}