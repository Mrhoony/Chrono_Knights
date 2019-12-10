using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Control : MovingObject
{
    protected GameObject target;
    protected Vector2 playerPos;
    protected GameObject eft;
    protected EnemyStat ehp;
    public DropItemList dil;

    public float distanceX;
    public float distanceY;
    
    protected float random;

    protected IEnumerator Moving;

    public bool isTrace;
    public int randomMove;
    public float randomMoveCount;
    public float randomAttack;
    public bool isDamagable;

    public float maxRotateDelayTime;
    public float curRotateDelayTime;
    public float maxAttackDelayTime;
    public float curAttackDelayTime;
    
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ehp = GetComponent<EnemyStat>();
        eft = transform.GetChild(0).gameObject;
        dil = GetComponent<DropItemList>();
    }

    public virtual void Update()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState == ActionState.NotMove) return;
        NotMoveDelayTime();
        if (actionState == ActionState.IsAtk) return;
        MonsterFlip();
    }

    public virtual void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        MonsterInit();
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

            if (distanceX < 2f && distanceY < 2f)
            {
                if (!isTrace)
                {
                    isTrace = true;
                    curRotateDelayTime = 0f;
                    StopCoroutine(Moving);
                }
            }
            else if(distanceX > 2f || distanceY > 2f)
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
    public IEnumerator SearchPlayerBoss()
    {
        while (actionState != ActionState.IsDead)
        {
            playerPos = target.transform.position;
            distanceX = playerPos.x - transform.position.x;
            distanceY = playerPos.y - transform.position.y;

            if (distanceX < 0) distanceX *= -1f;
            if (distanceY < 0) distanceY *= -1f;
            
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
    
    public void MonsterInit()
    {
        actionState = ActionState.Idle;
        ehp.SetCurrentHP();
        StartCoroutine(SearchPlayer());

        randomMoveCount = Random.Range(2f, 3f);
        Moving = RandomMove(randomMoveCount);
        StartCoroutine(Moving);
    }
    public void BossMonsterInit()
    {
        actionState = ActionState.Idle;
        ehp.SetCurrentHP();
        StartCoroutine(SearchPlayerBoss());
    }

    public void MonsterFlip()
    {
        if (actionState != ActionState.IsAtk)
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
    public void NotMoveDelayTime()
    {
        if (actionState == ActionState.IsAtk)
        {
            curAttackDelayTime = 0;
            curRotateDelayTime += Time.deltaTime;
            if (curRotateDelayTime > maxRotateDelayTime)
            {
                actionState = ActionState.Idle;
                curRotateDelayTime = 0f;
            }
        }
    }

    public void MonsterHit(int attack)
    {
        if (actionState == ActionState.IsDead) return;

        random = Random.Range(-1f, 1f);

        ehp.DecreaseHP(attack);

        actionState = ActionState.NotMove;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1f * PlayerControl.instance.GetArrowDirection() + random * 0.1f, 0f), ForceMode2D.Impulse);

        if (ehp.GetCurrentHP() <= 0)
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
        animator.SetTrigger("isDead");
        StopCoroutine(Moving);
        //duneonManager.MonsterDie();
        if(dil != null)
        {
            dil.ItemDropChance();
        }
    }
}
