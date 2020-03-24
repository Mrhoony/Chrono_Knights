using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_xFx_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        playerControl.multyHitCount = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                move = true;
                playerControl.AttackDistance(playerControl.Attack(AtkType.gun_XFX_Attack));
            }
        }
        if (stateInfo.normalizedTime > 0.4f)
        {
            playerControl.SetAttackState(3);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("is_xFxFx_Atk"))
        {
            playerControl.InputInit();
            playerControl.MoveSet();
        }
        Init();
        playerControl.multyHitCount = 0;
    }
}
