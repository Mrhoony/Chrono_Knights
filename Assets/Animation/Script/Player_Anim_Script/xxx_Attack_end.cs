using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xxx_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f && atk < 2)
        {
            ++atk;
            monster = Physics2D.OverlapBoxAll(new Vector2(PlayerControl.instance.transform.position.x + 0.5f * PlayerControl.instance.arrowDirection
                , PlayerControl.instance.transform.position.y), new Vector2(1f, 1f), 10);

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
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                PlayerControl.instance.transform.position = new Vector2((PlayerControl.instance.transform.position.x + 0.3f * PlayerControl.instance.arrowDirection), PlayerControl.instance.transform.position.y);
                move = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerControl.instance.atkState = 1;
        PlayerControl.instance.InputInit();
        Init();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
