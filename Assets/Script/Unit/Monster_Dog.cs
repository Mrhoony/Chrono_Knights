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
        effectX = 0.2f;
        effectY = 0.3f;
    }
    public void OnDisable()
    {
        StopCoroutine(Moving);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        isFaceRight = false;
        arrow = -1;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Move();
        if (!isTrace) return;
        Attack();
    }
    
    void Move()
    {
        if (!notMove)
        {
            if (isTrace)
            {
                if(distanceX > 0.5f)
                {
                    if (!isAtk)
                    {
                        animator.SetBool("isRun", true);
                        rb.velocity = new Vector2(ehp.GetMoveSpeed() * 2f * arrow, rb.velocity.y);
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
        }
        else
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isRun", false);
        }
    }

    void Attack()
    {
        if (distanceX < 0.5f)
        {
            rb.velocity = Vector2.zero;
            notMove = true;
            curAttackDelayTime += Time.fixedDeltaTime;
            if (curAttackDelayTime > maxAttackDelayTime)
            {
                isAtk = true;
                animator.SetBool("isAtk_Trigger", true);
                curRotateDelayTime = 0f;
                curAttackDelayTime = 0f;
            }
        }
    }
}
