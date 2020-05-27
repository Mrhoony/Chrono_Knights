using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RollingBug : NormalMonsterControl
{
    IEnumerator rolling;
    
    public override void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 3f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "Trigger_Attack"));
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
    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;

        enemyStatus.DecreaseHP(_damage);

        SetDamageText(_damage);

        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
            return false;
        }
        else
        {
            StartCoroutine(MonsterHitEffect());
            return true;
        }
    }

    public void Rolling()
    {
        rb.velocity = new Vector2(arrowDirection * moveSpeed * 2f, rb.velocity.y);
    }
    public void RollingCount()
    {
        rolling = RollingDuration();
        StartCoroutine(rolling);
    }
    IEnumerator RollingDuration()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Trigger_Attack_End");
    }
}
