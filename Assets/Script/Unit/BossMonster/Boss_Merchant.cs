using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Merchant : Monster_Control
{
    #region Debug Attack Position and Range

    public float Debug_AttackPositionX;
    public float Debug_AttackPositionY;
    public float Debug_AttackRangeX;
    public float Debug_AttackRangeY;

    #endregion
    
    #region Base Components

    #endregion

    #region Move Parameters

    #endregion

    #region Attack Parameters

    public bool ReadytoAttack;
    float attack1CoolTime;
    float attack2CoolTime;
    int counter;
    #endregion

    #region Guard Parameters
    bool isGuard;
    float guardTimeDelay;
    float guardTimer;
    #endregion

    Material DefaultMat;
    Material WhiteFlashMat;

    public bool Invincible;

    private void Start()
    {
        DefaultMat = Resources.Load<Material>("SpriteDefault");
        WhiteFlashMat = Resources.Load<Material>("WhiteFlash");
        
        rotateDelayTime = 1f;
        attack1CoolTime = 6f;
        attack2CoolTime = 4f;
        arrowDirection = 1;
        isGuard = false;
        isTrace = true;
        isFaceRight = true;
        ReadytoAttack = true;

        Invincible = false;
    }

    public new void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        BossMonsterInit();
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (!isTrace) return;
        if (actionState == ActionState.IsAtk) return;
        if (actionState == ActionState.NotMove) return;
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
            rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * -arrowDirection, rb.velocity.y + 2f);
        }
    }

    void Attack()
    {
        if (!ReadytoAttack) return;

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
        ReadytoAttack = false;
        actionState = ActionState.IsAtk;
        transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -0.3f, target.transform.position.y);
        animator.SetTrigger("isAttack1Trigger");
        StartCoroutine(AttackCoolTime(attack1CoolTime));
    }

    public void AttackMove(float moveDistance)
    {
        transform.position = new Vector2(transform.position.x + moveDistance * arrowDirection, transform.position.y);
    }

    void Attack2()
    {
        ReadytoAttack = false;
        actionState = ActionState.IsAtk;
        transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -0.3f, target.transform.position.y);
        animator.SetTrigger("isAttack2Trigger");
        StartCoroutine(AttackCoolTime(attack2CoolTime));
    }

    IEnumerator AttackCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        actionState = ActionState.Idle;
        ReadytoAttack = true;
    }

    public new void MonsterHit(int attack)
    {
        if (actionState == ActionState.IsDead) return;

        if (isGuard)
        {
            animator.SetTrigger("isCounterTrigger");
        }
        else
        {
            actionState = ActionState.NotMove;
            StartCoroutine(MoveDelayTime(1f));
            random = Random.Range(-2f, 2f);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(1f * PlayerControl.instance.GetArrowDirection() + random * 0.1f, 0f), ForceMode2D.Impulse);

            enemyStatus.DecreaseHP(attack);
            if (enemyStatus.IsDeadCheck())
            {
                actionState = ActionState.IsDead;
                gameObject.tag = "DeadBody";
            }
            else
            {
                animator.SetTrigger("isHit");
                eft.SetActive(true);
            }
        }
    }
    void Guard()
    {
        ReadytoAttack = false;
        isGuard = true;
        actionState = ActionState.IsAtk;
        StartCoroutine(AttackCoolTime(attack1CoolTime));
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
