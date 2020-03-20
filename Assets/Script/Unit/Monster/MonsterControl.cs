using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    monster, bossMonster
}

public abstract class Monster_Control : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
    public GameObject eft;
    public List<GameObject> eftPool;
    public EnemyStatus enemyStatus;
    public DropItemList dropItemList;
    public SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    public Material whiteFlashMaterial;

    public string monsterName;
    public int monsterCode;
    public bool coroutine;
    public bool moveDelay;
    public bool hitDelay;

    public float distanceX;
    public float distanceY;

    protected float random;
    public float moveSpeed;
    
    public float rotateDelayTime;
    public float attackCoolTime;
    public float maxAttackDelayTime;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyStatus = GetComponent<EnemyStatus>();
        dropItemList = GetComponent<DropItemList>();
        eft = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = Resources.Load<Material>("Material/SpriteDefault");
        whiteFlashMaterial = Resources.Load<Material>("Material/WhiteFlash");
        target = GameObject.Find("PlayerCharacter");
    }

    public void MonsterStop()
    {
        StopAllCoroutines();
        actionState = ActionState.Idle;
        spriteRenderer.material = defaultMaterial;
    }

    // 몬스터 행동시 딜레이
    public IEnumerator MoveDelayTime(float time)
    {
        if (moveDelay) yield break;
        moveDelay = true;

        yield return new WaitForSeconds(time);
        actionState = ActionState.Idle;

        moveDelay = false;
    }

    // 몬스터 공격 판정
    public virtual void MonsterAttack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        Collider2D[] player = Physics2D.OverlapBoxAll(
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
    public abstract void Dead();
}
