﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamageable : MonoBehaviour
{
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
        else if (gameObject.CompareTag("BossMonster"))
        {
            gameObject.GetComponent<Monster_Control>().MonsterHit(attack);
        }
    }
}
