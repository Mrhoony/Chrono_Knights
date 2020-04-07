using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_Jump_x_Atk : AnimatorManager
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime < 0.9f)
        {
            if (!move)
            {
                move = true;
                playerControl.InstantiateGunEft(GunEft.downshot);
                playerControl.AttackDistance(playerControl.Attack(AtkType.gun_JumpX_Attack));
            }
        }
        else
        {
            animator.SetBool("isFall", true);
            animator.SetBool("isJump_x_attack", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsFall", false);
        playerControl.InputInit();
        playerControl.PlayerJumpAttackEnd();
        move = false;
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
    }
    
}
