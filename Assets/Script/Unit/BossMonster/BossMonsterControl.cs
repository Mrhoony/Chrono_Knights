using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossMonster_Control : Monster_Control
{
    public IEnumerator moveDelayCount;
    public int playerPosition;
    public bool isDamagable;
    public bool isGuard;
    public int counter;
    public bool Invincible;
    
    public override void MonsterInit()
    {
        animator.SetBool("isDead", false);
        tag = "BossMonster";

        actionState = ActionState.Idle;
        enemyStatus.BossMonsterInit(monsterCode);
        moveSpeed = enemyStatus.GetMoveSpeed();
        StartCoroutine(SearchPlayerBoss());

        Debug.Log("BossMonsterInit");
    }
    public override void MonsterFlip()
    {
        if (actionState != ActionState.Idle) return;

        if (playerPos.x < transform.position.x && isFaceRight)
        {
            Flip();
        }
        else if (playerPos.x > transform.position.x && !isFaceRight)
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
        animator.SetBool("isDead", true);
        animator.SetTrigger("isDead_Trigger");
        DungeonManager.instance.dungeonMaker.FloorBossKill();
        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        enabled = false;
        monsterDeadCount();
        monsterDeadCount = null;
    }
}
