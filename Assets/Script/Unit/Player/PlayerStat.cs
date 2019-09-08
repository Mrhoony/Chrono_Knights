using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float moveSpeed;     // 이동 속도

    public float HP;     // 최대 체력
    public float currentHP;     // 현재 체력
    public int[] HPCut;
    public float MaxBuffTime;   // 현재 버프량
    public float currentBuffTime;   // 현재 버프량
    public int buffState;

    public int Atk;  // 공격력
    public float jumpPower;
    public float defense;   // 방어력
    public float stability;     // 안정성
    public float maxStability = 100;

    PlayerStateView psv;

    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        psv = GameObject.Find("ui_hpbar_base").GetComponent<PlayerStateView>();
        Init();
    }

    public void Init()
    {
        currentHP = HP;
        HPCut = new int[4];
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = 0;
        }
        MaxBuffTime = 100;
        buffState = 0;
        defense = 0;
        stability = 100;

        psv.Init();
    }

    IEnumerator StabilityEngauge()
    {
        yield return new WaitForSeconds(10f);

        ++stability;
        if (stability > maxStability)
            stability = maxStability;
    }

    public void DecreaseHP(int Atk)
    {
        if(currentHP / HP >= 0.8)
        {
            if(HPCut[0] < 1)
            {
                currentHP -= Atk;
                if (currentHP / HP <= 0.8f)
                {
                    currentHP = HP * 0.8f;
                    ++HPCut[0];
                }
            }
            else
            {
                currentHP = HP * 0.79f;
                psv.SetHPCut(0);
            }
        }
        else if (currentHP / HP >= 0.6)
        {
            if (HPCut[1] < 1)
            {
                currentHP -= Atk;
                if (currentHP / HP <= 0.6f)
                {
                    currentHP = HP * 0.6f;
                    ++HPCut[1];
                }
            }
            else
            {
                currentHP = HP * 0.59f;
                psv.SetHPCut(1);
            }
        }
        else if (currentHP / HP >= 0.4)
        {
            if (HPCut[2] < 1)
            {
                currentHP -= Atk;
                if (currentHP / HP <= 0.4f)
                {
                    currentHP = HP * 0.4f;
                    ++HPCut[2];
                }
            }
            else
            {
                currentHP = HP * 0.39f;
                psv.SetHPCut(2);
            }
        }
        else if (currentHP / HP >= 0.2)
        {
            if (HPCut[3] < 1)
            {
                currentHP -= Atk;
                if (currentHP / HP <= 0.2f)
                {
                    currentHP = HP * 0.2f;
                    ++HPCut[3];
                }
            }
            else
            {
                currentHP = HP * 0.19f;
                psv.SetHPCut(3);
            }
        }
        else
            currentHP -= Atk;

        psv.Hit(Atk);
        stability -= Atk;

        if (currentHP <= 0)
        {
            Debug.Log("isDead");
        }
        else
        {
            animator.SetTrigger("isHit");
        }
    }

    public void IncreaseHP()
    {
        currentHP += Atk;
    }

    public void SetBuff(int buffLevel)
    {
        currentBuffTime += 10 * buffLevel;
        if(currentBuffTime > MaxBuffTime)
        {
            currentBuffTime = MaxBuffTime;
        }
        psv.SetBuff(1);

    }
}
