using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Frog : NormalMonsterControl
{
    public GameObject box;
    
    public void Start()
    {
        rotateDelayTime = 2f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        arrowDirection = 1;
        isFaceRight = true;
        actionState = ActionState.Idle;
    }
    
    private new void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState == ActionState.IsAtk) return;
        Move();
        if (!isTrace) return;
        Attack();
    }

    public override void Move()
    {
        if (actionState == ActionState.NotMove) return;
        
        if (randomMove != 0)
        {
            actionState = ActionState.NotMove;
            animator.SetTrigger("isJump");
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(1.5f * randomMove, 2f);
            StartCoroutine(MoveDelayTime(rotateDelayTime));
        }
    }

    public override void Attack()
    {
        actionState = ActionState.NotMove;
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
            StartCoroutine(MoveDelayTime(rotateDelayTime));
            curAttackDelayTime = 0f;
        }
    }

    public void AttackStart(float x, float y)
    {
        rb.velocity = new Vector2(x * arrowDirection, y);
    }
}
