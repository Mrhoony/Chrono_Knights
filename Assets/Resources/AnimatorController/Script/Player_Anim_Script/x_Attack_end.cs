using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class x_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimationInit();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.1f && multyHitCount < 1)
        {
            multyHitCount++;
            playerControl.Attack(0.5f, 0f, 1f, 0.5f);
        }
        if(stateInfo.normalizedTime > 0.4f)
        {
            playerControl.SetAttackState(2);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("is_xx_Atk"))
        {
            Debug.Log("xx Init");
        }
        else if (animator.GetBool("is_xFx_Atk"))
        {
            Debug.Log("xFx Init");
        }
        else
        {
            playerControl.InputInit();
            playerControl.PlayerMoveSet();
        }
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
