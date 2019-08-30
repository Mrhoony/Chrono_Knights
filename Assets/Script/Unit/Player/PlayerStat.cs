using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float moveSpeed; // 이동 속도
    public float HP; // 최대 체력
    public float currentHP; // 현재 체력
    public int Atk; // 공격력
    public float jumpPower;
    public float defense; // 방어력
    public float stability; // 안정성

    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init()
    {
        currentHP = HP;
    }

    public void DecreaseHP(int Atk)
    {
        currentHP -= Atk;

        if (currentHP <= 0)
        {
            Debug.Log("isDead");
        }
        else
        {
            animator.SetTrigger("isHit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
