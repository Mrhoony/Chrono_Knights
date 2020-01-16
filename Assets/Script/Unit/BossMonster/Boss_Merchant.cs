using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Merchant : Monster_Control
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
    float attack1CoolTime;
    float attack2CoolTime;
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
        DefaultMat = Resources.Load<Material>("SpriteDefault");
        WhiteFlashMat = Resources.Load<Material>("WhiteFlash");
        
        rotateDelayTime = 0.8f;
        attack1CoolTime = 4f;
        attack2CoolTime = 1f;
        attack1TimeDelay = 0f;
        attack1Timer = 5f;
        arrowDirection = 1;
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
        Attack1();
    }

    public void Move()
    {
        if (distanceX < 0.5f)
        {
            actionState = ActionState.NotMove;
            StartCoroutine(MoveDelayTime(rotateDelayTime));
            animator.SetTrigger("isDodgeTrigger");
            rb.velocity = new Vector2(enemyStatus.GetMoveSpeed() * -arrowDirection, rb.velocity.y + 2f);
        }
    }

    void Attack1()
    {
        if (distanceX > 0.5f)
        {
            if (ReadytoAttack)
            {
                attack1TimeDelay += Time.deltaTime;
            }

            if (attack1TimeDelay >= attack1Timer)
            {
                attack1TimeDelay = 0f;
                ReadytoAttack = false;
                actionState = ActionState.IsAtk;
                transform.position = new Vector2(target.transform.position.x + target.GetComponent<PlayerControl>().GetArrowDirection() * -0.3f, target.transform.position.y);
                StartCoroutine(AttackCoolTime(attack1CoolTime));
            }
        }
    }
    public void Attack1Move(float moveDistance)
    {
        transform.position = new Vector2(transform.position.x + moveDistance * arrowDirection, transform.position.y);
    }

    IEnumerator AttackCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        actionState = ActionState.Idle;
        ReadytoAttack = true;
    }

    void Gaurd()
    {

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
