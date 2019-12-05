using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin : Monster_Control
{
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        effectX = 0.5f;
        effectY = 0.5f;
        isFaceRight = true;
        arrow = 1;
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Move();
        if (!isTrace) return;
        Attack();
    }

    void Move()
    {
        if (!notMove && !isAtk)
        {
            if (isTrace)
            {
                if (distanceX > 1f)
                {
                    animator.SetBool("isMove", true);
                    rb.velocity = new Vector2(ehp.GetMoveSpeed() * arrow, rb.velocity.y);
                }
                else
                {
                    animator.SetBool("isMove", false);
                }
            }
            else
            {
                if (randomMove != 0)
                {
                    animator.SetBool("isMove", true);
                    rb.velocity = new Vector2(ehp.GetMoveSpeed() * randomMove, rb.velocity.y);
                }
                else
                {
                    animator.SetBool("isMove", false);
                }
            }
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    void Attack()
    {
        MonsterFlip();

        if (distanceX < 1f)
        {
            notMove = true;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                isAtk = true;
                animator.SetTrigger("isAtk");
                curRotateDelayTime = 0f;
                curAttackDelayTime = 0f;
            }
        }
    }
}
