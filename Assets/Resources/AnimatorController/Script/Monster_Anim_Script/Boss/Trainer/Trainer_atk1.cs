using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer_atk1 : AnimatorManager
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Init();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!move && stateInfo.normalizedTime > 0.2f)
        {
            move = true;
            animator.GetComponent<Boss_Trainer>().MonsterAttack(MonsterAttackNumber.atk1);
            animator.GetComponent<Boss_Trainer>().AttackMove(0.2f);
        }
    }
}
