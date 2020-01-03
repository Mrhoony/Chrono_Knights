﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Attack3 : StateMachineBehaviour
{
    GameObject MonsterSelf;
    bool ReadytoAttack;

    float attackPositionX = 0f;
    float attackPositionY = -0.12f;
    float attackRangeX = 1f;
    float attackRangeY = 0.43f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MonsterSelf = animator.gameObject;
        ReadytoAttack = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ReadytoAttack)
        {
            Collider2D[] PlayertoDamage = Physics2D.OverlapBoxAll(new Vector2(MonsterSelf.transform.position.x + attackPositionX, MonsterSelf.transform.position.y + attackPositionY), new Vector2(attackRangeX, attackRangeY), 0);
            for (int i = 0; i < PlayertoDamage.Length; i++)
            {
                if (PlayertoDamage[i].CompareTag("Player"))
                {
                    Debug.Log("Player Hit");
                    //PlayertoDamage[i].GetComponent<PlayerStatus>().DecreaseHP(1f);
                    ReadytoAttack = false;
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MonsterSelf.GetComponent<Animator>().SetBool("isMove", true);
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