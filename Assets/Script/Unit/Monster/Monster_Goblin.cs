using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin : Monster_Control
{
    void Start()
    {
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        Move();
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        Attack();
    }

    void Move()
    {
        if (actionState == ActionState.NotMove) return;
        if (isTrace)
        {
            if (distanceX > 1f)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * arrowDirection, rb.velocity.y);
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
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * randomMove, rb.velocity.y);
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
        if (distanceX < 1f)
        {
            actionState = ActionState.NotMove;
            rb.velocity = Vector2.zero;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                actionState = ActionState.IsAtk;
                animator.SetTrigger("isAtk");
                curRotateDelayTime = 0f;
                curAttackDelayTime = 0f;
            }
        }
    }
}
