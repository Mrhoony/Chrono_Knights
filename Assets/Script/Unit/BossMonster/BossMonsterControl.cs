using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster_Control : Monster_Control
{
    public int playerPosition;

    public bool isDamagable;
    public bool isGuard;
    public int counter;
    public bool Invincible;
    
    // Start is called before the first frame update
    public void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        BossMonsterInit();
    }
    public void BossMonsterInit()
    {
        Debug.Log("BossMonsterInit");

        actionState = ActionState.Idle;
        enemyStatus.MonsterInit();
        moveSpeed = enemyStatus.GetMoveSpeed();
        StartCoroutine(SearchPlayerBoss());
    }
    public override void MonsterFlip()
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
    public override void MonsterHit(int damage)
    {
        if (actionState == ActionState.IsDead) return;

        if (isGuard)
        {
            animator.SetTrigger("isCounterTrigger");
        }
        else
        {
            actionState = ActionState.NotMove;
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
    public void AttackMove(float moveDistance)
    {
        transform.position = new Vector2(transform.position.x + moveDistance * arrowDirection, transform.position.y);
    }
}
