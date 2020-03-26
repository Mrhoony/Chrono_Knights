using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    monster, bossMonster
}
public enum MonsterAttackNumber {
    atk1, atk2, atk3,
}

[System.Serializable]
public class AttackRange {
    public float posX;
    public float posY;
    public float atkRangeX;
    public float atkRangeY;
}

public abstract class Monster_Control : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
    public GameObject eft;
    public List<GameObject> eftPool;
    public EnemyStatus enemyStatus;
    public DropItemList dropItemList;

    public string monsterName;
    public int monsterCode;
    public bool coroutine;
    public bool moveDelay;
    public bool hitDelay;

    public float distanceX;
    public float distanceY;

    protected float random;
    public float moveSpeed;
    public int monsterWeight;
    
    public float rotateDelayTime;
    public float attackCoolTime;
    public float maxAttackDelayTime;

    public delegate void MonsterDeadCount();
    public MonsterDeadCount monsterDeadCount;
    //posx, posy, rangex, rangey

    public AttackRange[] attackRange;

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
        enabled = false;
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
    public virtual void MonsterAttack(MonsterAttackNumber mat)
    {
        Collider2D[] player = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x + (attackRange[(int)mat].posX + attackRange[(int)mat].atkRangeX * 0.5f ) * GetArrowDirection()
            , transform.position.y + (attackRange[(int)mat].atkRangeY * 0.5f)), new Vector2(attackRange[(int)mat].atkRangeX, attackRange[(int)mat].atkRangeY), 8);

        if (player != null)
        {
            overlap = player.Length;
            for (int i = 0; i < overlap; ++i)
            {
                if (player[i].CompareTag("Player"))
                {
                    player[i].gameObject.GetComponent<PlayerControl>().Hit(gameObject.GetComponent<EnemyStatus>().GetAttack());
                }
            }
        }
    }

    public abstract void MonsterInit();
    public abstract void MonsterFlip();
    public abstract void MonsterHit(int _damage, int _knockBack);
    public abstract void Dead();

    public void OnDestroy()
    {
        enemyStatus.HPbarReset();
    }
}
