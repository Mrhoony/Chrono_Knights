﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Frog_Jump : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<Monster_Frog>().isAtk) {
            if (stateInfo.normalizedTime > 0.4f && atk < 1 && animator.gameObject.GetComponent<Monster_Frog>().isTrace)
            {
                ++atk;
                animator.gameObject.GetComponent<Monster_Frog>().box.SetActive(true);
                animator.gameObject.GetComponent<Monster_Frog>().AttackStart(2f, 3f);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Monster_Frog>().box.SetActive(false);
        animator.gameObject.GetComponent<Monster_Frog>().isAtk = false;
        animator.gameObject.GetComponent<Monster_Frog>().notMove = false;
        atk = 0;
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