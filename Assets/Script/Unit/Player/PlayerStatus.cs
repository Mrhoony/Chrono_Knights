using UnityEngine;

public enum Status
{
    attack = 0,
    defense,
    moveSpeed,
    attackSpeed,
    dashDistance,
    recovery,
    jumpCount,
    HP,
    jumpPower
}

public class PlayerStatus : MonoBehaviour
{
    public MainUI_PlayerStatusView playerStatusView;
    public PlayerData playerData;

    public float HP;
    public float currentHP;     // 현재 체력
    private bool[] HPCut;
    private int currentAmmo;   // 현재 버프량

    private readonly float[] attack = new float[3];        // 공격력
    private readonly float[] defense = new float[3];       // 안정성(방어력)
    private readonly float[] moveSpeed = new float[3];     // 이동 속도
    private readonly float[] attackSpeed = new float[3];   // 공격 속도
    private readonly float[] dashDistance = new float[3];  // 대시거리
    private readonly float[] recovery = new float[3];      // 회복력
    private int shield;
    
    private readonly int[] debuffAttack = new int[2];
    private readonly int[] debuffDefense = new int[2];

    private readonly float[] dodgeCoolTime = new float[2];
    private readonly float[] invincibleCoolTime = new float[2];
    private readonly float[] dodgeDuringTime = new float[2];

    #region 장비 스킬 관련
    public bool auraAttackOn;
    public bool chargingAttackOn;
    public bool immortalBuffOn;

    public bool miniAttackOn;
    #endregion

    public float jumpCount { get; set; }
    public float jumpPower { get; set; }

    float[] traningStat;
    
    public void SetPlayerData(PlayerData _playerData)
    {
        StatusInit();
        playerData = _playerData;
        playerData.GetPlayerEquipment().EquipmentLimitUpgrade();
        playerData.GetPlayerEquipment().EquipmentSkillCheck();

        traningStat = playerData.GetTraningStat();
        HPCut = new bool[4];

        PlayerStatusLoad();
        ReturnToTown();
    }
    public void StatusInit()
    {
        for (int i = 0; i < 2; i++)
        {
            debuffAttack[i] = 0;
            debuffDefense[i] = 0;

            dodgeCoolTime[i] = 0;
            invincibleCoolTime[i] = 0;
            dodgeDuringTime[i] = 0;
        }
        for (int i = 0; i < 3; i++)
        {
            attack[i] = 0f;
            defense[i] = 0f;
            moveSpeed[i] = 0f;
            attackSpeed[i] = 0f;
            dashDistance[i] = 0f;
            recovery[i] = 0f;
        }
        shield = 0;
    }
    public void PlayerStatusLoad()      // 기본 스테이터스 로드
    {
        attack[0] = playerData.GetStatus(0);
        defense[0] = playerData.GetStatus(1);
        moveSpeed[0] = playerData.GetStatus(2);
        attackSpeed[0] = playerData.GetStatus(3);
        dashDistance[0] = playerData.GetStatus(4);
        recovery[0] = playerData.GetStatus(5);
        dodgeCoolTime[0] = playerData.GetDodgeCoolTime(0);
        invincibleCoolTime[0] = playerData.GetDodgeCoolTime(1);
        dodgeDuringTime[0] = playerData.dodgeDuringTime;

        HP = playerData.GetStatus(7);
        jumpCount = playerData.GetStatus(6);
        jumpPower = playerData.GetStatus(8);
        currentAmmo = playerData.GetMaxAmmo();
    }
    public void ReturnToTown()          // 마을 복귀시 기본 스테이터스, hp 초기화
    {
        auraAttackOn = false;
        chargingAttackOn = false;
        shield = 0;

        Database_Game.instance.skillManager.SkillCoolTimeReset();
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
        
        dodgeCoolTime[1] = dodgeCoolTime[0];
        invincibleCoolTime[1] = invincibleCoolTime[0];
        dodgeDuringTime[1] = dodgeDuringTime[0];

        PlayerStatusResultInit();
    }
    public void PlayerStatusResultInit()
    {
        attack[2] = attack[1] * (1 + playerData.playerEquipment.equipment[(int)EquipmentType.Gloves].itemRarity * 0.5f);
        defense[2] = defense[1];
        moveSpeed[2] = moveSpeed[1];
        attackSpeed[2] = attackSpeed[1];
        dashDistance[2] = dashDistance[1];
        recovery[2] = recovery[1];
        
        PlayerControl.instance.SetAnimationAttackSpeed(attackSpeed[2]);
    }
    public void HPInit()
    {
        currentHP = HP;
        for (int i = 0; i < 4; ++i)
        {
            HPCut[i] = true;
        }
        playerStatusView.Init();
    }

    public int ShieldCheck(int _Damage)
    {
        if(shield > 0)
        {
            if(shield > _Damage)
            {
                shield -= _Damage;
                _Damage = 0;
            }
            else
            {
                _Damage -= shield;
            }
        }
        return _Damage;
    }
    public int DecreaseHP(int _damage)
    {
        _damage -= (int)defense[2];
        if (_damage < 1) _damage = 1;

        if (currentHP / HP >= 0.8)
        {
            if(HPCutCheck(0.8f, 0, _damage))
            {
                playerStatusView.DMGMultiDebuff();
            }
        }
        else if (currentHP / HP >= 0.6)
        {
            if(HPCutCheck(0.6f, 1, _damage))
            {
                SetDebuffAttack(0);
            }
        }
        else if (currentHP / HP >= 0.4)
        {
            if(HPCutCheck(0.4f, 2, _damage))
            {
                playerStatusView.DMGMultiDebuff();
            }
        }
        else if (currentHP / HP >= 0.2)
        {
            if(HPCutCheck(0.2f, 3, _damage))
            {
                Database_Game.instance.skillManager.FirstAidKit();
                SetDebuffAttack(1);
                SetDebuffRecovery();
            }
        }
        else currentHP -= _damage;

        playerStatusView.Hit(_damage);

        if (currentHP <= 0) // 죽었을 때 결과창 보여주고 마을로
        {
            PlayerControl.instance.Dead();
        }
        return _damage;
    }
    public bool IncreaseHP(int addCurrentHP)
    {
        if (currentHP < HP)
        {
            currentHP += addCurrentHP;
            if (currentHP > HP)
                currentHP = HP;
            return true;
        }
        else return false;
    }
    public bool HPCutCheck(float HPPercent, int HPCutNum, int damage)
    {
        currentHP -= damage;

        if (HPCut[HPCutNum])
        {
            if (currentHP / HP <= HPPercent)
            {
                currentHP = HP * HPPercent;
                HPCut[HPCutNum] = false;
                playerStatusView.SetHPCut(HPCutNum);
                return true;
            }
        }
        return false;
    }
    public void IncreaseShield(int _ShieldValue)
    {
        shield += _ShieldValue;
        // 쉴드 애니매이션 온
    }
    public void ShieldReset()
    {
        shield = 0;
        // 쉴드 애니매이션 오프
    }
    public void Resupply_Ammo(int _ammoReplenish)
    {
        currentAmmo += 10 * _ammoReplenish;

        if (currentAmmo > playerData.GetMaxAmmo())
        {
            currentAmmo = playerData.GetMaxAmmo();
        }
    }

    public void SetDebuffAttack(int _num)
    {
        debuffAttack[_num] = (int)(attack[2] * 0.8f);
        attack[2] -= debuffAttack[_num];
        if (attack[2] < 1) attack[2] = 1;
    }
    public void SetRecoveryAttack(int _num)
    {
        attack[2] += debuffAttack[_num];
    }
    public void SetDebuffRecovery()
    {
        recovery[2] *= 0.5f;
        if (recovery[2] < 0) recovery[2] = 0;
        playerStatusView.DMGRecoveryDebuff(recovery[2]);
    }

    #region get, set
    public void SetAttackMulty_Result(int multyAttack, bool multy)      // 공격력 배수
    {
        if (multy)
        {
            attack[2] = attack[2] * multyAttack;
        }
        else
        {
            if (0 != multyAttack)
                attack[2] = attack[2] / multyAttack;

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
            attack[2] = attack[2] + addAttack;
        }
        else
        {
            attack[2] = attack[2] - addAttack;
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
            defense[2] = defense[2] + addDefense;
        }
        else
        {
            defense[2] = defense[2] - addDefense;
        }
    }
    public void SetMoveSpeed_Result(int multyMoveSpeed, bool multy)     // 이동 속도 계산
    {
        if (multy)
        {
            moveSpeed[2] = moveSpeed[2] * multyMoveSpeed;
        }
        else
        {
            moveSpeed[2] = moveSpeed[2] / multyMoveSpeed;
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
            attackSpeed[2] = attackSpeed[2] * multyAttackSpeed;
        }
        else
        {
            attackSpeed[2] = attackSpeed[2] / multyAttackSpeed;
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
            dashDistance[2] = dashDistance[2] * multyDashDIstance;
        }
        else
        {
            dashDistance[2] = dashDistance[2] / multyDashDIstance;
            if (dashDistance[2] < 1)
            {
                dashDistance[2] = 1;
            }
        }
    }
    public void SetRecoveryAdd_Result(int addRecovery, bool add)
    {
        if (add)
            recovery[2] = recovery[2] + addRecovery;
        else
        {
            recovery[2] = recovery[2] - addRecovery;
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

    public float GetDodgeCoolTime()
    {
        return dodgeCoolTime[1] + Database_Game.instance.skillManager.EmergencyEscape();
    }
    public float GetInvincibleDurationTime()
    {
        return invincibleCoolTime[1]; // + Database_Game.instance.skillManager.GetPassiveSkill();
    }
    #endregion
}
