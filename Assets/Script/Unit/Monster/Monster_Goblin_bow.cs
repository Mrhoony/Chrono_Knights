
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin_bow : Monster_Control
{
    public GameObject shootingPosition;
    public GameObject arrow;

    void Start()
    {
        rotateDelayTime = 3f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
        actionState = ActionState.Idle;
        arrow.GetComponent<ProjectileObjectArrow>().Init(gameObject);
        arrow.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState == ActionState.IsAtk) return;
        Move();
        if (!isTrace) return;
        Attack();
    }

    void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            return;
        }
        Debug.Log("move");
        if (isTrace)
        {
            if (distanceX > 3f)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(moveSpeed * arrowDirection, rb.velocity.y);
                Debug.Log("move 1");
            }
            else
            {
                animator.SetBool("isMove", false);
                Debug.Log("move 11");
            }
        }
        else
        {
            if (randomMove != 0)
            {
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(moveSpeed * randomMove, rb.velocity.y);
                Debug.Log("move2");
            }
            else
            {
                animator.SetBool("isMove", false);
                Debug.Log("move22");
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
        arrow.GetComponent<ProjectileObjectArrow>().arrowShooting(distanceX, arrowDirection);
    }
}
