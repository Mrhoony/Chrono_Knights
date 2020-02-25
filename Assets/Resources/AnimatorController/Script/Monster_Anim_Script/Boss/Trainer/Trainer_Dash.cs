using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer_Dash : AnimatorManager
{
    Collider2D[] player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("dash");
        animator.GetComponent<Boss_Trainer>().Dash();
        player = Physics2D.OverlapBoxAll(
            new Vector2(animator.gameObject.transform.position.x + (0.5f * animator.gameObject.GetComponent<BossMonster_Control>().GetArrowDirection())
            , (animator.gameObject.transform.position.y)), new Vector2(0.6f, 0.2f), 8);

        if (player != null)
        {
            for (int i = 0; i < player.Length; ++i)
            {
                if (player[i].CompareTag("Player"))
                {
                    Debug.Log("catch");
                    animator.gameObject.GetComponent<Boss_Trainer>().DashAttack();
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
