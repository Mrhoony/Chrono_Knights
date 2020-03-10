using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Merchant : BossMonster_Control
{
    #region Debug Attack Position and Range

    public float Debug_AttackPositionX;
    public float Debug_AttackPositionY;
    public float Debug_AttackRangeX;
    public float Debug_AttackRangeY;

    #endregion
    
    float backAttackCoolTime;
    
    Material DefaultMat;
    Material WhiteFlashMat;

    private void OnEnable()
    {
        DefaultMat = Resources.Load<Material>("SpriteDefault");
        WhiteFlashMat = Resources.Load<Material>("WhiteFlash");
        
        rotateDelayTime = 1f;
        attackCoolTime = 6f;
        backAttackCoolTime = 4f;
        arrowDirection = 1;
        isGuard = false;
        isFaceRight = true;
        Invincible = false;
        monsterCode = 1001;
        BossMonsterInit(monsterCode);

        actionState = ActionState.NotMove;
        StartCoroutine(MoveDelayTime(2f));
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
        {
            actionState = ActionState.NotMove;
            StartCoroutine(MoveDelayTime(rotateDelayTime));
            animator.SetTrigger("isDodgeTrigger");
            rb.velocity = new Vector2(moveSpeed * -arrowDirection, rb.velocity.y + 2f);
        }
    }

    void Attack()
    {
        counter = Random.Range(0, 100);

        if(counter < 20)
        {
            Guard();
        }
        else
        {
            if (distanceX <= 2f)
                Attack1();
            else
                Attack2();
        }
    }

    void Attack1()
    {
        actionState = ActionState.IsAtk;
        transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -0.3f, target.transform.position.y);
        animator.SetTrigger("isAttack1Trigger");
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }

    void Attack2()
    {
        actionState = ActionState.IsAtk;
        transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -0.3f, target.transform.position.y);
        animator.SetTrigger("isAttack2Trigger");
        StartCoroutine(MoveDelayTime(backAttackCoolTime));
    }

    void Guard()
    {
        isGuard = true;
        actionState = ActionState.IsAtk;
        StartCoroutine(MoveDelayTime(attackCoolTime));
    }

    void Skill1()
    {
        animator.SetTrigger("isSkill");
        Invincible = true;
        enemyStatus.IncreaseHP(10);
        Debug.Log("Merchant's HP Restore!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + Debug_AttackPositionX, transform.position.y + Debug_AttackPositionY), new Vector3(Debug_AttackRangeX, Debug_AttackRangeY));
    }
}
