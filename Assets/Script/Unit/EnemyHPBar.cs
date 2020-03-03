using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public EnemyStatus allocatedMonster;
    public GameObject HPBarGauge;
    public bool isUsed = false;
    public Text monsterName;
    
    public bool SetMonster(EnemyStatus monster)
    {
        if (isUsed) return false;
        else
        {
            isUsed = true;
            allocatedMonster = monster;
            monsterName.text = monster.monsterName;
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
        Vector3 scale = HPBarGauge.transform.localScale;
        scale.x = allocatedMonster._currentHP / allocatedMonster._HP;
        HPBarGauge.transform.localScale = scale;
    }
}
