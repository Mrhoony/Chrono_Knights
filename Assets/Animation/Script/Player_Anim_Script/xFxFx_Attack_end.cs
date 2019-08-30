﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xFxFx_Attack_end : AnimatorManager
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
            Attack(1f, 0f, 2f, 0.5f);
        }

        if(stateInfo.normalizedTime > 0.1f)
        {
            if (!move)
            {
                PlayerControl.instance.rb.velocity = new Vector2((PlayerControl.instance.pStat.moveSpeed * PlayerControl.instance.arrowDirection), PlayerControl.instance.transform.position.y);
                move = true;
            }
        }
        if (stateInfo.normalizedTime > 0.9f)
            PlayerControl.instance.isAtk = false;
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
