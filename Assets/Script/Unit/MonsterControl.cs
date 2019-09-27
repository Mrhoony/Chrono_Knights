using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MovingObject
{
    protected GameObject target;
    protected Vector2 playerPos;
    public GameObject box;
    public GameObject eft;
    protected EnemyStat ehp;

    public float effectX;
    public float effectY;
    protected float random;

    protected IEnumerator Moving;

    public bool isTrace;
    public int randomMove;
    public float randomMoveCount;
    public float randomAttack;
    public bool isAtk;
    public bool isJump;
    public bool isDamagable;

    public float maxRotateDelayTime;
    public float curRotateDelayTime;
    public float maxAttackDelayTime;
    public float curAttackDelayTime;

    // Start is called before the first frame update

    public IEnumerator SearchPlayer()
    {
        while (!isDead)
        {
            playerPos = target.transform.position;
            yield return null;
        }
    }

    public IEnumerator RandomMove(float time)
    {
        randomMove = Random.Range(-1, 2);
        notMove = false;

        yield return new WaitForSeconds(time);

        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
    }

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ehp = GetComponent<EnemyStat>();

        target = GameObject.Find("PlayerCharacter");
    }

    public virtual void OnEnable()
    {
        ehp.currentHP = ehp.HP;
        StartCoroutine(SearchPlayer());

        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
    }
    
    public void notMoveDelayTime()
    {
        if (isAtk)
        {
            curAttackDelayTime = 0;
            curRotateDelayTime += Time.deltaTime;
            if (curRotateDelayTime > maxRotateDelayTime)
            {
                isAtk = false;
                notMove = false;
                curRotateDelayTime = 0f;
            }
        }
    }

    public void Hit(float playerAtk, float x, float y)
    {
        if (isDead || isDamagable)
            return;

        random = Random.Range(-1f, 1f);

        ehp.currentHP -= playerAtk;

        notMove = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1f * PlayerControl.instance.arrowDirection + random * 0.1f, 1f + random * 0.1f), ForceMode2D.Impulse);
        
        if(arrow > 0)
            Instantiate(eft, new Vector2(transform.position.x + -arrow * x, transform.position.y + y), Quaternion.Euler(new Vector3(0, 180f, 0)));
        else
            Instantiate(eft, new Vector2(transform.position.x + -arrow * x, transform.position.y + y), Quaternion.Euler(new Vector3(0, 0, 0)));

        if (ehp.currentHP <= 0)
        {
            Dead();
            isDead = true;
            notMove = true;
            gameObject.tag = "DeadBody";
        }
        else
        {
            animator.SetTrigger("isHit");
        }        
    }
    
    public void MonsterFlip()
    {
        if (!notMove && !isAtk && !isDead)
        {
            if (isTrace)
            {
                if (playerPos.x < transform.position.x && isFaceRight)
                {
                    Flip();
                }
                else if (playerPos.x > transform.position.x && !isFaceRight)
                {
                    Flip();
                }
            }
            else
            {
                if (randomMove < 0 && isFaceRight)
                {
                    Flip();
                }
                else if (randomMove > 0 && !isFaceRight)
                {
                    Flip();
                }
            }
        }
    }

    public void ActivateMonster(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    void Dead()
    {
        animator.SetTrigger("isDead");
        isDead = true;
    }
}
