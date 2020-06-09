using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Up_x_Atk : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                move = true;
                animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * 1f;
                playerControl.Attack(AtkType.spear_Jump_Up_X_Attack, playerControl.AttackDistance(AtkType.spear_Jump_Up_X_Attack));
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isJump_up_x_attack", false);
        if (!animator.GetBool("isJump_y_attack"))
        {
            playerControl.PlayerJumpAttackEnd();
            playerControl.InputInit();
            animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
        move = false;
    }
}
