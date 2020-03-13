using System.Collections;
using UnityEngine;

public class Boss_Trainer : BossMonster_Control
{
    float dashAttackCoolTime;
    Material DefaultMat;
    Material WhiteFlashMat;
    
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
        BossMonsterInit(monsterCode);
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
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f)
        {
            Attack1();
        }
        else if(distanceX >= 4f)
        {
            DashMove();
        }
    }

    void Attack1()
    {
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isAttackTrigger");
        StartCoroutine(MoveDelayTime(attackCoolTime));
        Debug.Log("attack");
    }
    public void DashMove()
    {
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isDashTrigger");
        StartCoroutine(DashDuration());
        rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
        Debug.Log("dashMove");
    }
    public void Dash()
    {
        rb.velocity = new Vector2(arrowDirection * moveSpeed * 2f, rb.velocity.y);
        Debug.Log("dash");
    }
    public void DashAttack()
    {
        StopCoroutine(DashDuration());
        animator.SetTrigger("isDashAttackTrigger");
        StartCoroutine(MoveDelayTime(dashAttackCoolTime));
        Debug.Log("dashAttack");
    }

    IEnumerator DashDuration()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("isDash", false);
        animator.SetBool("isDashAttack", false);
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }

    public override void MonsterHit(int damage)
    {
        if (actionState == ActionState.IsDead) return;

        actionState = ActionState.NotMove;
        StopAllCoroutines();
        StartCoroutine(MoveDelayTime(1f));
        random = Random.Range(-2f, 2f);
        rb.velocity = Vector2.zero;

        rb.AddForce(new Vector2(1f * playerPosition + random * 0.1f, 0.2f), ForceMode2D.Impulse);

        enemyStatus.DecreaseHP(damage);
        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            DungeonManager.instance.FloorBossKill();
        }
        else
        {
            animator.SetTrigger("isHit");
            eft.SetActive(true);
        }
    }
}
