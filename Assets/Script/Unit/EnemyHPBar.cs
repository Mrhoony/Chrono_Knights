using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public EnemyStatus allocatedMonster;
    public GameObject HPBarGauge;
    public bool isUsed = false;
    public Text monsterName;
    public Text monsterCurrentHP;
    
    public bool SetMonster(EnemyStatus monster)
    {
        if (isUsed) return false;
        else
        {
            isUsed = true;
            allocatedMonster = monster;
            return true;
        }
    }

    public void MonsterDie()
    {
        allocatedMonster = null;
        isUsed = false;
    }

    public void SetHPBar()
    {
        Vector2 scale = HPBarGauge.transform.localScale;
        scale.x = (float)allocatedMonster.GetCurrentHP() / allocatedMonster.GetHP();
        HPBarGauge.transform.localScale = scale;
    }
}
