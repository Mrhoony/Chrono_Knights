using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin : NormalMonsterControl
{
    void OnEnable()
    {
        monsterCode = 3;
        rotateDelayTime = 4f;
        maxAttackDelayTime = 1f;
        isFaceRight = true;
        arrowDirection = 1;
        actionState = ActionState.Idle;
        MonsterInit(monsterCode);
    }
    
    public override void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            return;
        }
        if (actionState != ActionState.Idle) return;

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
    }

    public override void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtk"));
        }
    }
}
