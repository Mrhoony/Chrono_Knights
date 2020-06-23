using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Sniper : BossMonster_Control
{
    public float backAttackCoolTime;

    private void OnEnable()
    {
        rotateDelayTime = 2f;
        attackCoolTime = 6f;
        backAttackCoolTime = 4f;

        arrowDirection = 1;

        isGuard = false;
        Invincible = false;
        MonsterInit();

        actionState = ActionState.Idle;
        StartCoroutine(MoveDelayTime(2f));
    }

    private void Update()
    {
        Zoom();
        Zoomshot();
        Walk();
        Esc();
        Cancel();
        Shot();
    }
    private void FixedUpdate()
    {
        if (CanvasManager.instance.isMainScenarioOn) return;
        if (actionState == ActionState.IsDead) return;

        MonsterFlip();
    }

    void Hide() { }
    void Zoom() { }
    void Zoomshot() { }
    void Walk() { }
    void Esc() { }
    void Cancel() { }
    void Shot() { }

    public override bool MonsterHit(int _damage)
    {
        if (actionState == ActionState.IsDead) return false;

        actionState = ActionState.NotMove;
        enemyStatus.DecreaseHP(_damage);
        StartCoroutine(MoveDelayTime(1f));

        if (enemyStatus.IsDeadCheck())
        {
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
            Dead();
            DungeonManager.instance.dungeonMaker.FloorBossKill();
            return true;
        }
        else
        {
            animator.SetTrigger("isHit");
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
