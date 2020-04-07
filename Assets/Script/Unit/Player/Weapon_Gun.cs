using System.Collections;
using UnityEngine;

public class Weapon_Gun : PlayerWeaponType
{
    public override void AttackX(int inputArrow)
    {
        if (commandCount <= attackState)
        {
            inputAttackList = inputArrow + commandCount;
            ++commandCount;

            if (!attackLock)
                StartCoroutine(AttackList());
        }
    }
    public override void JumpAttackX(int inputArrow)
    {
        inputAttackList = inputArrow + 6;
        if (!attackLock)
            StartCoroutine(AttackList());
    }
    public override void AttackY(int inputArrow)
    {
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

    public override void InputInit()
    {
        animator.SetBool("isJump_x_attack", false);
        animator.SetBool("is_x_attack", false);
        animator.SetBool("is_xFx_attack", false);
        animator.SetBool("is_xFxFx_attack", false);
        animator.SetBool("is_xx_attack", false);
        animator.SetBool("is_xxx_attack", false);
        animator.SetBool("is_y_attack", false);
        animator.SetBool("is_y_up_attack", false);

        inputAttackList = 9;
        commandCount = 1;
        attackState = 1;
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
                    animator.SetBool("is_x_attack", true);
                    break;
                case 31:
                    animator.SetBool("is_x_attack", true);
                    break;
                case 41:
                    PlayerControl.instance.actionState = ActionState.IsParrying;
                    animator.SetBool("is_x_attack", true);
                    break;
                case 2:
                    animator.SetBool("is_xx_attack", true);
                    break;
                case 12:
                    attackPattern = 1;
                    animator.SetBool("is_xFx_attack", true);
                    break;
                case 32:
                    attackPattern = 2;
                    animator.SetBool("is_xx_attack", true);
                    break;
                case 3:
                    if (attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_attack", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_attack", true);
                    }
                    break;
                case 13:
                    if (attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_attack", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_attack", true);
                    }
                    break;
                case 33:
                    if (attackPattern == 1)
                    {
                        animator.SetBool("is_xxx_attack", true);
                    }
                    else
                    animator.SetBool("is_xxx_attack", true);
                    break;
                case 5:
                    animator.SetBool("is_y_attack", true);
                    break;
                case 15:
                case 35:
                case 45:
                    animator.SetBool("is_y_attack", true);
                    break;
                case 0:
                    animator.SetBool("is_y_attack", true);
                    break;
                case 6:
                case 16:
                case 36:
                case 46:
                    animator.SetBool("isJump_x_attack", true);
                    break;
                case 9:
                    InputInit();
                    break;
            }
            yield return null;
        } while (PlayerControl.instance.actionState == ActionState.IsAtk);
        attackLock = !attackLock;
    }
}
