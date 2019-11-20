using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public PlayerStateView psv;
    Animator animator;

    public PlayerData playerData;
    public PlayerEquipment playerEquip;

    public float currentHP;     // 현재 체력
    public int[] HPCut;
    public float currentBuffTime;   // 현재 버프량
    public int buffState;

    public float jumpPower;

    public float moveSpeed;     // 이동 속도
    public int Atk;  // 공격력
    public float attackSpeed;
    public int jumpCount;
    public int defense;     // 안정성(방어력)
    public float recovery;      // 회복력
    public float dashDistance;  // 대시거리
    
    // Start is called before the first frame uplayerDataate
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerData = new PlayerData();
    }

    public void NewStart()
    {
        playerData.Init();
        Init();
    }

    public void Init()
    {
        currentHP = playerData.HP;
        defense = playerData.defense;
        jumpPower = playerData.jumpPower;

        moveSpeed = playerData.moveSpeed + playerData.up_moveSpeed + playerData.Traning_moveSpeed;
        Atk = playerData.Atk + playerData.up_Atk + playerData.Traning_Atk;
        attackSpeed = playerData.attackSpeed + playerData.up_attackSpeed + playerData.Traning_attackSpeed;
        jumpCount = (int)(playerData.jumpCount + playerData.up_jumpCount + playerData.Traning_jumpCount);
        defense = playerData.defense + playerData.up_defense + playerData.Traning_defense;
        recovery = playerData.recovery + playerData.up_recovery + playerData.Traning_recovery;
    }

    public void HPInit()
    {
        HPCut = new int[4];
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = 0;
        }

        psv.Init();
    }

    public void DecreaseHP(int Atk)
    {
        Atk -= defense;

        if(currentHP / playerData.HP >= 0.8)
        {
            if(HPCut[0] < 1)
            {
                currentHP -= Atk;
                if (currentHP / playerData.HP <= 0.8f)
                {
                    currentHP = playerData.HP * 0.8f;
                    ++HPCut[0];
                }
            }
            else
            {
                currentHP = playerData.HP * 0.79f;
                psv.SetHPCut(0);
            }
        }
        else if (currentHP / playerData.HP >= 0.6)
        {
            if (HPCut[1] < 1)
            {
                currentHP -= Atk;
                if (currentHP / playerData.HP <= 0.6f)
                {
                    currentHP = playerData.HP * 0.6f;
                    ++HPCut[1];
                }
            }
            else
            {
                currentHP = playerData.HP * 0.59f;
                psv.SetHPCut(1);
            }
        }
        else if (currentHP / playerData.HP >= 0.4)
        {
            if (HPCut[2] < 1)
            {
                currentHP -= Atk;
                if (currentHP / playerData.HP <= 0.4f)
                {
                    currentHP = playerData.HP * 0.4f;
                    ++HPCut[2];
                }
            }
            else
            {
                currentHP = playerData.HP * 0.39f;
                psv.SetHPCut(2);
            }
        }
        else if (currentHP / playerData.HP >= 0.2)
        {
            if (HPCut[3] < 1)
            {
                currentHP -= Atk;
                if (currentHP / playerData.HP <= 0.2f)
                {
                    currentHP = playerData.HP * 0.2f;
                    ++HPCut[3];
                }
            }
            else
            {
                currentHP = playerData.HP * 0.19f;
                psv.SetHPCut(3);
            }
        }
        else
            currentHP -= Atk;
        
        psv.Hit(Atk);

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
        currentHP += playerData.Atk;
    }

    public void SetBuff(int buffLevel)
    {
        currentBuffTime += 10 * buffLevel;
        if(currentBuffTime > playerData.maxBuffTime)
        {
            currentBuffTime = playerData.maxBuffTime;
        }
        psv.SetBuff(1);
    }
}
