using System.Collections;
using UnityEngine;

public class Weapon_Spear : PlayerWeaponType
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
        if (commandCount <= attackState)
        {
            inputAttackList = 100 + inputArrow + commandCount;
            ++commandCount;

            if (!attackLock)
                StartCoroutine(AttackList());
        }
    }

    public override void AttackY(int inputArrow)
    {
        inputAttackList = inputArrow + 5;
        if (!attackLock)
            StartCoroutine(AttackList());
    }
    public void AttackYFinal()
    {
        if (animator.GetBool("is_y_attack"))
        {
            animator.SetBool("is_y_up_attack", true);
            if (!attackLock)
                StartCoroutine(AttackList());
        }
    }
    public void JumpAttackY()
    {
        inputAttackList = 105;
        if (!attackLock)
            StartCoroutine(AttackList());
    }

    public override void InputInit()
    {
        animator.SetBool("isJump_x_attack", false);
        animator.SetBool("isJump_xx_attack", false);
        animator.SetBool("isJump_xxx_attack", false);
        animator.SetBool("is_x_attack", false);
        animator.SetBool("is_xFx_attack", false);
        animator.SetBool("is_xFxFx_attack", false);
        animator.SetBool("is_xx_attack", false);
        animator.SetBool("is_xxx_attack", false);
        animator.SetBool("is_y_attack", false);
        animator.SetBool("is_y_up_attack", false);
        animator.SetBool("isJump_y_attack", false);

        inputAttackList = 9;
        commandCount = 1;
        attackState = 1;
    }

    IEnumerator AttackList()
    {
        attackLock = !attackLock;
        do
        {
            if (rb.velocity.x * rb.velocity.x > 0 && PlayerControl.instance.actionState != ActionState.IsDodge)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            switch (inputAttackList)
            {
                case 1:
                case 11:
                case 31:
                    attackPattern = 0;
                    animator.SetBool("is_x_attack", true);
                    break;
                case 41:
                    PlayerControl.instance.Parrying();
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
                    animator.SetBool("is_x_upper_attack", true);
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
                    animator.SetBool("is_y_up_attack", true);
                    break;
                case 101:
                case 111:
                    attackPattern = 0;
                    animator.SetBool("isJump_x_attack", true);
                    break;
                case 131:
                    attackPattern = 2;
                    animator.SetBool("isJump_up_x_attack", true);
                    break;
                case 141:
                    attackPattern = 3;
                    animator.SetBool("isJump_down_x_attack", true);
                    break;
                case 102:
                case 112:
                    if (0 != attackPattern) break;
                    animator.SetBool("isJump_xx_attack", true);
                    break;
                case 132:
                    attackPattern = 2;
                    animator.SetBool("isJump_up_x_attack", true);
                    break;
                case 142:
                    attackPattern = 3;
                    animator.SetBool("isJump_down_x_attack", true);
                    break;
                case 103:
                case 113:
                    if (0 != attackPattern) break;
                    animator.SetBool("isJump_xxx_attack", true);
                    break;
                case 133:
                    animator.SetBool("isJump_up_x_attack", true);
                    break;
                case 143:
                    animator.SetBool("isJump_down_x_attack", true);
                    break;
                case 105:
                    animator.SetBool("isJump_y_attack", true);
                    break;
                case 134:
                    animator.SetBool("isJump_up_x_attack", true);
                    break;
                case 144:
                    animator.SetBool("isJump_down_x_attack", true);
                    break;
                default:
                    InputInit();
                    break;
            }
            yield return null;
        } while (PlayerControl.instance.actionState == ActionState.IsAtk || PlayerControl.instance.actionState == ActionState.IsJumpAttack);
        attackLock = !attackLock;
    }
}
