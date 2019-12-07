using UnityEngine;

public class Monster_Ogre : Monster_Control
{
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        isFaceRight = true;
        arrowDirection = 1;
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState == ActionState.NotMove) return;
        Move();
        if (!isTrace) return;
        Attack();
    }

    void Move()
    {
        if (!isAtk)
        {
            if (isTrace)
            {
                if (distanceX > 2f)
                {
                    animator.SetBool("isMove", true);
                    rb.velocity = new Vector2(ehp.GetMoveSpeed() * arrowDirection, rb.velocity.y);
                }
                else
                {
                    animator.SetBool("isMove", false);
                }
            }
            else
            {
                if (randomMove != 0)
                {
                    animator.SetBool("isMove", true);
                    rb.velocity = new Vector2(ehp.GetMoveSpeed() * randomMove, rb.velocity.y);
                }
                else
                {
                    animator.SetBool("isMove", false);
                }
            }
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    void Attack()
    {
        if (distanceX < 1f)
        {
            actionState = ActionState.NotMove;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                isAtk = true;
                curRotateDelayTime = 0f;
                curAttackDelayTime = 0f;
                randomAttack = Random.Range(0, 2);
                if (randomAttack > 0)
                    animator.SetTrigger("isAtkH_Trigger");
                else
                    animator.SetTrigger("isAtkV_Trigger");
            }
        }
    }
}
