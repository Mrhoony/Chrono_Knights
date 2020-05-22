using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RollingBug_Rolling : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        move = false;
        animator.GetComponent<Monster_RollingBug>().RollingCount();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster_RollingBug>().Rolling();
        
        Collider2D[] player = Physics2D.OverlapBoxAll(
            new Vector2(
                animator.gameObject.transform.position.x, 
                animator.gameObject.transform.position.y + 0.1f), 
            new Vector2(0.4f, 0.4f), 8);

        if (move) return;

        if (player != null)
        {
            for (int i = 0; i < player.Length; ++i)
            {
                if (player[i].CompareTag("Player"))
                {
                    move = true;
                    animator.GetComponent<Monster_RollingBug>().MonsterAttack(MonsterAttackNumber.atk1);
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
