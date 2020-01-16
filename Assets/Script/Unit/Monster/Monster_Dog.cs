using UnityEngine;

public class Monster_Dog : Monster_Control
{
    public void Start()
    {
        rotateDelayTime = 2f;
        maxAttackDelayTime = 1f;
        curAttackDelayTime = 0f;
        arrowDirection = -1;
        isFaceRight = false;
        actionState = ActionState.Idle;
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        Move();
        if (actionState == ActionState.NotMove) return;
        Attack();
    }
    
    void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isRun", false);
            return;
        }
        if (isTrace)
        {
            if (distanceX > 0.5f)
            {
                if (actionState != ActionState.IsAtk)
                {
                    animator.SetBool("isRun", true);
                    rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * 2f * arrowDirection, rb.velocity.y);
                }
            }
            else
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            if (randomMove != 0)
            {
                animator.SetBool("isMove", true);
                animator.SetBool("isRun", false);
                rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * randomMove, rb.velocity.y);
            }
            else
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isRun", false);
            }
        }
    }

    void Attack()
    {
        if (distanceX < 0.5f)
        {
            actionState = ActionState.NotMove;
            rb.velocity = Vector2.zero;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                actionState = ActionState.IsAtk;
                animator.SetBool("isAtk_Trigger", true);
                curAttackDelayTime = 0f;
                StartCoroutine(MoveDelayTime(rotateDelayTime));
            }
        }
    }
}
