using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Trainer : BossMonster_Control
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
        Move();
        if (actionState != ActionState.Idle) return;
        Attack();
    }

    public void Move()
    {
        if (actionState == ActionState.IsAtk)
            animator.SetBool("isMove", false);
        else
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
        }
    }

    void Attack()
    {
        if (distanceX < 1f)
        {
            counter = Random.Range(0, 100);

            if (counter < 40)
                Guard();
            else
                Attack1();
        }
        else if(distanceX > 4f && distanceX < 7f)
        {
            DashMove();
        }
    }

    void Attack1()
    {
        Debug.Log("attack");
        actionState = ActionState.IsAtk;
        animator.SetTrigger("isAttack1");
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }
    public void DashAttack()
    {
        Debug.Log("dashAttack");
        StopCoroutine("DashDuration");
        animator.SetBool("isDashAttack", false);
        StartCoroutine(MoveDelayTime(dashAttackCoolTime));
    }
    public void DashMove()
    {
        actionState = ActionState.IsAtk;
        animator.SetBool("isDash", true);
        animator.SetBool("isDashAttack", true);
        StartCoroutine("DashDuration");
        rb.velocity = new Vector2(arrowDirection * moveSpeed, rb.velocity.y);
    }
    public void DashAttackEnd()
    {
        animator.SetBool("isDash", false);
    }

    public void Dash()
    {
        Debug.Log("dash");
        rb.velocity = new Vector2(arrowDirection * moveSpeed * 2f, rb.velocity.y);
    }
    IEnumerator DashDuration()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("isDash", false);
        animator.SetBool("isDashAttack", false);
        StartCoroutine(MoveDelayTime(attackCoolTime));
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
