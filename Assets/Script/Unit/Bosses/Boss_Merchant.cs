using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Merchant : BossMonsterControl
{
    #region Debug Attack Position and Range
    public enum AttackTypes
    {
        Attack_1,
        Attack_2,
        Attack_3
    }
    public AttackTypes AttackType;
    public float Debug_AttackPositionX;
    public float Debug_AttackPositionY;
    public float Debug_AttackRangeX;
    public float Debug_AttackRangeY;
    #endregion

    #region Check Direction

    public bool isCheckRotationDirection;
    int MovementDirection;

    #endregion

    #region Base Components

    #endregion

    #region Move Parameters

    #endregion

    #region Attack Parameters

    public bool ReadytoAttack;
    float attack1TimeDelay;    
    float attack1Timer;

    #endregion

    #region Gaurd Parameters
    float gaurdTimeDelay;
    float gaurdTimer;
    #endregion

    Material DefaultMat;
    Material WhiteFlashMat;

    public bool Invincible;

    private void Start()
    {
        ehp = GetComponent<EnemyStat>();

        DefaultMat = Resources.Load<Material>("SpriteDefault");
        WhiteFlashMat = Resources.Load<Material>("WhiteFlash");

        curRotateDelayTime = 0f;
        maxRotateDelayTime = 0.8f;
        attack1TimeDelay = 0f;
        attack1Timer = 5f;

        MovementDirection = 1;
        isFaceRight = true;
        ReadytoAttack = true;
        isCheckRotationDirection = true;

        Invincible = false;
    }

    private void Update()
    {
        MonsterFlip();
        Attack1();
    }

    public void Move()
    {
        curRotateDelayTime += Time.deltaTime;

        if (curRotateDelayTime < maxRotateDelayTime)
        {
            rb.velocity = new Vector2(-1f * MovementDirection, rb.velocity.y);            
        }

        if (curRotateDelayTime >= maxRotateDelayTime)
        {
            curRotateDelayTime = 0f;
            animator.SetBool("isMove", false);
        }
    }

    void Attack1()
    {
        if (ReadytoAttack)
        {
            attack1TimeDelay += Time.deltaTime;
        }

        if (attack1TimeDelay >= attack1Timer)
        {
            isAtk = true;
            animator.SetBool("isAttack", true);
            rb.velocity = new Vector2(-1f * MovementDirection, rb.velocity.y);

            attack1TimeDelay = 0f;
        }
    }

    void Gaurd()
    {

    }

    void Skill1()
    {
        animator.SetTrigger("isSkill");
        Invincible = true;
        ehp.IncreaseHP(10);
        Debug.Log("Merchant's HP Restore!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + Debug_AttackPositionX, transform.position.y + Debug_AttackPositionY), new Vector3(Debug_AttackRangeX, Debug_AttackRangeY));
    }
}
