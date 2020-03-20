﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_x_Attack_end : AnimatorManager
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
        if (stateInfo.normalizedTime > 0.1f)
        {
            if (!move)
            {
                move = true;
                playerControl.InstantiateGunEft(GunEft.shot1);
                playerControl.Attack(0.5f, 0f, 1f, 0.5f, AtkType.notMove);
            }
        }
        if (stateInfo.normalizedTime > 0.4f)
        {
            playerControl.SetAttackState(2);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("is_xx_Atk") && !animator.GetBool("is_xFx_Atk"))
        {
            playerControl.InputInit();
            playerControl.MoveSet();
        }
        Init();
        playerControl.multyHitCount = 0;
    }
}
