using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_y_Atk : AnimatorManager
{
    bool isgroundCheck;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isgroundCheck = false;
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
                isgroundCheck = playerControl.AttackDistanceDown(playerControl.Attack(AtkType.spear_JumpX_Attack));
                animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (true == isgroundCheck) {
            animator.SetBool("isJump", false);
        }
        animator.SetBool("isJump_y_Atk", false);
        playerControl.InputInit();
        move = false;


    }
}
