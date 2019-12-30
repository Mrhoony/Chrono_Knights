using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Goblin_DashAtk : AnimatorManager
{
    Monster_Goblin monster_Goblin;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster_Goblin = animator.gameObject.GetComponent<Monster_Goblin>();
        atk = 0;
        move = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MonsterAttack(animator, stateInfo, 0.5f, 0.5f, 0f, 0.8f, 0.2f);

        if (stateInfo.normalizedTime > 0.4f)
        {
            if (!move)
            {
                monster_Goblin.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3f * monster_Goblin.GetArrowDirection(), monster_Goblin.gameObject.GetComponent<Rigidbody2D>().velocity.y);
                move = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        atk = 0;
    }

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
