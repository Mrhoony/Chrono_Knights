using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Image HPBarGauge;
    public float HPgauge;
    
    public void SetHPBar(float monsterCurrentHP, int monsterHP)
    {
        HPBarGauge.fillAmount = 1 - (monsterCurrentHP / monsterHP);
    }
}
