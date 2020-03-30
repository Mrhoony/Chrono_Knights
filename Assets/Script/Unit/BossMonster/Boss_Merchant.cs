using System.Collections;
using UnityEngine;

public class Boss_Merchant : BossMonster_Control
{
    float backAttackCoolTime;
    
    private void OnEnable()
    {
        rotateDelayTime = 2f;
        attackCoolTime = 6f;
        backAttackCoolTime = 4f;
        arrowDirection = 1;
        isGuard = false;
        isFaceRight = true;
        Invincible = false;
        monsterCode = 1001;
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

    void Move()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f)
        {
            actionState = ActionState.NotMove;
            StartCoroutine(MoveDelayTime(rotateDelayTime));
            animator.SetTrigger("isDodgeTrigger");
            rb.velocity = new Vector2(moveSpeed * -arrowDirection * 2, rb.velocity.y + 2f);
        }
    }
    void Attack()
    {
        if (actionState != ActionState.Idle) return;

        counter = Random.Range(0, 10);
        if(counter < 1)
        {
            Guard();
        }
        else
        {
            if (distanceX <= 3f)
                Attack1();
            else
                Attack2();
        }
    }
    
    void Attack1()
    {
        actionState = ActionState.IsAtk;
        MonsterFlip();
        animator.SetTrigger("isAttack1Trigger");
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }
    void Attack2()
    {
        actionState = ActionState.IsAtk;
        transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -1f, target.transform.position.y);
        MonsterFlip();
        animator.SetTrigger("isAttack2Trigger");
        StartCoroutine(MoveDelayTime(backAttackCoolTime));
    }
    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;

        if (isGuard)
        {
            StopCoroutine("GuardCount");
            StopCoroutine("MoveDelayTime");
            animator.SetBool("isCounterAttack", true);
            return false;
        }
        else
        {
            actionState = ActionState.NotMove;
            enemyStatus.DecreaseHP(_damage);
            StartCoroutine(MoveDelayTime(1f));

            if (enemyStatus.IsDeadCheck())
            {
                actionState = ActionState.IsDead;
                gameObject.tag = "DeadBody";
                Dead();
                DungeonManager.instance.FloorBossKill();
                return false;
            }
            else
            {
                animator.SetTrigger("isHit");
                eft.SetActive(true);
                return true;
            }
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

    void Guard()
    {
        actionState = ActionState.IsAtk;
        isGuard = true;
        animator.SetBool("isCounterWait", true);
        StartCoroutine(GuardCount());
        Debug.Log("Guard");
    }
    IEnumerator GuardCount()
    {
        yield return new WaitForSeconds(2f);
        isGuard = false;
        animator.SetBool("isCounterWait", false);
        StartCoroutine(MoveDelayTime(attackCoolTime));
        Debug.Log("Guard_Undo");
    }

    void Skill1()
    {
        animator.SetTrigger("isSkill");
        Invincible = true;
        enemyStatus.IncreaseHP(10);
        Debug.Log("Merchant's HP Restore!");
    }
}
