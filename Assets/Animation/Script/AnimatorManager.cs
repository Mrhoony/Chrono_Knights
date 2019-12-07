using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : StateMachineBehaviour
{
    public Collider2D[] monster;
    public Collider2D[] player;
    public PlayerControl playerControl;
    public int atk;
    public bool move;

    public void Init()
    {
        atk = 0;
        move = false;
        playerControl = PlayerControl.instance;
    }

    public void Attack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        ++atk;
        
        monster = Physics2D.OverlapBoxAll(new Vector2(playerControl.transform.position.x + (attackPosX * playerControl.GetArrowDirection()), playerControl.transform.position.y + attackPosY), new Vector2(attackRangeX, attackRangeY), 0);

        if(monster != null)
        {
            for (int i = 0; i < monster.Length; ++i)
            {
                if (monster[i].CompareTag("Monster"))
                {
                    monster[i].gameObject.GetComponent<IsDamageable>().MonsterHit(playerControl.playerStatus.attack);
                }
                else if (monster[i].CompareTag("BossMonster"))
                {
                    monster[i].gameObject.GetComponent<IsDamageable>().MonsterHit(playerControl.playerStatus.attack);
                }
            }
        }
    }

    public void MonsterAttack(Animator animator, AnimatorStateInfo stateInfo, float attackTime, float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        if (stateInfo.normalizedTime > attackTime && atk < 1)
        {
            ++atk;
            player = Physics2D.OverlapBoxAll(
                new Vector2(animator.gameObject.transform.position.x + (attackPosX * animator.gameObject.GetComponent<Monster_Control>().GetArrowDirection())
                , (animator.gameObject.transform.position.y))
                , new Vector2(attackRangeX, attackRangeY), 8);
            
            if(player != null)
            {
                for (int i = 0; i < player.Length; ++i)
                {
                    if (player[i].CompareTag("Player"))
                    {
                        player[i].gameObject.GetComponent<IsDamageable>().PlayerHit(animator.gameObject.GetComponent<EnemyStat>().GetAttack());
                    }
                }
            }
        }
    }
}
