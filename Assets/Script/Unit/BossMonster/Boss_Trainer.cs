using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Trainer : BossMonsterControl
{
    #region Debug Attack Position and Range

    public float Debug_AttackPositionX;
    public float Debug_AttackPositionY;
    public float Debug_AttackRangeX;
    public float Debug_AttackRangeY;

    #endregion

    float dashAttackCoolTime;
    Material DefaultMat;
    Material WhiteFlashMat;
    
    private void Start()
    {
        rotateDelayTime = 4f;
        attackCoolTime = 5f;
        dashAttackCoolTime = 7f;
        arrowDirection = 1;

        isFaceRight = true;

        Invincible = false;
        isGuard = false;

        StartCoroutine(MoveDelayTime(attackCoolTime));
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState != ActionState.Idle) return;
        Move();
        Attack();
    }

    public void Move()
    {
        if (distanceX < 1f)
            animator.SetBool("isMove", false);
        else
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
        }
    }

    void Attack()
    {
        counter = Random.Range(0, 100);

        if (counter < 40)
        {
            Guard();
        }

        if (distanceX < 1f)
        {
            Attack1();
        }
        else if(distanceX > 4f)
        {
            DashAttack();
        }
    }

    void Attack1()
    {
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isAttack1");
        rb.velocity = new Vector2(arrowDirection, rb.velocity.y);
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }
    void DashAttack()
    {
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isDashAttack");
        rb.velocity = new Vector2(3f * arrowDirection, rb.velocity.y);
        StartCoroutine(MoveDelayTime(dashAttackCoolTime));
    }
    void Guard()
    {
        isGuard = true;
        actionState = ActionState.IsAtk;
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + Debug_AttackPositionX, transform.position.y + Debug_AttackPositionY), new Vector3(Debug_AttackRangeX, Debug_AttackRangeY));
    }
}
