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
        arrowDirection = 1;
        isFaceRight = true;
        actionState = ActionState.Idle;
    }

    public override void OnEnable()
    {
        ehp.SetCurrentHP();
        StartCoroutine(SearchPlayer());
        randomMove = Random.Range(-2, 3);
        randomMoveCount = Random.Range(2f, 3f);
        maxRotateDelayTime = randomMoveCount;
    }

    public void OnDisable()
    {
        StopCoroutine(SearchPlayer());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    new void NotMoveDelayTime()
    {
        if (actionState == ActionState.IsAtk)
        {
            curRotateDelayTime += Time.deltaTime;
            if (curRotateDelayTime > maxRotateDelayTime)
            {
                actionState = ActionState.Idle;
                curRotateDelayTime = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead) return;
        Jump();
        if (!isTrace) return;
        Attack();
    }

    void Jump()
    {
        if (actionState != ActionState.IsAtk)
        {
            randomMove = Random.Range(-2, 3);

            if (randomMove != 0)
            {
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
        actionState = ActionState.NotMove;
        curRotateDelayTime = 0f;
        curAttackDelayTime += Time.deltaTime;
        if (curAttackDelayTime > maxAttackDelayTime)
        {
            actionState = ActionState.IsAtk;
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
        rb.velocity = new Vector2(x * arrowDirection, y);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider != null)
        {
            if(collider.tag == target.tag)
            {
                PlayerControl.instance.Hit(ehp.GetAttack());
            }
        }
    }
}
