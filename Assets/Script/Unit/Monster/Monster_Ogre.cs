﻿using UnityEngine;

public class Monster_Ogre : NormalMonsterControl
{
    void OnEnable()
    {
        monsterCode = 5;
        rotateDelayTime = 3f;
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
            randomAttack = Random.Range(0, 2);
            if (randomAttack > 0)
                StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtkH_Trigger"));
            else
                StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtkV_Trigger"));
        }
    }

    public override void MonsterHit(int damage)
    {
        if (actionState == ActionState.IsDead) return;

        enemyStatus.DecreaseHP(damage);

        if (enemyStatus.IsDeadCheck())
        {
            Dead();
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
        }
        else
        {
            eft.SetActive(true);
        }
    }
}
