using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Hit : AnimatorManager
{
    SpriteRenderer sr;
    Material DefaultMat;
    Material WhiteFlashMat;

    float FlashTimeDelay;
    float FlashTimer;

    private void Awake()
    {
        DefaultMat = Resources.Load<Material>("SpriteDefault");
        WhiteFlashMat = Resources.Load<Material>("WhiteFlash");

        FlashTimeDelay = 0f;
        FlashTimer = 0.2f;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<SpriteRenderer>().material = WhiteFlashMat;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FlashTimeDelay += Time.deltaTime;
        if (FlashTimeDelay >= FlashTimer)
        {
            animator.gameObject.GetComponent<SpriteRenderer>().material = DefaultMat;
            FlashTimeDelay = 0f;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FlashTimeDelay = 0f;
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
