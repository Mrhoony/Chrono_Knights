using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GameObject playerStatusView;
    MainUI_PlayerStatusView playerStatus;
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

    float[] traningStat;
    
    // Start is called before the first frame uplayerDataate
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerData = new PlayerData();
        playerData.playerEquipment = playerEquip;
        playerStatus = playerStatusView.GetComponent<MainUI_PlayerStatusView>();
    }

    public void NewStart()
    {
        playerData.Init();
        playerEquip.Init();
        Init();
    }

    public void Init()
    {
        traningStat = playerData.GetTraningStat();

        currentHP = playerData.HP;
        jumpPower = playerData.jumpPower;

        Atk = (int)(playerData.Atk + playerData.up_Atk + traningStat[0]);
        defense = (int)(playerData.defense + playerData.up_defense + traningStat[1]);
        moveSpeed = playerData.moveSpeed + playerData.up_moveSpeed + traningStat[2];
        attackSpeed = playerData.attackSpeed + playerData.up_attackSpeed + traningStat[3];
        dashDistance = playerData.dashDistance + playerData.up_dashDistance + traningStat[4];
        recovery = playerData.recovery + playerData.up_recovery + traningStat[5];

        jumpCount = (int)(playerData.jumpCount + playerData.up_jumpCount);
    }

    public void HPInit()
    {
        HPCut = new int[4];
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = 0;
        }
        playerStatus.Init();
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
                playerStatus.SetHPCut(0);
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
                playerStatus.SetHPCut(1);
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
                playerStatus.SetHPCut(2);
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
                playerStatus.SetHPCut(3);
            }
        }
        else
            currentHP -= Atk;
        
        playerStatus.Hit(Atk);

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
        playerStatus.SetBuff(1);
    }
}
