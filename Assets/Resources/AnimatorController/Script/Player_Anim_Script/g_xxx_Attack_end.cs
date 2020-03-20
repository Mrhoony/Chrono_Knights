using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_xxx_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                move = true;
                playerControl.InstantiateGunEft(GunEft.downshot);
                playerControl.AttackDistanceForce(playerControl.Attack(0.5f, 0f, 1f, 1f, AtkType.g_backJump));
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerControl.InputInit();
        playerControl.MoveSet();
        Init();
        playerControl.multyHitCount = 0;
    }
}
