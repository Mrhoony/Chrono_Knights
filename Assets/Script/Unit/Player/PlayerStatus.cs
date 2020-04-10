using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    attack, defense, moveSpeed, attackSpeed, dashDistance, recovery, jumpCount, HP, jumpPower
}

public class PlayerStatus : MonoBehaviour
{
    public MainUI_PlayerStatusView playerStatusView;
    public PlayerData playerData;
    public PlayerEquipment playerEquip;

    public float currentHP;     // 현재 체력
    private bool[] HPCut;
    private int currentAmmo;   // 현재 버프량
    private int buffState;
    
    private float[] attack = new float[3];        // 공격력
    private float[] defense = new float[3];       // 안정성(방어력)
    private float[] moveSpeed = new float[3];     // 이동 속도
    private float[] attackSpeed = new float[3];   // 공격 속도
    private float[] dashDistance = new float[3];  // 대시거리
    private float[] recovery = new float[3];      // 회복력

    private float jumpCount;
    private float jumpPower;

    float[] traningStat;
    
    public void SetPlayerData(PlayerData _playerData)
    {
        StatusInit();
        playerData = _playerData;
        
        playerEquip = playerData.GetPlayerEquipment();
        playerEquip.EquipmentLimitUpgrade();

        traningStat = playerData.GetTraningStat();
        HPCut = new bool[4];

        PlayerStatusLoad();
        ReturnToTown();
    }
    public void StatusInit()
    {
        for (int i = 0; i < 3; i++)
        {
            attack[i] = 0f;
            defense[i] = 0f;
            moveSpeed[i] = 0f;
            attackSpeed[i] = 0f;
            dashDistance[i] = 0f;
            recovery[i] = 0f;
        }
    }
    public void PlayerStatusLoad()
    {
        attack[0] = playerData.GetStatus(0);
        defense[0] = playerData.GetStatus(1);
        moveSpeed[0] = playerData.GetStatus(2);
        attackSpeed[0] = playerData.GetStatus(3);
        dashDistance[0] = playerData.GetStatus(4);
        recovery[0] = playerData.GetStatus(5);

        jumpCount = playerData.GetStatus(6);
        jumpPower = playerData.GetStatus(8);
        currentAmmo = playerData.GetMaxAmmo();
    }
    public void ReturnToTown()
    {
        PlayerStatusUpdate();
        HPInit();
    }
    public void PlayerStatusUpdate()
    {
        attack[1] = attack[0] + playerData.GetEquipmentStatus(0) + traningStat[0];
        defense[1] = defense[0] + playerData.GetEquipmentStatus(1) + traningStat[1];
        moveSpeed[1] = moveSpeed[0] * 2.2f + playerData.GetEquipmentStatus(2) + traningStat[2];
        attackSpeed[1] = attackSpeed[0] + playerData.GetEquipmentStatus(3) + traningStat[3];
        dashDistance[1] = dashDistance[0] + playerData.GetEquipmentStatus(4) + traningStat[4];
        recovery[1] = recovery[0] + playerData.GetEquipmentStatus(5) + traningStat[5];

        PlayerStatusResultInit();
    }
    public void PlayerStatusResultInit()
    {
        attack[2] = attack[1];
        defense[2] = defense[1];
        moveSpeed[2] = moveSpeed[1];
        attackSpeed[2] = attackSpeed[1];
        dashDistance[2] = dashDistance[1];
        recovery[2] = recovery[1];

        PlayerControl.instance.SetAnimationAttackSpeed(attackSpeed[2]);
    }
    public void HPInit()
    {
        currentHP = playerData.GetStatus(7);
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = true;
        }
        playerStatusView.Init();
    }

    public int DecreaseHP(int _damage)
    {
        _damage -= (int)defense[2];
        if (_damage < 1) _damage = 1;

        if (currentHP / playerData.GetStatus(7) >= 0.8)
        {
            HPCutCheck(0.8f, 0, _damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.6)
        {
            HPCutCheck(0.6f, 1, _damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.4)
        {
            HPCutCheck(0.4f, 2, _damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.2)
        {
            HPCutCheck(0.2f, 3, _damage);
        }
        else
            currentHP -= _damage;

        playerStatusView.Hit(_damage);

        if (currentHP <= 0) // 죽었을 때 결과창 보여주고 마을로
        {
            PlayerControl.instance.Dead();
        }
        return _damage;
    }
    public bool IncreaseHP(int addCurrentHP)
    {
        if (currentHP < playerData.GetStatus(7))
        {
            currentHP += addCurrentHP;
            if (currentHP > playerData.GetStatus(7))
                currentHP = playerData.GetStatus(7);
            return true;
        }
        else return false;
    }
    public void HPCutCheck(float HPPercent, int HPCutNum, int damage)
    {
        if (HPCut[HPCutNum])
        {
            currentHP -= damage;
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
    public void Resupply_Ammo(int _ammoReplenish)
    {
        currentAmmo += 10 * _ammoReplenish;

        if (currentAmmo > playerData.GetMaxAmmo())
        {
            currentAmmo = playerData.GetMaxAmmo();
        }
    }

    #region get, set
    public float GetJumpCount()
    {
        return jumpCount;
    }
    public float GetJumpPower()
    {
        return jumpPower;
    }
    
    public void SetAttackMulty_Result(int multyAttack, bool multy)      // 공격력 배수
    {
        if (multy)
        {
            attack[2] = attack[1] * multyAttack;
        }
        else
        {
            if (0 != multyAttack) attack[2] = attack[1] / multyAttack;
            else attack[2] = attack[1];

            if (attack[2] < 1)
            {
                attack[2] = 1;
            }
        }
    }
    public void SetAttackAdd_Result(int addAttack, bool add)            // 공격력 계산
    {
        if (add)
        {
            attack[2] = attack[1] + addAttack;
        }
        else
        {
            attack[2] = attack[1] - addAttack;
            if (attack[2] < 1)
            {
                attack[2] = 1;
            }
        }
    }
    public void SetDefenceAdd_Result(int addDefense, bool add)          // 방어력 계산
    {
        if (add)
        {
            defense[2] = defense[1] + addDefense;
        }
        else
        {
            defense[2] = defense[1] - addDefense;
        }
    }
    public void SetMoveSpeed_Result(int multyMoveSpeed, bool multy)     // 이동 속도 계산
    {
        if (multy)
        {
            moveSpeed[2] = moveSpeed[1] * multyMoveSpeed;
        }
        else
        {
            moveSpeed[2] = moveSpeed[1] / multyMoveSpeed;
            if (moveSpeed[2] < 1)
            {
                moveSpeed[2] = 1;
            }
        }
    }
    public void SetAttackSpeed_Result(int multyAttackSpeed, bool multy) // 공격 속도 계산
    {
        if (multy)
        {
            attackSpeed[2] = attackSpeed[1] * multyAttackSpeed;
        }
        else
        {
            attackSpeed[2] = attackSpeed[1] / multyAttackSpeed;
            if (attackSpeed[2] < 1)
            {
                attackSpeed[2] = 1;
            }
        }
        PlayerControl.instance.SetAnimationAttackSpeed(attackSpeed[2]);
    }
    public void SetDashDistance_Result(int multyDashDIstance, bool multy)
    {
        if (multy)
        {
            dashDistance[2] = dashDistance[1] * multyDashDIstance;
        }
        else
        {
            dashDistance[2] = dashDistance[1] / multyDashDIstance;
            if (dashDistance[2] < 1)
            {
                dashDistance[2] = 1;
            }
        }
    }
    public void SetRecoveryAdd_Result(int addRecovery, bool add)
    {
        if (add)
            recovery[2] = recovery[1] + addRecovery;
        else
        {
            recovery[2] = recovery[1] - addRecovery;
            if (recovery[2] < 0) recovery[2] = 0;
        }
    }

    public int GetAttack_Result()
    {
        return (int)attack[2];
    }
    public int GetDefence_Result()
    {
        return (int)defense[2];
    }
    public float GetMoveSpeed_Result()
    {
        return moveSpeed[2];
    }
    public float GetAttackSpeed_Result()
    {
        return attackSpeed[2];
    }
    public float GetDashDistance_Result()
    {
        return dashDistance[2];
    }
    public float GetRecovery_Result()
    {
        return recovery[2];
    }
    #endregion
}
