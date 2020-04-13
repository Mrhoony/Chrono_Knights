using UnityEngine;

public class Monster_Dog : NormalMonsterControl
{
    void OnEnable()
    {
        monsterCode = 2;
        rotateDelayTime = 4f;
        maxAttackDelayTime = 2f;
        arrowDirection = -1;
        isFaceRight = false;
        actionState = ActionState.Idle;

        MonsterInit();
    }
    
    public override void Move()
    {
        if (actionState == ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isRun", false);
            return;
        }

        if (isTrace)
        {
            if (distanceX > 1f)
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
    public override void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f)
        {
            rb.velocity = Vector2.zero;
            actionState = ActionState.IsAtk;
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAtk_Trigger"));
        }
    }
}
