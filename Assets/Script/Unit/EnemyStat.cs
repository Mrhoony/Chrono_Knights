using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    public float HP; // 최대 체력
    public float currentHP; // 현재 체력
    public int Atk; // 공격력
    public float jumpPower;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    

    public void IncreaseHP(int damage)
    {
        currentHP += damage;
        if(damage > HP)
        {
            currentHP = HP;
        }
    }

    public void DecreaseHP(int damage)
    {
        currentHP = currentHP - damage;
        if (currentHP <= 0)
        {
            anim.SetTrigger("isDead");
        }
        anim.SetTrigger("isHit");
    }
}
