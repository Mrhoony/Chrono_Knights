using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_y_Atk : AnimatorManager
{
    bool isGroundCheck;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isGroundCheck = false;
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
                animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isGroundCheck = playerControl.AttackDistanceDown(playerControl.Attack(AtkType.spear_Jump_Y_Attack));
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (true == isGroundCheck)
        {
            animator.SetBool("isJump", false);
        }
        animator.SetBool("isJump_y_Atk", false);
        playerControl.PlayerJumpAttackEnd();
        playerControl.Landing();
        playerControl.InputInit();
        move = false;
    }
}
