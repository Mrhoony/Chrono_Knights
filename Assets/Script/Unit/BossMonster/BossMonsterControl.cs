using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster_Control : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
    public GameObject eft;
    public EnemyStatus enemyStatus;
    public DropItemList dropItemList;
    
    public float distanceX;
    public float distanceY;
    public bool playerPosition;
    protected float random;

    public float moveSpeed;
    public bool isDamagable;
    public bool isGuard;
    public int counter;
    public bool Invincible;

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

    // Start is called before the first frame update
    public void OnEnable()
    {
        target = GameObject.Find("PlayerCharacter");
        BossMonsterInit();
    }

    public void BossMonsterInit()
    {
        Debug.Log("BossMonsterInit");

        actionState = ActionState.Idle;
        enemyStatus.MonsterInit();
        moveSpeed = enemyStatus.GetMoveSpeed();
        StartCoroutine(SearchPlayerBoss());
    }

    public IEnumerator SearchPlayerBoss()
    {
        while (actionState != ActionState.IsDead)
        {
            playerPos = target.transform.position;
            distanceX = playerPos.x - transform.position.x;
            distanceY = playerPos.y - transform.position.y;

            if (distanceX < 0)
            {
                distanceX *= -1f;
                playerPosition = true;
            }
            else
            {
                playerPosition = false;
            }

            if (distanceY < 0) distanceY *= -1f;

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState != ActionState.Idle) return;
        MonsterFlip();
    }

    public void MonsterFlip()
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

    public void MonsterHit(int attack)
    {
        if (actionState == ActionState.IsDead) return;

        if (isGuard)
        {
            animator.SetTrigger("isCounterTrigger");
        }
        else
        {
            actionState = ActionState.NotMove;
            StartCoroutine(MoveDelayTime(1f));
            random = Random.Range(-2f, 2f);
            rb.velocity = Vector2.zero;

            if(playerPosition)
                rb.AddForce(new Vector2(1f + random * 0.1f, 0.2f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(-1f * PlayerControl.instance.GetArrowDirection() + random * 0.1f, 0.2f), ForceMode2D.Impulse);

            enemyStatus.DecreaseHP(attack);
            if (enemyStatus.IsDeadCheck())
            {
                actionState = ActionState.IsDead;
                gameObject.tag = "DeadBody";
                DungeonManager.instance.FloorBossKill();
            }
            else
            {
                animator.SetTrigger("isHit");
                eft.SetActive(true);
            }
        }
    }
    public IEnumerator MoveDelayTime(float time)
    {
        yield return new WaitForSeconds(time);

        actionState = ActionState.Idle;
    }

    public void AttackMove(float moveDistance)
    {
        transform.position = new Vector2(transform.position.x + moveDistance * arrowDirection, transform.position.y);
    }
}
