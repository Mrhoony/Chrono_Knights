using UnityEngine;

public class Monster_Dog : MonsterControl
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
                if (!isAtk && Mathf.Abs(playerPos.x - transform.position.x) > 0.5f)
                {
                    animator.SetBool("isRun", true);
                    rb.velocity = new Vector2(ehp.moveSpeed * 2f * arrow, rb.velocity.y);
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
            /*
            if (isDamagable)
            {
                curRotateDelayTime = 0;
            }
            else
            {
                curRotateDelayTime += Time.fixedDeltaTime;
                if (curRotateDelayTime > maxRotateDelayTime)
                {
                    isAtk = false;
                    notMove = false;
                    curAttackDelayTime = 0;
                }
            }
            */
            if (Mathf.Abs(playerPos.x - transform.position.x) < 0.5f)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
                isTrace = true;
                StopCoroutine(Moving);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
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
}
