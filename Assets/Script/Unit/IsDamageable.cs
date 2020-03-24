using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamageable : MonoBehaviour
{
    public void Hit(int _attack, int _knockBack)
    {
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<PlayerControl>().Hit(_attack);
        }
        else if (gameObject.CompareTag("Monster"))
        {
            gameObject.GetComponent<Monster_Control>().MonsterHit(_attack, _knockBack);
        }
        else if (gameObject.CompareTag("BossMonster"))
        {
            gameObject.GetComponent<Monster_Control>().MonsterHit(_attack, _knockBack);
        }
    }
}
