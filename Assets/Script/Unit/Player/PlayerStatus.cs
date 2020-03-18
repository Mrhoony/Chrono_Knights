using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
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
    
    private float attack;        // 공격력
    private float defense;       // 안정성(방어력)
    private float moveSpeed;     // 이동 속도
    private float attackSpeed;   // 공격 속도
    private float dashDistance;  // 대시거리
    private float recovery;      // 회복력

    private float jumpCount;
    private float jumpPower;

    public int attack_Result;
    public int defense_Result;     // 안정성(방어력)
    public float moveSpeed_Result;
    public float attackSpeed_Result;
    public float dashDistance_Result;
    public float recovery_Result;      // 회복력

    float[] traningStat;
    
    public void SetPlayerData(PlayerData _playerData)
    {
        Debug.Log("player status Init");

        playerData = _playerData;
        playerEquip = playerData.GetPlayerEquipment();
        traningStat = playerData.GetTraningStat();
        HPCut = new bool[4];

        ReturnToTown();
    }

    public void ReturnToTown()
    {
        HPInit();
        PlayerStatusUpdate(playerEquip);
    }

    public void PlayerStatusInit()
    {
        PlayerStatusUpdate(playerEquip);
    }

    public void PlayerStatusUpdate(PlayerEquipment _playerEquipment)
    {
        Debug.Log("Player status update");
        playerData.renew(_playerEquipment);

        attack = playerData.GetStatus(0) + traningStat[0] + playerData.GetEquipmentStatus(0);
        defense = playerData.GetStatus(1) + playerData.GetEquipmentStatus(1) + traningStat[1];
        moveSpeed = playerData.GetStatus(2) + (playerData.GetEquipmentStatus(2) * 0.1f) + traningStat[2];
        attackSpeed = playerData.GetStatus(3) + (playerData.GetEquipmentStatus(3) * 0.1f) + traningStat[3];
        dashDistance = playerData.GetStatus(4) + playerData.GetEquipmentStatus(4) + traningStat[4];
        recovery = playerData.GetStatus(5) + playerData.GetEquipmentStatus(5) + traningStat[5];
        jumpCount = playerData.GetStatus(6) + playerData.GetEquipmentStatus(6);

        jumpPower = playerData.GetStatus(8);
        currentAmmo = playerData.GetMaxAmmo();

        SetAttackAdd_Result(0, true);
        SetAttackMulty_Result(1, true);
        SetDefenceAdd_Result(0, true);
        SetMoveSpeed_Result(1, true);
        SetAttackSpeed_Result(1, true);
        SetDashDistance_Result(1, true);
        SetRecoveryAdd_Result(0, true);

        PlayerControl.instance.SetAnimationAttackSpeed(attackSpeed_Result);
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

    public int DecreaseHP(int damage)
    {
        damage -= defense_Result;
        if (damage < 0)
            damage = 0;

        if (currentHP / playerData.GetStatus(7) >= 0.8)
        {
            HPCutCheck(0.8f, 0, damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.6)
        {
            HPCutCheck(0.6f, 1, damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.4)
        {
            HPCutCheck(0.4f, 2, damage);
        }
        else if (currentHP / playerData.GetStatus(7) >= 0.2)
        {
            HPCutCheck(0.2f, 3, damage);
        }
        else
            currentHP -= damage;

        playerStatusView.Hit(damage);

        if (currentHP <= 0) // 죽었을 때 결과창 보여주고 마을로
        {
            Debug.Log("isDead");
        }
        else
        {
            Debug.Log("isHit");
            //animator.SetTrigger("isHit");
        }
        return damage;
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
    public void replenish_Ammo(int _ammoReplenish)
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
    
    public void SetAttackMulty_Result(int multyAttack, bool multy)
    {
        if (multy)
        {
            attack_Result = (int)attack * multyAttack;
        }
        else
        {
            attack_Result = (int)attack / multyAttack;
            if(attack_Result < 1)
            {
                attack_Result = 1;
            }
        }
    }
    public void SetAttackAdd_Result(int addAttack, bool add)
    {
        if (add)
        {
            attack_Result = (int)attack + addAttack;
        }
        else
        {
            attack_Result = (int)attack - addAttack;
            if (attack_Result < 1)
            {
                attack_Result = 1;
            }
        }
    }
    public void SetDefenceAdd_Result(int addDefense, bool add)
    {
        if (add)
        {
            defense_Result = (int)defense + addDefense;
        }
        else
        {
            defense_Result = (int)defense - addDefense;
        }
    }
    public void SetMoveSpeed_Result(int multyMoveSpeed, bool multy)
    {
        if (multy)
        {
            moveSpeed_Result = moveSpeed * multyMoveSpeed;
        }
        else
        {
            moveSpeed_Result = moveSpeed / multyMoveSpeed;
            if (moveSpeed_Result < 1)
            {
                moveSpeed_Result = 1;
            }
        }
    }
    public void SetAttackSpeed_Result(int multyAttackSpeed, bool multy)
    {
        if (multy)
        {
            attackSpeed_Result = attackSpeed * multyAttackSpeed;
        }
        else
        {
            attackSpeed_Result = attackSpeed / multyAttackSpeed;
            if (attackSpeed_Result < 1)
            {
                attackSpeed_Result = 1;
            }
        }
    }
    public void SetDashDistance_Result(int multyDashDIstance, bool multy)
    {
        if (multy)
        {
            dashDistance_Result = dashDistance * multyDashDIstance;
        }
        else
        {
            dashDistance_Result = dashDistance / multyDashDIstance;
            if (dashDistance_Result < 1)
            {
                dashDistance_Result = 1;
            }
        }
    }
    public void SetRecoveryAdd_Result(int addRecovery, bool add)
    {
        if (add)
            recovery_Result = recovery + addRecovery;
        else
        {
            recovery_Result = recovery - addRecovery;
            if (recovery_Result < 0) recovery_Result = 0;
        }
    }

    public int GetAttack_Result()
    {
        return attack_Result;
    }
    public int GetDefence_Result()
    {
        return defense_Result;
    }
    public float GetMoveSpeed_Result()
    {
        return moveSpeed_Result;
    }
    public float GetAttackSpeed_Result()
    {
        return attackSpeed_Result;
    }
    public float GetDashDistance_Result()
    {
        return dashDistance_Result;
    }
    public float GetRecovery_Result()
    {
        return recovery_Result;
    }
    #endregion
}
