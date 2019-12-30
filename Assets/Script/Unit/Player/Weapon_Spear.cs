using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spear : MonoBehaviour
{
    public string ControllerPath = "AnimatorController/Player_Control/Spear/Spear_Controller";
    public bool attackLock;
    public int commandCount;
    public int attackState;
    public int attackPattern;

    public Animator animator;
    public Rigidbody2D rb;

    public Queue inputAttackList = new Queue();

    // Start is called before the first frame update
    void Start()
    {
        commandCount = 1;
        attackState = 1;
    }
    
    public void Init(Animator _animator, Rigidbody2D _rigidbody)
    {
        animator = _animator;
        rb = _rigidbody;
    }

    public void AttackX(int inputArrow)
    {
        if (commandCount <= attackState)
        {
            animator.SetBool("isWalk", false);
            inputAttackList.Enqueue(inputArrow + commandCount);
            ++commandCount;

            if (!attackLock)
                StartCoroutine(AttackList());

            if (commandCount > 3)
                commandCount = 1;
        }
    }
    public void JumpAttackX(int inputArrow)
    {
        inputAttackList.Enqueue(inputArrow + 6);
        if (!attackLock)
            StartCoroutine(AttackList());
    }

    public void AttackY(int inputArrow)
    {
        animator.SetBool("isWalk", false);

        if (attackPattern != 0) inputAttackList.Clear();

        inputAttackList.Enqueue(inputArrow + 5);
        if (!attackLock)
            StartCoroutine(AttackList());
    }
    public void AttackYFinal()
{
    if (animator.GetBool("is_y_Atk"))
    {
        inputAttackList.Enqueue(0);
        if (!attackLock)
            StartCoroutine(AttackList());
    }
}

    public void InputInit()
    {
        inputAttackList.Clear();
        animator.SetBool("is_x_Atk", false);
        animator.SetBool("is_xx_Atk", false);
        animator.SetBool("is_xFx_Atk", false);
        animator.SetBool("is_xxx_Atk", false);
        animator.SetBool("is_xFxFx_Atk", false);
        animator.SetBool("is_y_Atk", false);
        commandCount = 1;
        attackState = 1;
    }

    IEnumerator AttackList()
    {
        attackLock = !attackLock;
        do
        {
            if (rb.velocity.x * rb.velocity.x > 0)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isRun", false);
            }
            switch (inputAttackList.Dequeue())
            {
                case 1:
                case 11:
                    attackPattern = 0;
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 31:
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 41:
                    PlayerControl.instance.actionState = ActionState.IsParrying;
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 2:
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 12:
                    attackPattern = 1;
                    animator.SetBool("is_xFx_Atk", true);
                    break;
                case 32:
                    attackPattern = 2;
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 3:
                    if (attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 13:
                    if (attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 33:
                    if (attackPattern == 1)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else
                        animator.SetBool("is_xxx_Atk", true);
                    break;
                case 5:
                    animator.SetBool("is_y_Atk", true);
                    break;
                case 15:
                case 35:
                case 45:
                    animator.SetBool("is_y_Atk", true);
                    break;
                case 0:
                    animator.SetBool("is_y_up_Atk", true);
                    break;
                case 6:
                case 16:
                case 36:
                case 46:
                    animator.SetBool("isJump_x_Atk", true);
                    break;
            }
            yield return null;
        } while (inputAttackList.Count > 0);
        attackLock = !attackLock;
    }

    public void SetAttackState(int _attackState)
    {
        attackState = _attackState;
    }
    public void AttackMotionCheck()
    {
        if (attackPattern == 0)
        {
            if (!animator.GetBool("is_xx_Atk"))
            {
                InputInit();
                PlayerControl.instance.actionState = ActionState.Idle;
            }
        }
        else if (attackPattern == 1)
        {
            if (!animator.GetBool("is_xFx_Atk"))
            {
                InputInit();
                PlayerControl.instance.actionState = ActionState.Idle;
            }
        }
        else if (attackPattern == 2)
        {
            if (!animator.GetBool("is_xFx_Atk"))
            {
                InputInit();
                PlayerControl.instance.actionState = ActionState.Idle;
            }
        }
    }
}
