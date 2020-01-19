using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bossMonsterList
{
    merchant = 1,
    trainer,
    enchanter
}

public class IsDamageable : MonoBehaviour
{
    public int bossNumber;

    public void PlayerHit(int attack)
    {
        gameObject.GetComponent<PlayerControl>().Hit(attack);
    }

    public void MonsterHit(int attack)
    {
        gameObject.GetComponent<Monster_Control>().MonsterHit(attack);
    }

    public void BossMonsterHit(int attack)
    {
        switch (bossNumber)
        {
            case 1:
                gameObject.GetComponent<Boss_Merchant>().MonsterHit(attack);
                break;
            case 2:
                gameObject.GetComponent<Boss_Trainer>().MonsterHit(attack);
                break;
        }
    }
}
