using UnityEngine;
using System.Collections;

public class Monster_Frog : NormalMonsterControl
{
    public GameObject box;
    
    void OnEnable()
    {
        monsterCode = 1;
        rotateDelayTime = 3f;
        maxAttackDelayTime = 2f;
        arrowDirection = 1;
        isFaceRight = true;
        actionState = ActionState.Idle;
        MonsterInit(monsterCode);
    }

    public override void Move()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX < 1f) return;
        if (randomMove != 0)
        {
            actionState = ActionState.NotMove;
            animator.SetTrigger("isJump");
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(1.5f * randomMove, 2f);
            StartCoroutine(MoveDelayTime(rotateDelayTime));
        }
    }

    public override void Attack()
    {
        if (actionState != ActionState.Idle) return;

        if (distanceX >= 1f) return;

        actionState = ActionState.IsAtk;
        int AttackType = Random.Range(0, 2);

        if (AttackType == 0)
        {
            StartCoroutine(AttackDelayCountBool(maxAttackDelayTime, rotateDelayTime, "isJumping"));
        }
        else if (AttackType == 1)
        {
            StartCoroutine(AttackDelayCount(maxAttackDelayTime, rotateDelayTime, "isAttack"));
        }
    }

    public void AttackStart(float x, float y)
    {
        rb.velocity = new Vector2(x * arrowDirection, y);
    }
}
