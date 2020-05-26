using System.Collections;
using UnityEngine;

public abstract class NormalMonsterControl : Monster_Control
{
    IEnumerator randomMoving;
    IEnumerator moveDelayCoroutine;

    public bool isTrace;
    public int randomMove;
    public float randomMoveCount;
    public float randomAttack;
    public bool isDamagable;

    public override void MonsterInit()
    {
        tag = "Monster";

        coroutine = false;
        actionState = ActionState.Idle;
        enemyStatus.MonsterInit(monsterCode);
        moveSpeed = enemyStatus.GetMoveSpeed();
        monsterWeight = enemyStatus.monsterWeight;

        StartCoroutine(SearchPlayer());
        
        randomMoving = RandomMove();
        StartCoroutine(randomMoving);
        monsterDeadCount = null;
    }
    public override void MonsterFlip()
    {
        if (actionState != ActionState.Idle) return;

        if (isTrace)
        {
            if (playerPos.x < transform.position.x && arrowDirection == 1)
            {
                Flip();
            }
            else if (playerPos.x > transform.position.x && arrowDirection != 1)
            {
                Flip();
            }
        }
        else
        {
            if (randomMove < 0 && arrowDirection == 1)
            {
                Flip();
            }
            else if (randomMove > 0 && arrowDirection != 1)
            {
                Flip();
            }
        }
    }

    public void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        MonsterFlip();
        Move();
        if (!isTrace) return;
        Attack();
    }
    public abstract void Move();
    public abstract void Attack();

    public IEnumerator SearchPlayer()
    {
        while (actionState != ActionState.IsDead)
        {
            playerPos = target.transform.position;
            distanceX = playerPos.x - transform.position.x;
            distanceY = playerPos.y - transform.position.y;

            if (distanceX < 0) distanceX *= -1f;
            if (distanceY < 0) distanceY *= -1f;

            if (distanceX < 3f && distanceY < 2f)
            {
                if (!isTrace)
                {
                    if (actionState != ActionState.IsAtk)
                    {
                        isTrace = true;
                        StopCoroutine(randomMoving);
                    }
                }
            }
            else if (distanceX > 3f || distanceY > 2f)
            {
                if (isTrace)
                {
                    if (actionState != ActionState.IsAtk)
                    {
                        isTrace = false;
                        StartCoroutine(randomMoving);
                    }
                }
            }
            yield return null;
        }
    }
    public IEnumerator RandomMove()
    {
        if (coroutine)
        {
            yield break;
        }
        coroutine = true;

        randomMoveCount = Random.Range(1f, 2f);

        while (true)
        {
            randomMove = Random.Range(-1, 2);
            yield return new WaitForSeconds(randomMoveCount);
        }
    }
    public IEnumerator AttackDelayCount(float _attackDelayCount, float _rotateDelayCount, string _triggerName)
    {
        yield return new WaitForSeconds(_attackDelayCount);
        animator.SetTrigger(_triggerName);

        moveDelayCoroutine = MoveDelayTime(_rotateDelayCount);
        StartCoroutine(moveDelayCoroutine);
    }
    public IEnumerator AttackDelayCountBool(float _attackDelayCount, float _rotateDelayCount, string _boolName)
    {
        yield return new WaitForSeconds(_attackDelayCount);
        animator.SetBool(_boolName, true);

        moveDelayCoroutine = MoveDelayTime(_rotateDelayCount);
        StartCoroutine(moveDelayCoroutine);
        Debug.Log("monster attack");
    }

    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;
        
        _damage = enemyStatus.DecreaseHP(_damage);

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
            animator.SetTrigger("isHit");
            actionState = ActionState.NotMove;
            StopCoroutine("moveDelayCoroutine");
            moveDelayCoroutine = MoveDelayTime(1.5f);
            StartCoroutine(moveDelayCoroutine);
            return true;
        }
    }
    public override void MonsterHitRigidbodyEffectUpper(int _knockBack)
    {
        random = Random.Range(-0.2f, 0.2f);

        int knockBack = _knockBack - monsterWeight;
        if (knockBack > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2((PlayerControl.instance.GetArrowDirection() + random) * 0.5f, knockBack), ForceMode2D.Impulse);
        }
    }
    public override void MonsterHitRigidbodyEffectKnockBack(int _knockBack)
    {
        random = Random.Range(-0.2f, 0.2f);

        int knockBack = _knockBack - monsterWeight;
        if (knockBack > 0)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2((PlayerControl.instance.GetArrowDirection() + random) * knockBack * 0.5f, 1f), ForceMode2D.Impulse);
        }
    }
    public IEnumerator MonsterHitEffect()
    {
        for(int i = 0; i < 2; ++i)
        {
            spriteRenderer.material = whiteFlashMaterial;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = defaultMaterial;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void Landing()
    {
        animator.SetBool("isJumping", false);
    }
    public override void Dead()
    {
        animator.SetTrigger("Trigger_Die");
        spriteRenderer.material = defaultMaterial;
        StopAllCoroutines();
        //die => sort layer
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }

        enabled = false;
        monsterDeadCount();
        monsterDeadCount = null;
    }
}
