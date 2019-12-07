using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    MainUI_PlayerStatusView playerStatusView;
    Animator animator;

    public DataBase dataBase;
    public PlayerData playerData;
    public PlayerEquipment playerEquip;

    public float currentHP;     // 현재 체력
    public bool[] HPCut;
    public float currentBuffTime;   // 현재 버프량
    public int buffState;

    public float jumpPower;

    public float moveSpeed;     // 이동 속도
    public int attack;  // 공격력
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
        playerStatusView = GameObject.Find("UI/Hpbar").GetComponent<MainUI_PlayerStatusView>();
    }

    public void SetPlayerData(PlayerData _playerData)
    {
        playerData = _playerData;
        playerEquip.Init();
    }

    public void NewStart(PlayerData playerData)
    {
        SetPlayerData(playerData);
        Init();
    }

    public void Init()
    {
        traningStat = playerData.GetTraningStat();

        currentHP = playerData.GetStatus(7);
        jumpPower = playerData.GetStatus(8);

        attack = (int)(playerData.GetStatus(0) + playerData.GetEquipmentStatus(0) + traningStat[0]);
        defense = (int)(playerData.GetStatus(1) + playerData.GetEquipmentStatus(1) + traningStat[1]);
        moveSpeed = playerData.GetStatus(2) + playerData.GetEquipmentStatus(2) + traningStat[2];
        attackSpeed = playerData.GetStatus(3) + playerData.GetEquipmentStatus(3) + traningStat[3];
        dashDistance = playerData.GetStatus(4) + playerData.GetEquipmentStatus(4) + traningStat[4];
        recovery = playerData.GetStatus(5) + playerData.GetEquipmentStatus(5) + traningStat[5];
        jumpCount = (int)(playerData.GetStatus(6) + playerData.GetEquipmentStatus(6));
    }

    public void HPInit()
    {
        HPCut = new bool[4];
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = true;
        }
        playerStatusView.Init();
    }

    public void DecreaseHP(int MonsterAttack)
    {
        MonsterAttack -= defense;
        if (MonsterAttack < 0)
            MonsterAttack = 0;

        if(currentHP / playerData.GetStatus(7) >= 0.8)
        {
            HPCutCheck(0.8f, 0, MonsterAttack);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.6)
        {
            HPCutCheck(0.6f, 1, MonsterAttack);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.4)
        {
            HPCutCheck(0.4f, 2, MonsterAttack);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.2)
        {
            HPCutCheck(0.2f, 3, MonsterAttack);
        }
        else
            currentHP -= MonsterAttack;

        playerStatusView.Hit(MonsterAttack);

        if (currentHP <= 0) // 죽었을 때 결과창 보여주고 마을로
        {
            Debug.Log("isDead");
        }
        else
        {
            Debug.Log("isHit");
            //animator.SetTrigger("isHit");
        }
    }

    public void HPCutCheck(float HPPercent, int HPCutNum, int MonsterAttack)
    {
        if (HPCut[HPCutNum])
        {
            currentHP -= MonsterAttack;
            if (currentHP / playerData.GetStatus(7) <= HPPercent)
            {
                currentHP = playerData.GetStatus(7) * HPPercent;
                HPCut[HPCutNum] = false;
            }
        }
        else
        {
            currentHP = playerData.GetStatus(7) * (HPPercent - 0.01f);
            playerStatusView.SetHPCut(HPCutNum);
        }
    }

    public void IncreaseHP()
    {
        currentHP += playerData.GetStatus(0);
    }

    public void SetBuff(int buffLevel)
    {
        currentBuffTime += 10 * buffLevel;
        if(currentBuffTime > playerData.GetMaxBuffTime())
        {
            currentBuffTime = playerData.GetMaxBuffTime();
        }
        playerStatusView.SetBuff(1);
    }
}
