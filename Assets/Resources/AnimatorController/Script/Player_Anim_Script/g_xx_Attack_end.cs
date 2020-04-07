using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_xx_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.1f)
        {
            if (!move)
            {
                move = true;
                playerControl.InstantiateGunEft(GunEft.shot2);
                playerControl.AttackDistance(playerControl.Attack(AtkType.gun_XX_Attack));
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("is_xxx_attack"))
        {
            playerControl.InputInit();
            playerControl.MoveSet();
        }
        move = false;
    }
}
