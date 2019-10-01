using UnityEngine;

public class Monster_Ogre : MonsterControl
{
    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        effectX = 0.5f;
        effectY = 0.5f;
        isFaceRight = true;
        arrow = 1;
    }

    public override void OnEnable()
    {
        base.OnEnable();
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
        if (!notMove && !isAtk)
        {
            if (isTrace)
            {
                if (playerPos.x * playerPos.x - transform.position.x * transform.position.x > 2f)
                {
                    animator.SetBool("isMove", true);
                    rb.velocity = new Vector2(ehp.moveSpeed * arrow, rb.velocity.y);
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
                    rb.velocity = new Vector2(ehp.moveSpeed * randomMove, rb.velocity.y);
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
        if (isTrace)
        {
            MonsterFlip();

            if (playerPos.x * playerPos.x - transform.position.x * transform.position.x < 2f)
            {
                notMove = true;
                curAttackDelayTime += Time.fixedDeltaTime;
                if (curAttackDelayTime > maxAttackDelayTime)
                {
                    MonsterFlip();
                    isAtk = true;
                    randomAttack = Random.Range(0, 2);
                    if (randomAttack > 0)
                        animator.SetTrigger("isAtkH_Trigger");
                    else
                        animator.SetTrigger("isAtkV_Trigger");
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
            if (collision.CompareTag("Player"))
            {
                isTrace = true;
                StopCoroutine(Moving);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                notMove = true;
                isTrace = false;
                curAttackDelayTime = 0f;
                Moving = RandomMove(randomMoveCount);
                StartCoroutine(Moving);
            }
        }
    }
}
