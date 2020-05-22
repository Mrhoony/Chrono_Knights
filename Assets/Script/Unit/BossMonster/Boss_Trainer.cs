﻿using System.Collections;
using UnityEngine;

public class Boss_Trainer : BossMonster_Control
{
    IEnumerator dashCount;
    float dashAttackCoolTime;
    
    private void OnEnable()
    {
        rotateDelayTime = 4f;
        attackCoolTime = 5f;
        dashAttackCoolTime = 7f;

        arrowDirection = 1;

        Invincible = false;
        isGuard = false;
        monsterCode = 1002;
        MonsterInit();

        actionState = ActionState.NotMove;
        StartCoroutine(MoveDelayTime(2f));
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        MonsterFlip();
        Move();
        Attack();
    }

    public void Move()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX >= 1f && distanceX < 4)
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
            Debug.Log("Move");
        }
        else
        {
            animator.SetBool("isMove", false);
            Debug.Log("notMove");
        }
    }

    void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f)
        {
            Attack1();
            Debug.Log("Attack");
        }
        else if(distanceX >= 4f)
        {
            DashMove();
            Debug.Log("Dash");
        }
    }

    void Attack1()
    {
        actionState = ActionState.IsAtk;
        animator.SetBool("isDash", true);
        animator.SetTrigger("isAttack_Trigger");
        moveDelayCount = MoveDelayTime(attackCoolTime);
        StartCoroutine(moveDelayCount);
    }
    void DashMove()
    {
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isDash_Trigger");
        dashCount = DashDuration();
        StartCoroutine(dashCount);
    }
    public void Dash()
    {
        rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
    }
    public void DashAttack()
    {
        StopCoroutine(dashCount);
        animator.SetTrigger("isDashAttack_Trigger");
        moveDelayCount = MoveDelayTime(dashAttackCoolTime);
        StartCoroutine(moveDelayCount);
        Debug.Log("DashAtk");
    }

    IEnumerator DashDuration()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("isDash", false);
        moveDelayCount = MoveDelayTime(attackCoolTime);
        StartCoroutine(moveDelayCount);
        Debug.Log("DashEnd");
    }

    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;

        actionState = ActionState.NotMove;
        StartCoroutine(MoveDelayTime(1f));
        random = Random.Range(-0.2f, 0.2f);

        enemyStatus.DecreaseHP(_damage);
        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
            DungeonManager.instance.dungeonMaker.FloorBossKill();
            return false;
        }
        else
        {
            animator.SetTrigger("isHit_Trigger");
            eft.SetActive(true);
            return true;
        }
    }
    public override void MonsterHitRigidbodyEffectKnockBack(int _knockBack)
    {
        int knockBack = _knockBack - monsterWeight;
        if (knockBack > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2((PlayerControl.instance.GetArrowDirection() + random) * knockBack * 0.5f, 1f), ForceMode2D.Impulse);
        }
    }
    public override void MonsterHitRigidbodyEffectUpper(int _knockBack)
    {
        int knockBack = _knockBack - monsterWeight;
        if (knockBack > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2((PlayerControl.instance.GetArrowDirection() + random) * 0.5f, knockBack), ForceMode2D.Impulse);
        }
    }
}
