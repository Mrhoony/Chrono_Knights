using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
    public GameObject box;
    public GameObject eft;
    public EnemyStat ehp;

    public float effectX;
    public float effectY;
    public float random;

    public bool isTrace;
    public int randomMove;
    public float randomMoveCount;
    public float randomAttack;
    public bool isAtk;
    public bool isJump;
    public bool isDamagable;
    public int degreeOfRisk;
    
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

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ehp = GetComponent<EnemyStat>();

        target = GameObject.Find("Player Character");
    }

    public void Hit(float playerAtk, float x, float y)
    {
        if (isDead || isDamagable)
            return;

        isDamagable = true;
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
        if (!notMove && !isDead)
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
        DungeonManager.instance.MonsterKill();
        isDead = true;
    }
}
