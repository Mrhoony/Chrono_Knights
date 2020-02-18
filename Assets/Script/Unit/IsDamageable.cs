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
    
    public void Hit(int attack)
    {
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<PlayerControl>().Hit(attack);
        }
        else if(gameObject.CompareTag("Monster"))
        {
            gameObject.GetComponent<Monster_Control>().MonsterHit(attack);
        }
        else
        {
            gameObject.GetComponent<BossMonster_Control>().MonsterHit(attack);
        }
    }
}
