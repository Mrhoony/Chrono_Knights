﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Frog : Monster_Control
{
    public GameObject box;
    
    public void Start()
    {
        maxRotateDelayTime = randomMoveCount;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        arrowDirection = 1;
        isFaceRight = true;
        actionState = ActionState.Idle;
    }
    
    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        Jump();
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        Attack();
    }

    void Jump()
    {
        if (actionState == ActionState.NotMove) return;
        
        if (randomMove != 0)
        {
            actionState = ActionState.NotMove;
            animator.SetTrigger("isJump");
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(1.5f * randomMove, 2f);
            curRotateDelayTime = 0f;

            randomMoveCount = Random.Range(2f, 3f);
            maxRotateDelayTime = randomMoveCount;
        }
    }

    void Attack()
    {
        actionState = ActionState.NotMove;
        curRotateDelayTime = 0f;
        curAttackDelayTime += Time.deltaTime;
        if (curAttackDelayTime > maxAttackDelayTime)
        {
            actionState = ActionState.IsAtk;
            int AttackType = Random.Range(0, 2);
            if (AttackType == 0)
            {
                AttackStart(2f, 3f);
                animator.SetTrigger("isJump");
                animator.SetBool("isJumping", true);
            }
            else if (AttackType == 1)
            {
                AttackStart(4f, 1f);
                animator.SetTrigger("isAttack");
            }
            curAttackDelayTime = 0f;
        }
    }

    public void AttackStart(float x, float y)
    {
        rb.velocity = new Vector2(x * arrowDirection, y);
    }
}