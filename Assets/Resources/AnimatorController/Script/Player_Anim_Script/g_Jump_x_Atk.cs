using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_Jump_x_Atk : AnimatorManager
{
    float velY;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
        playerControl.multyHitCount = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        velY = animator.gameObject.GetComponent<Rigidbody2D>().velocity.y;
        if (0 > velY) animator.SetBool("isFall", true);

        if (stateInfo.normalizedTime < 0.9f) {
            if (!move)
            {
                move = true;
                playerControl.InstantiateGunEft(GunEft.downshot);
                playerControl.AttackDistance(playerControl.Attack(1f, 0f, 2f, 0.5f, AtkType.notMove));
            }
            animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0.4f);
        }
        if (stateInfo.normalizedTime > 0.9f) {
            animator.SetBool("isJump_x_Atk", false);
            PlayerControl.instance.PlayerJumpAttackEnd();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerControl.InputInit();
        playerControl.MoveSet();
        Init();
        playerControl.multyHitCount = 0;
    }
    
}
