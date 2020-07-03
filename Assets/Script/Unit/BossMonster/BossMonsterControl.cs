using System.Collections;
using UnityEngine;

public abstract class BossMonster_Control : Monster_Control
{
    public IEnumerator moveDelayCount;
    public int playerPosition;
    public bool isDamagable;
    public bool isGuard;
    public int counter;
    public bool Invincible;
    public float attackCoolTime;
    public float attackMaxCoolTime;
    public int accumulationDamage;
    public string bossDieEvent = "";
    
    public override void MonsterInit()
    {
        if (CanvasManager.instance.isMainScenarioOn) return;
        tag = "BossMonster";

        arrowDirection = 1;
        actionState = ActionState.Idle;
        enemyStatus.BossMonsterInit(monsterCode);
        moveSpeed = enemyStatus.GetMoveSpeed();

        target = GameObject.Find("PlayerCharacter");
        animator.SetTrigger("Trigger_Reset");
        StartCoroutine(SearchPlayerBoss());

        Debug.Log("BossMonsterInit");
    }
    public override void MonsterFlip()
    {
        if (actionState != ActionState.Idle) return;

        if (playerPos.x < transform.position.x && arrowDirection == 1)
        {
            Flip();
        }
        else if (playerPos.x > transform.position.x && arrowDirection != 1)
        {
            Flip();
        }
    }
    
    public IEnumerator SearchPlayerBoss()
    {
        while (actionState != ActionState.IsDead)
        {
            playerPos = target.transform.position;
            distanceX = playerPos.x - transform.position.x;
            distanceY = playerPos.y - transform.position.y;

            if (distanceX < 0)
            {
                distanceX *= -1f;
                playerPosition = 1;
            }
            else
            {
                playerPosition = -1;
            }
            yield return null;
        }
    }
    public abstract override bool MonsterHit(int _damage);

    public void AttackMove(float moveDistance)
    {
        transform.position = new Vector2(transform.position.x + moveDistance * arrowDirection, transform.position.y);
    }

    public override void Dead()
    {
        animator.SetTrigger("isDie_Trigger");
        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        enabled = false;
        monsterDeadCount();
        monsterDeadCount = null;
        Invoke("BossDeadEvent", 2f);
    }

    public void BossDeadEvent()
    {
        gameObject.SetActive(false);
        if(bossDieEvent != "")
        {
            DungeonManager.instance.scenarioManager.ScenarioCheck(bossDieEvent);
        }
    }
}
