using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public PlayerStateView psv;
    Animator animator;
    public PlayerData pd;

    public float currentHP;     // 현재 체력
    public int[] HPCut;
    public float moveSpeed;     // 이동 속도
    public float currentBuffTime;   // 현재 버프량
    public int buffState;

    public int Atk;  // 공격력
    public float jumpPower;
    public float defense;   // 방어력
    public float stability;     // 안정성
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        pd = new PlayerData();
    }

    public void NewStart()
    {
        pd.Init();

        Init();
    }

    public void Init()
    {
        moveSpeed = pd.moveSpeed + pd.up_moveSpeed;
        Atk = pd.Atk + pd.up_Atk;
        currentHP = pd.HP + pd.up_HP;
        jumpPower = pd.jumpPower + pd.up_jumpPower;
        defense = pd.defense + pd.up_defense;
        stability = pd.maxStability;
        
        HPCut = new int[4];
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = 0;
        }

        psv.Init();
    }

    IEnumerator StabilityEngauge()
    {
        yield return new WaitForSeconds(10f);

        ++stability;
        if (stability > pd.maxStability)
            stability = pd.maxStability;
    }

    public void DecreaseHP(int Atk)
    {
        if(currentHP / pd.HP >= 0.8)
        {
            if(HPCut[0] < 1)
            {
                currentHP -= Atk;
                if (currentHP / pd.HP <= 0.8f)
                {
                    currentHP = pd.HP * 0.8f;
                    ++HPCut[0];
                }
            }
            else
            {
                currentHP = pd.HP * 0.79f;
                psv.SetHPCut(0);
            }
        }
        else if (currentHP / pd.HP >= 0.6)
        {
            if (HPCut[1] < 1)
            {
                currentHP -= Atk;
                if (currentHP / pd.HP <= 0.6f)
                {
                    currentHP = pd.HP * 0.6f;
                    ++HPCut[1];
                }
            }
            else
            {
                currentHP = pd.HP * 0.59f;
                psv.SetHPCut(1);
            }
        }
        else if (currentHP / pd.HP >= 0.4)
        {
            if (HPCut[2] < 1)
            {
                currentHP -= Atk;
                if (currentHP / pd.HP <= 0.4f)
                {
                    currentHP = pd.HP * 0.4f;
                    ++HPCut[2];
                }
            }
            else
            {
                currentHP = pd.HP * 0.39f;
                psv.SetHPCut(2);
            }
        }
        else if (currentHP / pd.HP >= 0.2)
        {
            if (HPCut[3] < 1)
            {
                currentHP -= Atk;
                if (currentHP / pd.HP <= 0.2f)
                {
                    currentHP = pd.HP * 0.2f;
                    ++HPCut[3];
                }
            }
            else
            {
                currentHP = pd.HP * 0.19f;
                psv.SetHPCut(3);
            }
        }
        else
            currentHP -= Atk;

        stability -= Atk;
        psv.Hit(Atk, stability);

        if (currentHP <= 0) // 죽었을 때 결과창 보여주고 마을로
        {
            Debug.Log("isDead");
            //결과창 띄우기
            DungeonManager.instance.PlayerDie();
        }
        else
        {
            animator.SetTrigger("isHit");
        }
    }

    public void IncreaseHP()
    {
        currentHP += pd.Atk;
    }

    public void SetBuff(int buffLevel)
    {
        currentBuffTime += 10 * buffLevel;
        if(currentBuffTime > pd.maxBuffTime)
        {
            currentBuffTime = pd.maxBuffTime;
        }
        psv.SetBuff(1);

    }
}
