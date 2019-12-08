using UnityEngine;

public class Monster_Dog : Monster_Control
{
    public override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        arrowDirection = -1;
    }
    public void OnDisable()
    {
        StopCoroutine(Moving);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        isFaceRight = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        Move();
        if (!isTrace) return;
        Attack();
    }
    
    void Move()
    {
        if (isTrace)
        {
            if (distanceX > 0.5f)
            {
                if (actionState != ActionState.IsAtk)
                {
                    animator.SetBool("isRun", true);
                    rb.velocity = new Vector2(ehp.GetMoveSpeed() * 2f * arrowDirection, rb.velocity.y);
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
                animator.SetBool("isRun", false);
                animator.SetBool("isMove", true);
                rb.velocity = new Vector2(ehp.GetMoveSpeed() * randomMove, rb.velocity.y);
            }
            else
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isRun", false);
            }
        }
        if (actionState != ActionState.NotMove)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isRun", false);
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
                curRotateDelayTime = 0f;
                curAttackDelayTime = 0f;
            }
        }
    }
}
