using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xx_Attack_end : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.1f && atk < 1)
        {
            Attack(0.5f, 0f, 1f, 0.5f);
        }
        if (stateInfo.normalizedTime > 0.1f)
        {
            PlayerControl.instance.atkState = 3;
            if (!move)
            {
                PlayerControl.instance.rb.velocity = new Vector2(PlayerControl.instance.pStat.moveSpeed * PlayerControl.instance.arrowDirection, PlayerControl.instance.rb.velocity.y);

                move = true;
            }
        }
        PlayerControl.instance.notMove = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!PlayerControl.instance.animator.GetBool("is_xxx_Atk"))
        {
            PlayerControl.instance.InputInit();
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
