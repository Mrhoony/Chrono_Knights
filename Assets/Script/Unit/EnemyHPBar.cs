using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public GameObject HPBarGauge;
    public bool isUsed = false;
    
    public bool SetMonster()
    {
        if (isUsed)
            return false;
        else
        {
            isUsed = true;
            return true;
        }
    }

    public void SetHPBar(float monsterCurrentHP, int monsterHP)
    {
        Vector2 scale = HPBarGauge.transform.localScale;
        scale.x = monsterCurrentHP / monsterHP;
        HPBarGauge.transform.localScale = scale;
    }
}
