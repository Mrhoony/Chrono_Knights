using System.Collections;
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
public enum SlashEft
{
    DOWNUP, UPDOWN, UPDOWN2, RANDOM
}

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public BoxCollider2D playerCollider;
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

    public GameObject[] spearEffect;

    public Weapon_Spear weaponSpear;
    public Weapon_Gun weaponGun;
    public int weaponType;

    public int inputDirection;
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
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStatus = GetComponent<PlayerStatus>();
        weaponSpear = GetComponent<Weapon_Spear>();
        weaponGun = GetComponent<Weapon_Gun>();

        weaponType = 0;
        weaponSpear.Init(animator, rb);

        chargingAttack = 0f;
        arrowDirection = 1;
        jumpAttack = 0;
        dodgable = true;
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanvasManager.instance.GameMenuOnCheck() || CanvasManager.instance.TownUIOnCheck()) return;       // UI 켜져 있을 때 입력 제한
        if (actionState == ActionState.IsDead) return;
        
        if (rb.velocity.y < -0.2f && actionState != ActionState.IsDodge)
        {
            if (actionState != ActionState.IsJump && isGround && !isJump)
            {
                isJump = true;
                isGround = false;
                actionState = ActionState.IsJump;
                --currentJumpCount;
            }
            animator.SetBool("isFall", true);
            GroundCheck.SetActive(true);
        }   // 낙하 체크

        if (actionState == ActionState.IsDodge || actionState == ActionState.NotMove || actionState == ActionState.IsParrying) return;

        if (Input.GetKey(KeyBindManager.instance.KeyBinds["Left"]))         inputDirection = -1;
        else if (Input.GetKey(KeyBindManager.instance.KeyBinds["Right"]))   inputDirection = 1;
        else                                                                inputDirection = 0;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) inputArrow = 30;
        else if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) inputArrow = 40;
        else if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) inputArrow = 10;
        else if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) inputArrow = 10;

        if (!Input.GetKey(KeyBindManager.instance.KeyBinds["Up"]) &&
            !Input.GetKey(KeyBindManager.instance.KeyBinds["Down"]) &&
            !Input.GetKey(KeyBindManager.instance.KeyBinds["Left"]) &&
            !Input.GetKey(KeyBindManager.instance.KeyBinds["Right"]))
        {
            inputArrow = 0;
        }
        
        // x 공격 입력
        if (weaponType == 0)        // 현재 무기가 창이면
        {
            SpearAttack();
        }
        else if(weaponType == 1)    // 현재 무기가 총이면
        {
            GunAttack();
        }
        
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Dodge"]) && dodgable) Dodge();  // 회피

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
        }  // shift 키 입력시 대쉬

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Jump"])) Jump();     // 점프

        if (actionState != ActionState.Idle) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["ActiveSkill"]))
        {
            Debug.Log("스킬 1 입력");
            skillManager.ActiveSkillUse(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.D))
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
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    chargingAttack += Time.deltaTime;
                }

                if (Input.GetKeyUp(KeyBindManager.instance.KeyBinds["X"]))
                {
                    if (chargingAttack > 1f)
                    {
                        //차지공격 (임시)
                        SpearXAttack();
                    }
                    else
                    {
                        SpearXAttack();
                    }
                    chargingAttack = 0f;
                    return;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    SpearXAttack();
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
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

        if (Input.GetKeyUp(KeyBindManager.instance.KeyBinds["Y"]))
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
    void SpearXAttack()
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
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
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
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"]))
        {
            isLrun = 0;
            ++isRrun;
            runDelay = 0.2f;
        }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"]))
        {
            isRrun = 0;
            ++isLrun;
            runDelay = 0.2f;
        }

        if ((Input.GetKeyUp(KeyBindManager.instance.KeyBinds["Right"]) && isRrun > 1) || (Input.GetKeyUp(KeyBindManager.instance.KeyBinds["Left"]) && isLrun > 1))
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
    #endregion

    void Dodge()
    {
        if (!dodgable) return;
        isGround = false;
        dodgable = false;
        invincible = true;

        StartCoroutine(DodgeIgnore());

        actionState = ActionState.IsDodge;
        animator.SetBool("isLand", false);
        animator.SetTrigger("isDodge");
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
        yield return new WaitForSeconds(0.1f);
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
    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(playerStatus.GetInvincibleDurationTime());
        invincible = false;
    }
    public void Dead()
    {
        DungeonManager.instance.PlayerIsDead();
    }

    public void PlayerJumpAttackEnd()
    {
        actionState = ActionState.IsJump;
        GroundCheck.SetActive(true);
    }
    public void Landing()
    {
        if (isGround) return;

        Debug.Log("land");

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
    public void InstantiateSpearEft(SlashEft se, Transform t)
    {
        Vector3 pos = new Vector3(t.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.3f, 0.3f));
        switch (se)
        {
            case SlashEft.DOWNUP:
                Instantiate(spearEffect[0], pos, gameObject.transform.rotation);
                break;
            case SlashEft.UPDOWN:
                Instantiate(spearEffect[1], pos, gameObject.transform.rotation);
                break;
            case SlashEft.UPDOWN2:
                Instantiate(spearEffect[2], pos, gameObject.transform.rotation);
                break;
            case SlashEft.RANDOM:
                Instantiate(spearEffect[Random.Range(0, 3)], pos, gameObject.transform.rotation);
                break;

            default:
                break;
        }
    }
    
    public void Attack(AtkType _AttackType) // 창의 기본 공격범위, 총의 기본 공격범위~
    {
        Collider2D[] monster;
        playerAttack = Database_Game.instance.GetPlayerAttackInformation(_AttackType);
        float attackDistance = playerStatus.GetDashDistance_Result() * playerAttack.distanceMultiply * 0.5f;

        monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (playerAttack.attackXPoint + attackDistance) * arrowDirection * 0.5f
            , transform.position.y + playerAttack.attackYPoint), new Vector2(attackDistance + playerAttack.attackXPoint, playerAttack.attackYPoint), 0);


        overlap = monster.Length;
        bool _hit = false;
        for (int j = 0; j < overlap; ++j)
        {
            if (monster[j].CompareTag("Monster") || monster[j].CompareTag("BossMonster"))
            {
                switch (_AttackType)
                {
                    case AtkType.spear_X_Upper_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        {
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Jump_Up_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        { 
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Jump_Down_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        { 
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Y_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.RANDOM, monster[j].gameObject.transform);
                            }
                        }
                        CameraManager.instance.CameraShake(1);
                        break;

                    case AtkType.spear_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {                                 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XXX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XFX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN2, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XFXFX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_XX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN2, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_XXX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_Y_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Y_Up_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    default:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            { 
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.RANDOM, monster[j].gameObject.transform);
                            }
                        }
                        break;
                }
                
            }
        }

        // 장비 액티브 발동
        // 장비 패시브 체크 / 발동
        if (playerStatus.auraAttackOn)
        {
            AddAttackAuraSpear(_AttackType, attackDistance);
        }

        if (playerStatus.miniAttackOn)
        {
            AddAttackSupport(_AttackType, attackDistance);
        }
    }
    public void Attack(AtkType _AttackType, float _AttackDistance) // 창의 기본 공격범위, 총의 기본 공격범위~
    {
        Collider2D[] monster;

        playerAttack = Database_Game.instance.GetPlayerAttackInformation(_AttackType);

        monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (playerAttack.attackXPoint + _AttackDistance) * arrowDirection * 0.5f
            , transform.position.y + playerAttack.attackYPoint), new Vector2(_AttackDistance + playerAttack.attackXPoint, playerAttack.attackYPoint), 0);


        overlap = monster.Length;
        bool _hit = false;
        for (int j = 0; j < overlap; ++j)
        {
            if (monster[j].CompareTag("Monster") || monster[j].CompareTag("BossMonster"))
            {
                switch (_AttackType)
                {
                    case AtkType.spear_X_Upper_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        {
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Jump_Up_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        {
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Jump_Down_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                        if (_hit)
                        {
                            monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectUpper(playerAttack.knockBack);
                            InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                        }
                        break;

                    case AtkType.spear_Y_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.RANDOM, monster[j].gameObject.transform);
                            }
                        }
                        CameraManager.instance.CameraShake(1);
                        break;

                    case AtkType.spear_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XXX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.DOWNUP, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XFX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN2, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_XFXFX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_X_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_XX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN2, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_XXX_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Jump_Y_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    case AtkType.spear_Y_Up_Attack:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.UPDOWN, monster[j].gameObject.transform);
                            }
                        }
                        break;

                    default:
                        for (int i = 0; i < playerAttack.attackMultiHit; ++i)
                        {
                            _hit = monster[j].gameObject.GetComponent<Monster_Control>().MonsterHit((int)(playerStatus.GetAttack_Result() * playerAttack.attackMultiply));
                            if (_hit)
                            {
                                monster[j].gameObject.GetComponent<Monster_Control>().MonsterHitRigidbodyEffectKnockBack(playerAttack.knockBack);
                                InstantiateSpearEft(SlashEft.RANDOM, monster[j].gameObject.transform);
                            }
                        }
                        break;
                }
            }
        }

        // 장비 액티브 발동
        // 장비 패시브 체크 / 발동
        if (playerStatus.auraAttackOn)
        {
            AddAttackAuraSpear(_AttackType, _AttackDistance);
        }

        if (playerStatus.miniAttackOn)
        {
            AddAttackSupport(_AttackType, _AttackDistance);
        }
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

    public float AttackDistance(AtkType _AttackType)
    {
        playerAttack = Database_Game.instance.GetPlayerAttackInformation(_AttackType);
        float attackDistance = playerStatus.GetDashDistance_Result() * playerAttack.distanceMultiply * 0.5f;

        RaycastHit2D playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x + playerCollider.size.x * 0.5f * arrowDirection
            , transform.position.y - 0.2f), new Vector2(arrowDirection, 0), attackDistance, rayDashLayerMask);

        if (playerDashBotDistance)
        {
            float botDistance = playerDashBotDistance.point.x - (transform.position.x + playerCollider.size.x * 0.5f * arrowDirection);
            if (attackDistance > Mathf.Abs(botDistance) - playerCollider.size.x * 0.5f)
                attackDistance = Mathf.Abs(botDistance);
        }
        transform.position = new Vector2(transform.position.x + attackDistance * arrowDirection, transform.position.y);

        return attackDistance;
    }
    public void AttackDistanceForce()
    {
        isGround = false;
        GroundCheck.SetActive(false);

        float attackDistance = playerStatus.GetDashDistance_Result() * playerAttack.distanceMultiply * 0.5f;

        --currentJumpCount;

        animator.SetBool("isLand", false);

        rb.velocity = new Vector2(arrowDirection * -1 * (attackDistance * 2f + 5f), attackDistance * 2f + 10f);
    }
    public float AttackDistanceDown()
    {
        RaycastHit2D playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y)
            , new Vector2(arrowDirection, -1), 20f, rayGroundLayerMask);

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
        StartCoroutine(InputIgnore(0.1f));
        InputInit();
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
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
    public bool PlayerIdleCheck()
    {
        if (actionState == ActionState.Idle && inputArrow == 0) return true;
        return false;
    }
    public void PlayerInputKeyFlip()
    {
        ObjectFlip(playerFollowObject);
    }

    public new void SetDamageText(int _Damage)
    {
        GameObject DamageText;
        DamageText = Instantiate(hitTextBox);

        DamageText.transform.position = new Vector3(transform.position.x,
            transform.position.y + playerCollider.size.y,
            transform.position.z);

        DamageText.transform.SetParent(GameObject.Find("FloatingTextPool").transform);
        DamageText.GetComponent<DamageText>().SetDamage(_Damage);
        DamageText.SetActive(true);
    }
}