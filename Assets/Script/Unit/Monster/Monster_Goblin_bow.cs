
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
        if (actionState != ActionState.Idle) return;

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
        if (actionState != ActionState.Idle) return;

        if (distanceX < 3f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            animator.SetTrigger("isAtk");
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtk"));
        }
    }

    public void Shooting()
    {
        arrow = Instantiate(arrowObject, shootingPosition.transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileObjectArrow>().arrowShooting(enemyStatus.GetAttack(), distanceX, arrowDirection);
    }
}
