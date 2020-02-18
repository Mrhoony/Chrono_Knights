using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : StateMachineBehaviour
{
    public Collider2D[] monster;
    public Collider2D[] player;
    public PlayerControl playerControl;
    public int atk;
    public int overlap;
    public bool move;

    public void Init()
    {
        move = false;
        playerControl = PlayerControl.instance;
        atk = 0;
    }

    public void Attack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        ++atk;
        
        monster = Physics2D.OverlapBoxAll(new Vector2(playerControl.transform.position.x + (attackPosX * playerControl.GetArrowDirection())
            , playerControl.transform.position.y + attackPosY), new Vector2(attackRangeX, attackRangeY), 0);

        if(monster != null)
        {
            overlap = monster.Length;
            for (int i = 0; i < overlap; ++i)
            {
                if (monster[i].CompareTag("Monster"))
                {
                    monster[i].gameObject.GetComponent<IsDamageable>().Hit(playerControl.playerStatus.GetAttack_Result());
                }
            }
        }
    }

    public void MonsterAttack(Animator animator, AnimatorStateInfo stateInfo, float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        ++atk;
        player = Physics2D.OverlapBoxAll(
            new Vector2(animator.gameObject.transform.position.x + (attackPosX * animator.gameObject.GetComponent<Monster_Control>().GetArrowDirection())
            , (animator.gameObject.transform.position.y))
            , new Vector2(attackRangeX, attackRangeY), 8);

        if (player != null)
        {
            overlap = player.Length;
            for (int i = 0; i < overlap; ++i)
            {
                if (player[i].CompareTag("Player"))
                {
                    player[i].gameObject.GetComponent<IsDamageable>().Hit(animator.gameObject.GetComponent<EnemyStatus>().GetAttack());
                }
            }
        }
    }
}
