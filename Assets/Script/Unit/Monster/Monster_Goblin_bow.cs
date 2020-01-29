
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin_bow : Monster_Control
{
    public GameObject shootingPosition;
    public GameObject arrow;

    void Start()
    {
        rotateDelayTime = 2f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
        actionState = ActionState.Idle;
        arrow = Instantiate(arrow, transform.position, Quaternion.identity);
        arrow.GetComponent<ProjectileObjectArrow>().Init(gameObject);
        arrow.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        Move();
        Attack();
    }

    void Move()
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
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * arrowDirection, rb.velocity.y);
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
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * randomMove, rb.velocity.y);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }
    }

    void Attack()
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
                arrow.SetActive(false);
                animator.SetTrigger("isAtk");
                curAttackDelayTime = 0f;
                StartCoroutine(MoveDelayTime(rotateDelayTime));
            }
        }
    }

    public void Dead()
    {
        animator.SetBool("isDead", true);
        StopCoroutine(Moving);
        //duneonManager.MonsterDie();
        if (dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        DeadAnimation();
        Destroy(arrow);
    }

    public void Shooting()
    {
        arrow.SetActive(true);
        arrow.transform.position = shootingPosition.transform.position;
        arrow.GetComponent<ProjectileObjectArrow>().arrowShooting(distanceX * arrowDirection);
    }
}
