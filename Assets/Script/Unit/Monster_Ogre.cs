using UnityEngine;

public class Monster_Ogre : Monster_Control
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
        if (!notMove && !isAtk)
        {
            if (isTrace)
            {
                if (distanceX > 2f)
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
        if (distanceX < 1f)
        {
            notMove = true;
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
