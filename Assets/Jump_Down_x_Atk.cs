using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Down_x_Atk : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.2f)
        {
            if (!move)
            {
                move = true;
                playerControl.AttackDistance(playerControl.Attack(AtkType.spear_Jump_Down_X_Attack));
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isJump_down_x_Atk", false);
        playerControl.PlayerJumpAttackEnd();
        playerControl.InputInit();
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        move = false;
    }
}
