﻿using UnityEngine;

public class Monster_Ogre : Monster_Control
{
    void Start()
    {
        rotateDelayTime = 2f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
        actionState = ActionState.Idle;
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        Move();
        if (actionState == ActionState.NotMove) return;
        Attack();
    }

    void Move()
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
                randomAttack = Random.Range(0, 2);
                if (randomAttack > 0)
                    animator.SetTrigger("isAtkH_Trigger");
                else
                    animator.SetTrigger("isAtkV_Trigger");
                curAttackDelayTime = 0f;
                StartCoroutine(MoveDelayTime(rotateDelayTime));
            }
        }
    }
}
