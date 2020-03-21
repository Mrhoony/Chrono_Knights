using System.Collections;
using UnityEngine;

public class Boss_Trainer : BossMonster_Control
{
    IEnumerator dashCount;
    float dashAttackCoolTime;
    
    private void OnEnable()
    {
        arrowDirection = 1;
        isFaceRight = true;

        rotateDelayTime = 4f;
        attackCoolTime = 5f;
        dashAttackCoolTime = 7f;
        
        Invincible = false;
        isGuard = false;
        monsterCode = 1002;
        MonsterInit();
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

    public override void MonsterHit(int damage)
    {
        if (actionState == ActionState.IsDead) return;

        actionState = ActionState.NotMove;
        StartCoroutine(MoveDelayTime(1f));
        random = Random.Range(-0.2f, 0.2f);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1f * playerPosition + random, 0.2f), ForceMode2D.Impulse);

        enemyStatus.DecreaseHP(damage);
        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
            DungeonManager.instance.FloorBossKill();
        }
        else
        {
            animator.SetTrigger("isHit");
            eft.SetActive(true);
        }
    }
}
