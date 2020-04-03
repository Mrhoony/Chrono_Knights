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
        playerControl.GroundCheck.SetActive(false);
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

                if (isGroundCheck > 2f)
                {
                    playerControl.AttackDistance(playerControl.Attack(AtkType.spear_Y_Attack));
                    CameraManager.instance.CameraShake(1);
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
        animator.SetBool("isJump_y_Atk", false);
        animator.SetBool("isJump_up_x_Atk", false);
        animator.SetBool("isJump_down_x_Atk", false);
        playerControl.GroundCheck.SetActive(true);
        playerControl.InputInit();
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        move = false;
    }
}
