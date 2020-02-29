using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : PlayerWeaponType
{
    public void AttackX(int inputArrow)
    {
        if (commandCount <= attackState)
        {
            Debug.Log("input");
            animator.SetBool("isWalk", false);
            inputAttackList = inputArrow + commandCount;
            ++commandCount;

            if (!attackLock)
                StartCoroutine(AttackList());

            if (commandCount > 3)
                commandCount = 1;
        }
    }
    public void JumpAttackX(int inputArrow)
    {
        inputAttackList = inputArrow + 6;
        if (!attackLock)
            StartCoroutine(AttackList());
    }
    public void AttackY(int inputArrow)
    {
        animator.SetBool("isWalk", false);

        inputAttackList = inputArrow + 5;
        if (!attackLock)
            StartCoroutine(AttackList());
    }
    public void AttackYFinal()
    {
        if (animator.GetBool("is_y_Atk"))
        {
            inputAttackList = 0;
            if (!attackLock)
                StartCoroutine(AttackList());
        }
    }

    IEnumerator AttackList()
    {
        attackLock = !attackLock;
        if (rb.velocity.x * rb.velocity.x > 0)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isRun", false);
        }
        do
        {
            switch (inputAttackList)
            {
                case 1:
                case 11:
                    attackPattern = 0;
                    Debug.Log("x");
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 31:
                    Debug.Log("x");
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 41:
                    PlayerControl.instance.actionState = ActionState.IsParrying;
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 2:
                    Debug.Log("xx");
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 12:
                    attackPattern = 1;
                    Debug.Log("xFx");
                    animator.SetBool("is_xFx_Atk", true);
                    break;
                case 32:
                    attackPattern = 2;
                    Debug.Log("xx");
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 3:
                    if (attackPattern == 0)
                    {
                        Debug.Log("xxx");
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        Debug.Log("xFxFx");
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 13:
                    if (attackPattern == 0)
                    {
                        Debug.Log("xxx");
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        Debug.Log("xFxFx");
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 33:
                    if (attackPattern == 1)
                    {
                        Debug.Log("xxx");
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else
                        Debug.Log("xxx");
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
                    animator.SetBool("is_y_Atk", true);
                    break;
                case 6:
                case 16:
                case 36:
                case 46:
                    Debug.Log("jx");
                    animator.SetBool("isJump_x_Atk", true);
                    break;
            }
            inputAttackList = 9;
            yield return null;
        } while (PlayerControl.instance.actionState == ActionState.IsAtk);
        InputInit();
        attackLock = !attackLock;
    }
}
