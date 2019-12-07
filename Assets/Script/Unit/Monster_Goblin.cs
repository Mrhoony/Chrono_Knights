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
        isFaceRight = true;
        arrowDirection = 1;
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
        if (actionState == ActionState.IsDead) return;
        Move();
        if (!isTrace) return;
        Attack();
    }

    void Move()
    {
        if (isTrace)
        {
            if (distanceX > 1f)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(ehp.GetMoveSpeed() * arrowDirection, rb.velocity.y);
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
        if (actionState == ActionState.NotMove)
            animator.SetBool("isMove", false);
    }

    void Attack()
    {
        MonsterFlip();

        if (distanceX < 1f)
        {
            actionState = ActionState.NotMove;
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
