using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RollingBug : NormalMonsterControl
{
    IEnumerator rolling;

    void OnEnable()
    {
        rotateDelayTime = 4f;
        maxAttackDelayTime = 1f;
        arrowDirection = 1;
        actionState = ActionState.Idle;

        MonsterInit();
    }


    public override void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 3f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtk_Trigger"));
        }
    }
    public override void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            return;
        }

        if (isTrace)
        {
            if (distanceX > 1f)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * 2f * arrowDirection, rb.velocity.y);
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

    public void Rolling()
    {
        rb.velocity = new Vector2(arrowDirection * moveSpeed * 3f, rb.velocity.y);
    }
    public void RollingCount()
    {
        rolling = RollingDuration();
        StartCoroutine(rolling);
    }
    IEnumerator RollingDuration()
    {
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("isAtk_End_Trigger");
    }
}
