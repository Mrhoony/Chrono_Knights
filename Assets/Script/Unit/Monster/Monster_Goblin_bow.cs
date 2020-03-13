
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin_bow : NormalMonsterControl
{
    public GameObject shootingPosition;
    public GameObject arrowObject;
    public GameObject arrow;

    void OnEnable()
    {
        monsterCode = 4;
        rotateDelayTime = 3f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
        actionState = ActionState.Idle;
        MonsterInit(monsterCode);
    }
    
    public override void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            return;
        }
        if (isTrace)
        {
            if (distanceX > 3f)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(moveSpeed * arrowDirection, rb.velocity.y);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }
        else
        {
            if (randomMove != 0)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(moveSpeed * randomMove, rb.velocity.y);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }
    }

    public override void Attack()
    {
        if (distanceX < 3f)
        {
            MonsterFlip();
            actionState = ActionState.NotMove;
            rb.velocity = Vector2.zero;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                actionState = ActionState.IsAtk;
                animator.SetTrigger("isAtk");
                curAttackDelayTime = 0f;
                StartCoroutine(MoveDelayTime(rotateDelayTime));
            }
        }
    }

    public new void Dead()
    {
        animator.SetBool("isDead", true);
        StopCoroutine(Moving);
        //duneonManager.MonsterDie();
        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        DeadAnimation();
    }

    public void Shooting()
    {
        arrow = Instantiate(arrowObject, shootingPosition.transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileObjectArrow>().arrowShooting(enemyStatus.GetAttack(), distanceX, arrowDirection);
    }
}
