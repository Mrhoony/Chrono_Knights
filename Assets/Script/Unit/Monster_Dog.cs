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
    void Update()
    {
        MonsterFlip();
        notMoveDelayTime();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Attack();
        }
    }
    
    void Move()
    {
        if (!notMove)
        {
            if (isTrace)
            {
                if(playerPos.x * playerPos.x - transform.position.x * transform.position.x > 0.5f)
                {
                    if (!isAtk)
                    {
                        animator.SetBool("isRun", true);
                        rb.velocity = new Vector2(ehp.moveSpeed * 2f * arrow, rb.velocity.y);
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
                    rb.velocity = new Vector2(ehp.moveSpeed * randomMove, rb.velocity.y);
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
        if (isTrace)
        {
            MonsterFlip();

            if (playerPos.x * playerPos.x - transform.position.x * transform.position.x < 0.5f)
            {
                rb.velocity = Vector2.zero;
                notMove = true;
                curAttackDelayTime += Time.fixedDeltaTime;
                if (curAttackDelayTime > maxAttackDelayTime)
                {
                    MonsterFlip();
                    isAtk = true;
                    animator.SetBool("isAtk_Trigger", true);
                    curRotateDelayTime = 0f;
                    curAttackDelayTime = 0f;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTrace = true;
            StopCoroutine(Moving);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAtk = false;
            notMove = true;
            isTrace = false;
            curAttackDelayTime = 0f;
            Moving = RandomMove(randomMoveCount);
            StartCoroutine(Moving);
        }
    }
}
