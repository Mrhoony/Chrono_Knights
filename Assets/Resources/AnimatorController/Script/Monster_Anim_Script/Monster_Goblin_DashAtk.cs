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
        move = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!move && stateInfo.normalizedTime > 0.7f)
        {
            move = true;
            monster_Goblin.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3f * monster_Goblin.GetArrowDirection(), monster_Goblin.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            monster_Goblin.MonsterAttack(MonsterAttackNumber.atk1);
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
