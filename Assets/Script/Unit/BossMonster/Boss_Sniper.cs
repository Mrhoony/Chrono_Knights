using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Sniper : BossMonster_Control
{
    public readonly float zoomShotMaxDelayTime = 2f;
    public readonly float zoomShotMaxCoolTime = 6f;
    public float zoomShotDelayTime;
    public float zoomShotCoolTime;
    public bool isZoom;

    private void OnEnable()
    {
        attackMaxCoolTime = 4f;
        rotateDelayTime = 2f;
        attackCoolTime = attackMaxCoolTime;
        zoomShotCoolTime = zoomShotMaxCoolTime;
        zoomShotDelayTime = zoomShotMaxDelayTime;

        accumulationDamage = 0;

        arrowDirection = 1;

        isGuard = false;
        Invincible = false;
        MonsterInit();

        actionState = ActionState.Idle;
        StartCoroutine(MoveDelayTime(2f));
    }

    private void Update()
    {
        if (isZoom)
        {
            if(zoomShotDelayTime > 0f)
            {
                zoomShotDelayTime -= Time.deltaTime;
            }
        }

        if(zoomShotCoolTime > 0f)
        {
            zoomShotCoolTime -= Time.deltaTime;
        }
        if(attackCoolTime > 0f)
        {
            attackCoolTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (CanvasManager.instance.isMainScenarioOn) return;
        if (actionState == ActionState.IsDead) return;

        MonsterFlip();
        Walk();
        Attack();
    }

    void Walk()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX >= 2f && distanceX <= 4f)
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
        }
        else if (distanceX > 4f && distanceX <= 6f)
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(-arrowDirection * moveSpeed, rb.velocity.y);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }
    void Attack()
    {
        if(isZoom && zoomShotDelayTime <= 0f)
        {
            ZoomShot();
        }

        if (actionState != ActionState.Idle) return;

        if (distanceX > 6f)
        {
            Zoom();
        }
        else if(distanceX < 2f)
        {
            Shot();
        }
    }

    void Zoom()
    {
        actionState = ActionState.IsAtk;
        isZoom = true;
        zoomShotDelayTime = zoomShotMaxDelayTime;
        animator.SetBool("isZoom", true);
    }
    void ZoomShot()
    {
        isZoom = false;
        zoomShotDelayTime = zoomShotMaxDelayTime;
        zoomShotCoolTime = zoomShotMaxCoolTime;
        animator.SetBool("isZoom", false);
        animator.SetTrigger("Trigger_ZoomShot");
        moveDelayCount = MoveDelayTime(attackCoolTime);
        StartCoroutine(moveDelayCount);
    }
    void Cancel()
    {
        isZoom = false;
        animator.SetBool("isZoom", false);
        actionState = ActionState.NotMove;
        moveDelayCount = MoveDelayTime(attackCoolTime);
        StartCoroutine(moveDelayCount);
    }

    void Shot()
    {
        actionState = ActionState.IsAtk;
        attackCoolTime = attackMaxCoolTime;
        moveDelayCount = MoveDelayTime(attackCoolTime);
        StartCoroutine(moveDelayCount);
    }

    void Hide()
    {
        Color color = spriteRenderer.color;
    }
    void Esc() { }

    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;

        accumulationDamage += enemyStatus.DecreaseHP(_damage);
        eft.SetActive(true);

        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
            return true;
        }
        else
        {
            if (isZoom)
            {
                if (enemyStatus.AccumulationDamageCheck(accumulationDamage))
                {
                    Cancel();
                }
            }
            else
            {
                actionState = ActionState.NotMove;
                moveDelayCount = MoveDelayTime(attackCoolTime);
                StartCoroutine(MoveDelayTime(1f));
            }
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
}
