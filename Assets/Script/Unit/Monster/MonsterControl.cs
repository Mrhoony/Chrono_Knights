using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    monster, bossMonster
}

public abstract class Monster_Control : MovingObject
{
    public Collider2D[] player;
    public GameObject target;
    public Vector2 playerPos;
    public GameObject eft;
    public EnemyStatus enemyStatus;
    public DropItemList dropItemList;

    public string monsterName;
    public int monsterCode;

    public float distanceX;
    public float distanceY;

    protected float random;
    public float moveSpeed;
    
    public float rotateDelayTime;
    public float attackCoolTime;
    public float maxAttackDelayTime;
    public float curAttackDelayTime;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyStatus = GetComponent<EnemyStatus>();
        dropItemList = GetComponent<DropItemList>();
        eft = transform.GetChild(0).gameObject;
        target = GameObject.Find("PlayerCharacter");
    }
    // Update is called once per frame
    public void Update()
    {
        if (actionState == ActionState.IsDead) return;
        if (actionState != ActionState.Idle) return;
        MonsterFlip();
    }

    // 몬스터 행동시 딜레이
    public IEnumerator MoveDelayTime(float time)
    {
        yield return new WaitForSeconds(time);

        actionState = ActionState.Idle;
    }
    // 몬스터 공격 판정
    public void MonsterAttack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        player = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x + (attackPosX * GetArrowDirection()), transform.position.y), new Vector2(attackRangeX, attackRangeY), 8);

        if (player != null)
        {
            overlap = player.Length;
            for (int i = 0; i < overlap; ++i)
            {
                if (player[i].CompareTag("Player"))
                {
                    player[i].gameObject.GetComponent<IsDamageable>().Hit(gameObject.GetComponent<EnemyStatus>().GetAttack());
                }
            }
        }
    }

    public abstract void MonsterFlip();
    public abstract void MonsterHit(int damage);
}
