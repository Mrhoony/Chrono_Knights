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

    public void MonsterInit()
    {
        animator.SetBool("isDead", false);
        tag = "Monster";

        coroutine = false;
        actionState = ActionState.Idle;
        enemyStatus.MonsterInit(monsterCode);
        moveSpeed = enemyStatus.GetMoveSpeed();

        StartCoroutine(SearchPlayer());
        
        randomMoving = RandomMove();
        StartCoroutine(randomMoving);
    }
    public override void MonsterFlip()
    {
        if (actionState != ActionState.Idle) return;

        if (isTrace)
        {
            if (playerPos.x < transform.position.x && isFaceRight)
            {
                Flip();
            }
            else if (playerPos.x > transform.position.x && !isFaceRight)
            {
                Flip();
            }
        }
        else
        {
            if (randomMove < 0 && isFaceRight)
            {
                Flip();
            }
            else if (randomMove > 0 && !isFaceRight)
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

    public override void MonsterHit(int damage)
    {
        if (actionState == ActionState.IsDead) return;
        
        enemyStatus.DecreaseHP(damage);
        spriteRenderer.material = whiteFlashMaterial;
        spriteRenderer.material = defaultMaterial;

        if (enemyStatus.IsDeadCheck())
        {
            StopAllCoroutines();
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
        }
        else
        {
            animator.SetTrigger("isHit");
            StopCoroutine("moveDelayCoroutine");
            actionState = ActionState.NotMove;
            moveDelayCoroutine = MoveDelayTime(1f);
            StartCoroutine(moveDelayCoroutine);
            random = Random.Range(-0.2f, 0.2f);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(PlayerControl.instance.GetArrowDirection() + random, 0.2f), ForceMode2D.Impulse);
        }
    }
    public void Landing()
    {
        animator.SetBool("isJumping", false);
    }
    public override void Dead()
    {
        animator.SetBool("isDead", true);
        animator.SetTrigger("isDead_Trigger");
        //duneonManager.MonsterDie();
        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        //DeadAnimation();
    }

    public void OnDestroy()
    {
        //eftPool.Clear();
    }
}
