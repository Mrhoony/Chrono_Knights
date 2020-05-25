using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Dokkaebi : NormalMonsterControl
{
    void OnEnable()
    {
        rotateDelayTime = 4f;
        maxAttackDelayTime = 1f;
        arrowDirection = 1;
        actionState = ActionState.Idle;

        MonsterInit();
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
            if (distanceX > 2f)
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

        if (distanceX < 2f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "Trigger_Attack"));
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
}
