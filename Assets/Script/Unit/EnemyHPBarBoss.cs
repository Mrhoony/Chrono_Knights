using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarBoss : EnemyHPBar
{
    public Text monsterCurrentHP;

    public new void SetHPBar()
    {
        Vector2 scale = HPBarGauge.transform.localScale;
        scale.x = allocatedMonster._currentHP / allocatedMonster._HP;
        monsterCurrentHP.text = allocatedMonster._currentHP.ToString() + " + " + allocatedMonster._HP.ToString();
        HPBarGauge.transform.localScale = scale;
    }
}
