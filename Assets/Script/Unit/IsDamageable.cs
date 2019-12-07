using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamageable : MonoBehaviour
{
    public void PlayerHit(int attack)
    {
        gameObject.GetComponent<PlayerControl>().Hit(attack);
    }

    public void MonsterHit(int attack)
    {
        gameObject.GetComponent<Monster_Control>().MonsterHit(attack);
    }
}
