using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : StateMachineBehaviour
{
    public Collider2D[] monster;
    public Collider2D[] player;
    public int atk;
    public bool move;

    public void Init()
    {
        atk = 0;
        move = false;
        PlayerControl.instance.notMove = true;
    }

    public void Attack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        ++atk;
        monster = Physics2D.OverlapBoxAll(new Vector2(PlayerControl.instance.transform.position.x + attackPosX * PlayerControl.instance.arrowDirection
            , PlayerControl.instance.transform.position.y + attackPosY), new Vector2(0.6f, 0.5f), 10);
        foreach (Collider2D mon in monster)
        {
            if (mon.tag == "Monster")
            {
                mon.gameObject.GetComponent<MonsterControl>().Hit(PlayerControl.instance.pStat.Atk
                    , mon.gameObject.GetComponent<MonsterControl>().effectX, mon.gameObject.GetComponent<MonsterControl>().effectY);
            }
            else if (mon.tag == "BossMonster")
            {
                mon.gameObject.GetComponent<BossMonsterControl>().Hit(PlayerControl.instance.pStat.Atk
                    , mon.gameObject.GetComponent<BossMonsterControl>().effectX, mon.gameObject.GetComponent<BossMonsterControl>().effectY);
            }
        }
    }

}
