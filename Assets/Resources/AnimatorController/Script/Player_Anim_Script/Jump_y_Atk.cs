using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_y_Atk : AnimatorManager
{
    float isGroundCheck;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isGroundCheck = 0f;
        Init();
        animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerControl.GroundCheck.SetActive(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                move = true;
                isGroundCheck = playerControl.AttackDistanceDown();

                if (isGroundCheck > 2f)
                {
                    playerControl.Attack(AtkType.spear_Y_Attack, isGroundCheck);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (20f >= isGroundCheck)
        {
            animator.SetBool("isJump", false);
            playerControl.PlayerJumpAttackEnd();
        }
        playerControl.GroundCheck.SetActive(true);
        playerControl.InputInit();
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        move = false;
    }
}
