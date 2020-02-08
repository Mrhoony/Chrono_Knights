using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    IsDead,
    NotMove,
    IsJump,
    IsJumpAttack,
    IsAtk,
    IsParrying,
    IsMove,
    Idle
}
public enum State
{
    common, poison, bleeding, slow
}

public abstract class MovingObject : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

    public string currentState;
    
    public ActionState actionState;

    public bool isFaceRight; // 보는 방향
    public int arrowDirection;

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
