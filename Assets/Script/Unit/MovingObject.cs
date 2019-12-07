﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

    protected enum State
    {
        common, poison, bleeding, slow
    }
    private string currentState;

    public enum ActionState
    {
        IsDead,
        NotMove,
        IsJump,
        IsAtk,
        IsParrying,
        Idle
    }

    public ActionState actionState = ActionState.Idle; 

    protected bool isFaceRight; // 보는 방향
    protected int arrowDirection;

    public void Flip()
    {
        isFaceRight = !isFaceRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        arrowDirection *= -1;
    }

    public string StateInfo
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public int GetArrowDirection()
    {
        return arrowDirection;
    }
    public ActionState GetActionState()
    {
        return actionState;
    }
}
