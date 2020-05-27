﻿using System.Collections;
using UnityEngine;

public enum GunEft {
    shot1, shot2, downshot
}
public enum AtkType
{
    spear_X_Attack, spear_XX_Attack, spear_XXX_Attack,
    spear_XFX_Attack, spear_XFXFX_Attack, 
    spear_X_Upper_Attack,
    spear_Y_Attack, spear_Y_Up_Attack,
    spear_Up_X_Attack,

    //spear jump
    spear_Jump_X_Attack, spear_Jump_XX_Attack, spear_Jump_XXX_Attack,
    spear_Jump_Y_Attack, spear_Jump_Down_X_Attack, spear_Jump_Up_X_Attack,

    gun_X_Attack, gun_XX_Attack, gun_XXX_Attack,
    gun_XFX_Attack, gun_XFXFX_Attack, gun_JumpX_Attack,
    gun_Y_Attack, gun_YUp_Attack,

    //gun jump
}

public class PlayerControl : MovingObject
{
    public static TestDrawBox TDB = new TestDrawBox();

    public static PlayerControl instance;
    public PlayerAttack playerAttack;
    public LayerMask rayDashLayerMask;
    public LayerMask rayGroundLayerMask;
    public GameObject GroundCheck;
    public PlayerStatus playerStatus;
    public SkillManager skillManager;
    public GameObject playerEffect;
    public GameObject playerInputKey;
    public GameObject playerFollowObject;

    public GameObject[] gunEffect;
    public GameObject[] shotPoint;

    public Weapon_Spear weaponSpear;
    public Weapon_Gun weaponGun;
    public int weaponType;

    public float inputDirection;
    public int inputArrow;

    public float runDelay;
    public int isRrun;
    public int isLrun;

    public float chargingAttack;
    public int jumpAttack;
    public bool isGround;
    public bool isJump;
    
    public bool dodgable;           // 회피 가능 여부
    public bool invincible;         // 무적 시간
    public bool isBlock;

    public bool debugOn;
    public int currentJumpCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStatus = GetComponent<PlayerStatus>();
        weaponSpear = GetComponent<Weapon_Spear>();
        weaponSpear.Init(animator, rb);
        weaponGun = GetComponent<Weapon_Gun>();

        chargingAttack = 0f;
        arrowDirection = 1;
        weaponType = 0;
        jumpAttack = 0;
        dodgable = true;
        isJump = false;

        Debug.Log("control awake");
    }

    // Update is called once per frame
    void Update()
    {
        if (CanvasManager.instance.GameMenuOnCheck() || CanvasManager.instance.TownUIOnCheck()) return;       // UI 켜져 있을 때 입력 제한

        if (actionState == ActionState.IsDead || actionState == ActionState.IsDodge || 
            actionState == ActionState.NotMove || actionState == ActionState.IsParrying) return;

        inputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetKey(KeyCode.UpArrow)) inputArrow = 30;
        else if (Input.GetKey(KeyCode.DownArrow)) inputArrow = 40;
        else if (inputDirection != 0) inputArrow = 10;
        else inputArrow = 0;
        
        // x 공격 입력
        if (weaponType == 0)        // 현재 무기가 창이면
        {
            SpearAttack();
        }
        else if(weaponType == 1)    // 현재 무기가 총이면
        {
            GunAttack();
        }

        // 낙하 체크
        if (rb.velocity.y < -0.5f && actionState != ActionState.IsJumpAttack)
        {
            GroundCheck.SetActive(true);
            if (rb.velocity.y < 1f)
            {
                if (actionState != ActionState.IsJump && !isGround && !isJump)
                {
                    isJump = true;
                    actionState = ActionState.IsJump;
                    --currentJumpCount;
                }
                animator.SetBool("isFall", true);
            }
        }

        if (Input.GetButtonDown("Fire3") && dodgable) Dodge();      // 회피

        if (actionState == ActionState.IsAtk) return;
        if (actionState == ActionState.IsJumpAttack) return;

        RunCheck();        // 대쉬 딜레이
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (arrowDirection > 0)
            {
                isLrun = 0;
                isRrun = 2;
            }
            else if (arrowDirection < 0)
            {
                isRrun = 0;
                isLrun = 2;
            }
        }              // shift 키 입력시 대쉬
        if (Input.GetButtonDown("Jump")) Jump();                    // 점프

        if (actionState != ActionState.Idle) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("스킬 1 입력");
            skillManager.ActiveSkillUse(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(weaponType == 0) // 스피어 -> 건
            {
                weaponType = 1;
                weaponGun.Init(animator, rb);
            }
            else if (weaponType == 1)
            {
                weaponType = 0;
                weaponSpear.Init(animator, rb);
            }
        }       // 장착 무기 변경
    }
    private void FixedUpdate()
    {
        if (actionState == ActionState.IsDead || actionState == ActionState.NotMove) return;
        if (actionState != ActionState.Idle && actionState != ActionState.IsJump) return;     // 피격 시 입력무시

        Move();
        Run();
        // 캐릭터 뒤집기
        if (inputDirection > 0 && arrowDirection != 1)
        {
            Flip();
            PlayerInputKeyFlip();
        }
        else if (inputDirection < 0 && arrowDirection == 1)
        {
            Flip();
            PlayerInputKeyFlip();
        }
    }
    
    void SpearAttack()
    {
        if (!animator.GetBool("is_y_attack"))
        {
            if (playerStatus.chargingAttackOn)
            {
                if (Input.GetButton("Fire1"))
                {
                    chargingAttack += Time.deltaTime;
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    if (chargingAttack > 1f)
                    {
                        XAttack();
                    }
                    else
                    {
                        XAttack();
                    }
                    chargingAttack = 0f;
                    return;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    XAttack();
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (actionState == ActionState.IsJump || actionState == ActionState.IsJumpAttack)
            {
                if (actionState != ActionState.IsJumpAttack)
                {
                    actionState |= ActionState.IsJumpAttack;
                    weaponSpear.JumpAttackY();
                }
            }
            else
            {
                actionState = ActionState.IsAtk;
                weaponSpear.AttackY(inputArrow);
            }
            return;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (actionState != ActionState.IsJump && actionState != ActionState.IsJumpAttack)
            {
                if (actionState == ActionState.IsDodge)
                {
                    weaponSpear.InputInit();
                }
                else
                {
                    weaponSpear.AttackYFinal();
                }
            }
        }
    }

    void XAttack()
    {
        if (actionState == ActionState.IsJumpAttack)
        {
            weaponSpear.JumpAttackX(inputArrow);
        }
        else
        {
            if (actionState == ActionState.IsJump)
            {
                if (jumpAttack < 1) return;
                --jumpAttack;
                actionState |= ActionState.IsJumpAttack;
                weaponSpear.JumpAttackX(inputArrow);
            }
            else
            {
                actionState = ActionState.IsAtk;
                weaponSpear.AttackX(inputArrow);
            }
        }
    }

    void GunAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            if (actionState == ActionState.IsJump)
            {
                if (jumpAttack < 1) return;
                --jumpAttack;
                actionState = ActionState.IsJumpAttack;
                weaponGun.JumpAttackX(inputArrow);
            }
            else
            {
                actionState = ActionState.IsAtk;
                weaponGun.AttackX(inputArrow);
            }
        }
    }

    #region 이동 관련 함수
    void Jump()
    {
        if (actionState == ActionState.IsDodge) return;
        if (actionState == ActionState.IsJumpAttack) return;
        if (currentJumpCount < 1) return;

        isGround = false;
        isJump = true;
        GroundCheck.SetActive(false);

        jumpAttack = 1;
        --currentJumpCount;

        animator.SetBool("isLand", false);
        animator.SetBool("isFall", false);
        animator.SetTrigger("isJumpTrigger");
        animator.SetBool("isJump", true);

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0f, playerStatus.jumpPower), ForceMode2D.Impulse);
        actionState = ActionState.IsJump;
    }
    void Move()
    {
        if (inputDirection != 0)
        {
            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result(), rb.velocity.y);
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    void Run()
    {
        if (inputDirection > 0 && isRrun > 1)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result() * 2f, rb.velocity.y);
        }

        if (inputDirection < 0 && isLrun > 1)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result() * 2f, rb.velocity.y);
        }
    }
    void RunCheck()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isLrun = 0;
            ++isRrun;
            runDelay = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isRrun = 0;
            ++isLrun;
            runDelay = 0.2f;
        }

        if (((Input.GetKeyUp(KeyCode.RightArrow) && isRrun > 1) || (Input.GetKeyUp(KeyCode.LeftArrow) && isLrun > 1)))
        {
            isRrun = 0;
            isLrun = 0;
            animator.SetBool("isRun", false);
        }

        if ((isLrun > 0 || isRrun > 0) && inputDirection == 0)
        {
            animator.SetBool("isRun", false);
        }

        if (runDelay > 0)
        {
            runDelay -= Time.deltaTime;
            if (runDelay <= 0 && (isRrun < 2 && isLrun < 2))
            {
                isRrun = 0;
                isLrun = 0;
            }
        }
    }
    public void Blink()
    {

    }
    #endregion

    void Dodge()
    {
        if (!dodgable) return;
        dodgable = false;
        invincible = true;

        StartCoroutine(DodgeIgnore());

        animator.SetBool("isLand", false);
        animator.SetTrigger("isDodge");
        actionState = ActionState.IsDodge;
        InputInit();

        if (weaponType == 0)
        {
            if (inputDirection == arrowDirection)
            {
                rb.velocity = new Vector2(arrowDirection * playerStatus.GetDashDistance_Result() * 2f, 4f);
            }
            else
            {
                rb.velocity = new Vector2(-arrowDirection * playerStatus.GetDashDistance_Result() * 2f, 4f);
            }
        }
        else
        {
            rb.velocity = new Vector2(-arrowDirection * playerStatus.GetDashDistance_Result() * 2f, 1f);
        }
        StartCoroutine(DodgeCount());
        StartCoroutine(InvincibleCount());
        Debug.Log("dodge");
    }
    IEnumerator DodgeIgnore()
    {
        GroundCheck.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        GroundCheck.SetActive(true);
    }
    IEnumerator DodgeCount()
    {
        yield return new WaitForSeconds(playerStatus.GetDodgeCoolTime());
        dodgable = true;
    }

    public void Hit(int _damage)
    {
        if (invincible)
        {
            if (actionState == ActionState.IsDodge)
            {
                playerEffect.GetComponent<Animator>().SetTrigger("isDodge_Trigger");
            }
            return;
        }// 무적시간일 때
        else if (actionState == ActionState.IsParrying)
        {
            playerEffect.GetComponent<Animator>().SetTrigger("isDodge_Trigger");
            animator.SetBool("is_x_Atk", true);
            return;
        }//패링중일때

        // 쉴드가 있을경우 
        _damage = playerStatus.ShieldCheck(_damage);
        if (_damage <= 0) return;

        SetDamageText(_damage);

        StopPlayer();
        rb.gravityScale = 1f;
        CameraManager.instance.CameraShake(playerStatus.DecreaseHP(_damage) / 2);
        animator.SetTrigger("isHit");
        playerEffect.GetComponent<Animator>().SetTrigger("isHit_Trigger");

        rb.velocity = new Vector2(-arrowDirection * 3f, 2.5f);

        invincible = true;
        StartCoroutine(InvincibleCount());
    }
    public void Dead()
    {
        DungeonManager.instance.PlayerIsDead();
    }
    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(playerStatus.GetInvincibleDurationTime());
        invincible = false;
    }

    public void PlayerJumpAttackEnd()
    {
        actionState = ActionState.IsJump;
    }
    public void Landing()
    {
        currentJumpCount = (int)playerStatus.jumpCount;
        isGround = true;
        isJump = false;
        animator.SetBool("isJump", false);
        animator.SetBool("isLand", true);
        InputInit();
        actionState = ActionState.Idle;
    }

    #region parrying
    public void Parrying()
    {
        actionState = ActionState.IsParrying;
        StartCoroutine(ParryingCount());
    }
    public void ParryingCheck()
    {
        // 애니매이션에서 체크
        StopCoroutine("ParryingCount");
        animator.SetBool("is_x_attack", true);
        Debug.Log("parrying");
    }
    IEnumerator ParryingCount()
    {
        yield return new WaitForSeconds(1f);
        actionState = ActionState.NotMove;
        yield return new WaitForSeconds(1f);
        actionState = ActionState.Idle;
    }
    #endregion

    public void SetCurrentJumpCount()
    {
        currentJumpCount = (int)playerStatus.jumpCount;
    }
    public void InputInit()
    {
        if(weaponType == 0)
        {
            weaponSpear.InputInit();
        }
        else
        {
            weaponGun.InputInit();
        }
    }

    public void SetAttackState(int _attackState)
    {
        if (weaponType == 0)
            weaponSpear.SetAttackState(_attackState);
        else
            weaponGun.SetAttackState(_attackState);
    }
    public void SetAnimationAttackSpeed(float _attackSpeed)
    {
        animator.SetFloat("AttackSpeed", _attackSpeed);
    }

    public IEnumerator MonsterHitEffect()
    {
        for (int i = 0; i < 3; ++i)
        {
            spriteRenderer.material = whiteFlashMaterial;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = defaultMaterial;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void InstantiateGunEft(GunEft ge) {
        switch (ge) {
            case GunEft.shot1:
                Instantiate(gunEffect[0], shotPoint[0].transform);
                break;
            case GunEft.shot2:
                Instantiate(gunEffect[1], shotPoint[1].transform);
                break;
            case GunEft.downshot:
                Instantiate(gunEffect[2], shotPoint[2].transform);
                break;
            default:
                break;
        }
    }
    
    public float Attack(AtkType _atkType) //창의 기본 공격범위, 총의 기본 공격범위~
    {
        Collider2D[] monster;
        float attackDistance = 0f;

        playerAttack = Database_Game.instance.GetPlayerAttackInformation(_atkType);

        attackDistance = playerStatus.GetDashDistance_Result() * playerAttack.distanceMultiply * 0.5f;
        monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (playerAttack.attackXPoint + attackDistance) * arrowDirection * 0.5f
            , transform.position.y + playerAttack.attackYPoint), new Vector2(attackDistance + playerAttack.attackXPoint, playerAttack.attackYPoint), 0);


        overlap = monster.Length;
        bool _hit = false;
        for (int j = 0; j < overlap; ++j)
        {
            if (monster[j].CompareTag("Monster") || monster[j].CompareTag("BossMonster"))
            {
                switch (_atkType)
                {
                    case AtkType.spear_X_Upper_Attack:
                    case AtkType.spear_Jump_Up_X_Attack:
                    case AtkType.spear_Jump_Down_X_Attack:
                        for(int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                        break;

                    case AtkType.spear_Y_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                        }
                        CameraManager.instance.CameraShake(1);
                        break;

                    default:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                        }
                        break;
                }
            }
        }

        // 장비 액티브 발동
        // 장비 패시브 체크 / 발동
        if (playerStatus.auraAttackOn)
        {
            AddAttackAuraSpear(_atkType, attackDistance);
        }

        if (playerStatus.miniAttackOn)
        {
            AddAttackSupport(_atkType, attackDistance);
        }

        return attackDistance;
    }
    public void AddAttackAuraSpear(AtkType _AttackType, float _AttackDistance)
    {
        int overlap2;
        Collider2D[] monster_Aura;
        Skill _auraSkill = Database_Game.instance.GetSkill(101);

        monster_Aura = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x + (playerAttack.attackXPoint + _AttackDistance) * arrowDirection * 0.5f,
            transform.position.y + playerAttack.attackYPoint), 
            new Vector2(_AttackDistance + playerAttack.attackXPoint + _auraSkill.skillValue, 
            playerAttack.attackYPoint + _auraSkill.skillValue), 
            0);

        overlap2 = monster_Aura.Length;
        for (int j = 0; j < overlap2; ++j)
        {
            if (monster_Aura[j].CompareTag("Monster") || monster_Aura[j].CompareTag("BossMonster"))
            {
                monster_Aura[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * _auraSkill.skillMultiply));
            }
        }
        Debug.Log("aura attack " + (int)(playerStatus.GetAttack_Result() * _auraSkill.skillMultiply));
    }
    public void AddAttackSupport(AtkType _AttackType, float _AttackDistance)
    {
        int overlap2;
        Collider2D[] monster_Aura;
        Skill _auraSkill = Database_Game.instance.GetSkill(201);

        monster_Aura = Physics2D.OverlapBoxAll(
            new Vector2(transform.position.x + (playerAttack.attackXPoint + _AttackDistance) * arrowDirection * 0.5f,
            transform.position.y + playerAttack.attackYPoint),
            new Vector2(_AttackDistance + playerAttack.attackXPoint + _auraSkill.skillValue,
            playerAttack.attackYPoint + _auraSkill.skillValue),
            0);

        overlap2 = monster_Aura.Length;
        for (int j = 0; j < overlap2; ++j)
        {
            if (monster_Aura[j].CompareTag("Monster") || monster_Aura[j].CompareTag("BossMonster"))
            {
                monster_Aura[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * _auraSkill.skillMultiply));
            }
        }
        Debug.Log("add attack " + (int)(playerStatus.GetAttack_Result() * _auraSkill.skillMultiply));
    }

    public void AttackDistance(float _distanceMulty)
    {
        RaycastHit2D playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x + GetComponent<BoxCollider2D>().size.x * 0.5f * arrowDirection
            , transform.position.y - 0.2f), new Vector2(arrowDirection, 0), _distanceMulty, rayDashLayerMask);

        if (playerDashBotDistance)
        {
            float botDistance = playerDashBotDistance.point.x - (transform.position.x + GetComponent<BoxCollider2D>().size.x * 0.5f * arrowDirection);
            if (_distanceMulty > Mathf.Abs(botDistance) - GetComponent<BoxCollider2D>().size.x * 0.5f)
                _distanceMulty = Mathf.Abs(botDistance) - GetComponent<BoxCollider2D>().size.x * 0.5f;
        }
        transform.position = new Vector2(transform.position.x + _distanceMulty * arrowDirection, transform.position.y);
    }
    public void AttackDistanceForce(float _distanceMulty)
    {
        isGround = false;
        GroundCheck.SetActive(false);

        --currentJumpCount;

        animator.SetBool("isLand", false);

        rb.velocity = new Vector2(arrowDirection * -1 * (_distanceMulty * 2f + 5f), _distanceMulty * 2f + 10f);
    }
    public float AttackDistanceDown(float _distanceMulty)
    {
        RaycastHit2D playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y)
            , new Vector2(arrowDirection, -1), 20f, rayDashLayerMask);

        if (playerDashBotDistance)
        {
            transform.position = new Vector2(playerDashBotDistance.point.x - (arrowDirection * 0.5f), playerDashBotDistance.point.y + 0.25f);
            return playerDashBotDistance.distance * 0.87f;
        }
        else
        {
            return 20f;
        }
    }

    public void StopPlayer()
    {
        actionState = ActionState.NotMove;
        rb.velocity = Vector2.zero;
        StartCoroutine(InputIgnore(0.5f));
        InputInit();
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetTrigger("PlayerStop");
    }
    IEnumerator InputIgnore(float _time)
    {
        yield return new WaitForSeconds(_time);
        actionState = ActionState.Idle;
    }
    public void PlayerStateInit()
    {
        actionState = ActionState.Idle;
    }

    public void PlayerInputKeyFlip()
    {
        ObjectFlip(playerFollowObject);
    }
    public void OnDrawGizmosSelected()
    {
        if (actionState == ActionState.IsAtk)
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawCube(
                new Vector3(transform.position.x + (3f + 0.2f) * arrowDirection * 0.5f, transform.position.y + 0.2f, 0) // 중심지점
                , new Vector3(3f + 0.4f, 0.4f, 0)   // 전체 범위
                );
        }
    }
}

public class TestDrawBox
{
    public Color color = Color.green;
    public Vector2 v2TopLeft;
    public Vector2 v2BottomRight;

    public void DrawBox(GameObject target, Vector2 dis, Vector2 size)
    {
        v2TopLeft = (Vector2)target.transform.position + dis + new Vector2(size.x * -0.5f, size.y * 0.5f);
        v2BottomRight = (Vector2)target.transform.position + dis + new Vector2(size.x * 0.5f, size.y * -0.5f);
        Debug.DrawLine(v2TopLeft, v2BottomRight, color);
    }
    public void DrawBox(Vector2 target, Vector2 size)
    {
        v2TopLeft = target + new Vector2(size.x * -0.5f, size.y * 0.5f);
        v2BottomRight = target + new Vector2(size.x * 0.5f, size.y * -0.5f);
        Debug.DrawLine(v2TopLeft, v2BottomRight, color);
    }
}