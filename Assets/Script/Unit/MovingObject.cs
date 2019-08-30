using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public enum state
    {
        common, poison, bleeding, slow
    }

    public bool isFaceRight; // 보는 방향

    private string currentState;

    public bool isDead;
    public bool notMove;

    public Rigidbody2D rb;
    public Animator animator;
    public int arrow;

    public void Flip()
    {
        isFaceRight = !isFaceRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        arrow *= -1;
        notMove = false;
    }

    public string StateInfo
    {
        get { return currentState; }
        set { currentState = value; }
    }
}
