using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Control : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
    public GameObject eft;
    public EnemyStatus enemyStatus;
    public DropItemList dropItemList;

    public float distanceX;
    public float distanceY;
    
    protected float random;
    
    public float moveSpeed;
    protected IEnumerator Moving;

    public bool isTrace;
    public int randomMove;
    public float randomMoveCount;
    public float randomAttack;
    public bool isDamagable;

    public float rotateDelayTime;
    public float attackCoolTime;
    public float maxAttackDelayTime;
    public float curAttackDelayTime;
    
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyStatus = GetComponent<EnemyStatus>();
        dropItemList = GetComponent<DropItemList>();
        eft = transform.GetChild(0).gameObject;
    }

    public void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        MonsterInit();
    }

    public void OnDisable()
    {
        StopCoroutine(Moving);
    }

    public virtual void Update()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState != ActionState.Idle) return;
        MonsterFlip();
    }
    
    public IEnumerator SearchPlayer()
    {
        while (actionState != ActionState.IsDead)
        {
            playerPos = target.transform.position;
            distanceX = playerPos.x - transform.position.x;
            distanceY = playerPos.y - transform.position.y;

            if (distanceX < 0) distanceX *= -1f;
            if (distanceY < 0) distanceY *= -1f;

            if (distanceX < 3f && distanceY < 2f)
            {
                if (!isTrace)
                {
                    isTrace = true;
                    StopCoroutine(Moving);
                }
            }
            else if(distanceX > 3f || distanceY > 2f)
            {
                if (isTrace)
                {
                    actionState = ActionState.Idle;
                    isTrace = false;
                    StartCoroutine(Moving);
                    curAttackDelayTime = 0f;
                }
            }
            yield return null;
        }
    }
    public IEnumerator RandomMove(float time)
    {
        randomMove = Random.Range(-1, 2);
        actionState = ActionState.Idle;

        yield return new WaitForSeconds(time);

        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
    }
    public IEnumerator MoveDelayTime(float time)
    {
        yield return new WaitForSeconds(time);

        actionState = ActionState.Idle;
    }

    public void MonsterInit()
    {
        Debug.Log("MonsterInit");
        
        actionState = ActionState.Idle;
        enemyStatus.MonsterInit();
        moveSpeed = enemyStatus.GetMoveSpeed();
        StartCoroutine(SearchPlayer());

        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
    }
    public void MonsterFlip()
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

    public void MonsterHit(int attack)
    {
        if (actionState == ActionState.IsDead) return;
        actionState = ActionState.NotMove;
        StartCoroutine(MoveDelayTime(1f));
        random = Random.Range(-0.2f, 0.2f);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(PlayerControl.instance.GetArrowDirection() + random, 0.2f), ForceMode2D.Impulse);

        enemyStatus.DecreaseHP(attack);

        if (enemyStatus.IsDeadCheck())
        {
            Dead();
            actionState = ActionState.IsDead;
            gameObject.tag = "DeadBody";
        }
        else
        {
            animator.SetTrigger("isHit");
            eft.SetActive(true);
        }
    }
    public void Landing()
    {
        animator.SetBool("isJumping", false);
    }
    
    void Dead()
    {
        animator.SetBool("isDead", true);
        StopCoroutine(Moving);
        //duneonManager.MonsterDie();
        if(dropItemList != null)
        {
            dropItemList.ItemDropChance();
        }
        DeadAnimation();
    }

    public void DeadAnimation()
    {
        gameObject.SetActive(false);
    }
}
