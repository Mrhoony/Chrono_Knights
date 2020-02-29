using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponType : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController animatorController;
    public Rigidbody2D rb;
    
    public bool attackLock;
    public int commandCount;
    public int attackState;
    public int attackPattern;

    public int inputAttackList;

    public void Init(Animator _animator, Rigidbody2D _rigidbody)
    {
        animator = _animator;
        animator.runtimeAnimatorController = animatorController;
        rb = _rigidbody;
        commandCount = 1;
        attackState = 1;
        inputAttackList = 9;
    }

    public void InputInit()
    {
        inputAttackList = 9;
        animator.SetBool("is_x_Atk", false);
        animator.SetBool("is_xx_Atk", false);
        animator.SetBool("is_xFx_Atk", false);
        animator.SetBool("is_xxx_Atk", false);
        animator.SetBool("is_xFxFx_Atk", false);
        commandCount = 1;
        attackState = 1;
    }

    public void SetAttackState(int _attackState)
    {
        attackState = _attackState;
    }
}
