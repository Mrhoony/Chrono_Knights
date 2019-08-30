using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterControl : MovingObject
{
    public GameObject target;
    public Vector2 playerPos;
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

    public float maxRotateDelayTime;
    public float curRotateDelayTime;
    public float maxAttackDelayTime;
    public float curAttackDelayTime;
    
    public IEnumerator SearchPlayer()
    {
        while (!isDead)
        {
            playerPos = target.transform.position;
            yield return null;
        }
    }

    public void Hit(int playerAtk, float x, float y)
    {
        if (isDead || isDamagable || isAtk)
            return;

        isDamagable = true;
        random = Random.Range(-1f, 1f);
        
        notMove = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1f * PlayerControl.instance.arrowDirection + random * 0.1f, 1f + random * 0.1f), ForceMode2D.Impulse);

        if (arrow > 0)
            Instantiate(eft, new Vector2(transform.position.x + -arrow * x, transform.position.y + y), Quaternion.Euler(new Vector3(0, 180f, 0)));
        else
            Instantiate(eft, new Vector2(transform.position.x + -arrow * x, transform.position.y + y), Quaternion.Euler(new Vector3(0, 0, 0)));

        ehp.DecreaseHP(playerAtk);
    }

    public void MonsterFlip()
    {
        if (!notMove && !isDead)
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
    }

    public void ActivateMonster(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    void Dead()
    {
        animator.SetTrigger("isDead");
        isDead = true;
    }
}
