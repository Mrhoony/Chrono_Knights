using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Dog : MonsterControl
{
    IEnumerator Moving;

    public override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
        maxRotateDelayTime = 2f;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 5f;
        curAttackDelayTime = 0f;
        effectX = 0.2f;
        effectY = 0.3f;
    }

    public void OnDisable()
    {
        StopCoroutine(Moving);
    }

    public void OnEnable()
    {
        ehp.currentHP = ehp.HP;
        StartCoroutine(SearchPlayer());
        randomMoveCount = Random.Range(2f, 4f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
        isFaceRight = false;
        arrow = -1;
    }

    // Update is called once per frame
    void Update()
    {
        MonsterFlip();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Attack();
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

    void Move()
    {
        if (!notMove)
        {
            if (isTrace)
            {
                if (!isAtk && Mathf.Abs(playerPos.x - transform.position.x) > 0.8f)
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
            
            if (Mathf.Abs(playerPos.x - transform.position.x) < 0.8f)
            {
                isAtk = true;
                notMove = true;
                curRotateDelayTime = 0f;
                curAttackDelayTime += Time.fixedDeltaTime;
                if (curAttackDelayTime > maxAttackDelayTime)
                {
                    notMove = true;
                    animator.SetBool("isAtk", true);
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
