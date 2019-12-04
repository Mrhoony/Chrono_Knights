using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Frog : Monster_Control
{
    public GameObject box;

    public override void Awake()
    {
        base.Awake();
        
        box = GameObject.Find("Frog_Attack");
        box.SetActive(false);
    }

    public void Start()
    {
        maxRotateDelayTime = randomMoveCount;
        curRotateDelayTime = 0f;
        maxAttackDelayTime = 2f;
        curAttackDelayTime = 0f;
        effectX = 0.3f;
        effectY = 0.3f;
        arrow = 1;
        isFaceRight = true;
        isAtk = false;
    }

    public override void OnEnable()
    {
        ehp.currentHP = ehp.HP;
        StartCoroutine(SearchPlayer());
        isJump = true;
        randomMove = Random.Range(-2, 3);
        randomMoveCount = Random.Range(2f, 3f);
        maxRotateDelayTime = randomMoveCount;
        arrow = 1;
    }

    public void OnDisable()
    {
        StopCoroutine(SearchPlayer());
    }

    // Update is called once per frame
    public new void Update()
    {
        if (isDead) return;
        NotMoveDelayTime();
        if (notMove) return;
        MonsterFlip();
    }

    new void NotMoveDelayTime()
    {
        if (isJump || isAtk)
        {
            curRotateDelayTime += Time.deltaTime;
            if (curRotateDelayTime > maxRotateDelayTime)
            {
                isJump = false;
                isAtk = false;
                notMove = false;
                curRotateDelayTime = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Jump();
        if (!isTrace) return;
        Attack();
    }

    void Jump()
    {
        if (!isJump && !isAtk)
        {
            randomMove = Random.Range(-2, 3);

            if (randomMove != 0)
            {
                isJump = true;

                animator.SetTrigger("isJump");
                animator.SetBool("isJumping", true);
                rb.velocity = new Vector2(1.5f * randomMove, 3f);
                curRotateDelayTime = 0f;
                
                randomMoveCount = Random.Range(2f, 3f);
                maxRotateDelayTime = randomMoveCount;
            }
        }
    }

    void Attack()
    {
        MonsterFlip();
        notMove = true;
        isJump = true;
        curRotateDelayTime = 0f;
        curAttackDelayTime += Time.deltaTime;
        if (curAttackDelayTime > maxAttackDelayTime)
        {
            isAtk = true;
            int AttackType = Random.Range(0, 2);
            if (AttackType == 0)
            {
                animator.SetTrigger("isJump");
                animator.SetBool("isJumping", true);
            }
            else if (AttackType == 1)
            {
                animator.SetTrigger("isAttack");
            }
            curAttackDelayTime = 0f;
        }
    }

    public void AttackStart(float x, float y)
    {
        rb.velocity = new Vector2(x * arrow, y);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider != null)
        {
            if(collider.tag == target.tag)
            {
                PlayerControl.instance.Hit(ehp.Atk);
            }
        }
    }
}
