using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public EnemyStatus allocatedMonster;
    public GameObject HPBarGauge;
    public Text monsterName;
    
    public void SetMonster(EnemyStatus monster)
    {
        allocatedMonster = monster;
        monsterName.text = monster.monsterName;
        SetHPBar();
    }

    public void MonsterDie()
    {
        allocatedMonster = null;
    }

    public void SetHPBar()
    {
        if(allocatedMonster._currentHP <= 0)
        {
            allocatedMonster._currentHP = 0;
        }
        Vector3 scale = HPBarGauge.transform.localScale;
        scale.x = allocatedMonster._currentHP / allocatedMonster._HP;
        HPBarGauge.transform.localScale = scale;
    }
}
