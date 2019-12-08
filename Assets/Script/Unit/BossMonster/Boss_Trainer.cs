using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Trainer : Monster_Control
{
    #region Debug Attack Position and Range
    public enum AttackTypes
    {
        Attack1,
        DashAttack,
    }
    public AttackTypes AttackType;
    public float Debug_AttackPositionX;
    public float Debug_AttackPositionY;
    public float Debug_AttackRangeX;
    public float Debug_AttackRangeY;
    #endregion

    #region Check Direction
    
    #endregion

    #region Base Components

    #endregion

    #region Move Parameters

    #endregion

    #region Attack Parameters

    public bool ReadytoAttack;
    float attack1TimeDelay;
    float attack1Timer;
    float dashAttackCoolTime;
    float dashAttackTimer;

    bool isAttack1;
    bool isDashAttack;

    #endregion

    #region Gaurd Parameters
    float gaurdTimeDelay;
    float gaurdTimer;
    #endregion

    Material DefaultMat;
    Material WhiteFlashMat;

    public bool Invincible;

    private new void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        BossMonsterInit();
    }

    private void Start()
    {
        curRotateDelayTime = 0f;
        maxRotateDelayTime = 4f;
        attack1TimeDelay = 5f;
        attack1Timer = 0f;
        dashAttackCoolTime = 7f;
        attack1Timer = 0f;

        arrowDirection = 1;
        isFaceRight = true;
        ReadytoAttack = true;

        Invincible = false;
        isTrace = true;
    }

    private void FixedUpdate()
    {
        if (actionState != ActionState.Idle) return;
        Move();
        Attack1();
        DashAttack();
    }

    public void Move()
    {
        if (distanceX < 1f)
            animator.SetBool("isMove", false);
        else
        {
            animator.SetBool("isMove", true);
            rb.velocity = new Vector2(arrowDirection, rb.velocity.y);
        }
    }

    void Attack1()
    {
        if (ReadytoAttack)
            attack1Timer += Time.deltaTime;
        
        if (distanceX > 1f) return;

        if (attack1Timer >= attack1TimeDelay)
        {
            actionState = ActionState.IsAtk;
            animator.SetTrigger("isAttack1");
            rb.velocity = new Vector2(arrowDirection, rb.velocity.y);

            attack1Timer = 0f;
        }
    }

    void DashAttack()
    {
        if (dashAttackTimer < dashAttackCoolTime) return;

        if (distanceX > 4f)
        {
            actionState = ActionState.IsAtk;
            animator.SetTrigger("isDashAttack");
            rb.velocity = new Vector2(3f * arrowDirection, rb.velocity.y);

            dashAttackTimer = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + Debug_AttackPositionX, transform.position.y + Debug_AttackPositionY), new Vector3(Debug_AttackRangeX, Debug_AttackRangeY));
    }
}
