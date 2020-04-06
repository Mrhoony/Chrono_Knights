using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeaponType : MonoBehaviour
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

    public abstract void AttackX(int attackArrow);
    public abstract void AttackY(int attackArrow);
    public abstract void JumpAttackX(int attackArrow);

    public void InputInit()
    {
        inputAttackList = 9;
        commandCount = 1;
        attackState = 1;
    }

    public void SetAttackState(int _attackState)
    {
        attackState = _attackState;
    }
}