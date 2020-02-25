using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public BoxCollider2D playerCharacterCollider;
    public LayerMask rayDashLayerMask;
    public GameObject GroundCheck;
    public PlayerStatus playerStatus;
    public SkillManager skillManager;
    
    public Collider2D[] monster;

    public Weapon_Spear weaponSpear;
    public Weapon_Gun weaponGun;
    public RaycastHit2D playerDashTopDistance;
    public RaycastHit2D playerDashBotDistance;

    public int weaponType;

    public float inputDirection;

    public int inputArrow;
    public bool inputAttackX;
    public bool inputAttackY;
    public bool finalAttackY;
    public bool inputJump;
    public bool inputDodge;

    public float runDelay;
    public int isRrun;
    public int isLrun;
    
    public bool isGround;
    public bool isJumpAttack;
    
    public bool dodgable;
    public bool invincible;
    public float invincibleCount;

    public int currentJumpCount;

    public bool isBlock;

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

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        playerCharacterCollider = gameObject.GetComponent<BoxCollider2D>();
        playerStatus = GetComponent<PlayerStatus>();
        weaponSpear = GetComponent<Weapon_Spear>();
        weaponSpear.Init(animator, rb);
        weaponGun = GetComponent<Weapon_Gun>();
        skillManager.Init();
        
        arrowDirection = 1;
        weaponType = 0;
        dodgable = true;
        isJumpAttack = false;

        Debug.Log("control awake");
    }

    // Update is called once per frame
    void Update()
    {
        // x 공격 입력
        if (weaponType == 0)
        {
            if (Input.GetButtonDown("Fire1") && !animator.GetBool("is_y_Atk")) inputAttackX = true;
            if (Input.GetButtonUp("Fire2"))
            {
                inputAttackY = false;
                finalAttackY = true;
            }
        }
        else if(weaponType == 1)
        {
            if (Input.GetButtonDown("Fire1")) inputAttackX = true;
        }

        if (actionState == ActionState.NotMove) return;             // notMove 가 아닐 때

        inputDirection = Input.GetAxisRaw("Horizontal");

        // shift 키 입력시 대쉬
        if (Input.GetKey(KeyCode.LeftShift))
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
        }

        // 낙하 체크
        if (rb.velocity.y <= -0.5f)
        {
            if (actionState == ActionState.IsAtk) return;
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            if (actionState != ActionState.IsJump && !isGround)
            {
                actionState = ActionState.IsJump;
                --currentJumpCount;
            }
            GroundCheck.SetActive(true);
        }

        // 대쉬 딜레이
        RunCheck();
        
        if (actionState == ActionState.IsParrying) StartCoroutine(ParryingCount());

        // 커맨드용 입력
        if (Input.GetKey(KeyCode.UpArrow))        inputArrow = 30;
        else if (Input.GetKey(KeyCode.DownArrow)) inputArrow = 40;
        else if (inputDirection != 0) inputArrow = 10;
        else inputArrow = 0;

        // 회피
        if (Input.GetButtonDown("Fire3") && dodgable) inputDodge = true;

        if (actionState == ActionState.IsAtk) return;

        InputSkillButton();
        
        // y 공격 입력
        if (weaponType == 0)
        {
            if (Input.GetButton("Fire2")) inputAttackY = true;
        }
        else if (weaponType == 1)
        {

        }

        if (Input.GetButtonDown("Jump")) inputJump = true;

        // 장착 무기 변경
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(weaponType == 0) // 스피어 -> 건
            {
                weaponType = 1;
                weaponSpear.enabled = false;    // 스피어 오프
                weaponGun.enabled = true;       // 건 온
                weaponGun.Init(animator, rb);
            }
            else if (weaponType == 1)
            {
                weaponType = 0;
                weaponGun.enabled = false;
                weaponSpear.enabled = true;
                weaponSpear.Init(animator, rb);
            }
        }
    }

    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(0.5f);
        invincible = false;
    }
    IEnumerator ParryingCount()
    {
        yield return new WaitForSeconds(0.2f);
        actionState = ActionState.Idle;
    }
    IEnumerator DodgeCount()
    {
        playerCharacterCollider.enabled = false;
        yield return new WaitForSeconds(0.3f);
        playerCharacterCollider.enabled = true;
        dodgable = true;
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

        if((isLrun > 0 || isRrun > 0) && inputDirection == 0)
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

    public void InputSkillButton()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (playerStatus.playerEquip.equipment[0].skillCode == 0 || playerStatus.playerEquip.equipment[0].isUsed) return;
            Debug.Log("스킬 1 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (playerStatus.playerEquip.equipment[1].skillCode == 0 || playerStatus.playerEquip.equipment[1].isUsed) return;
            Debug.Log("스킬 2 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (playerStatus.playerEquip.equipment[2].skillCode == 0 || playerStatus.playerEquip.equipment[2].isUsed) return;
            Debug.Log("스킬 3 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (playerStatus.playerEquip.equipment[3].skillCode == 0 || playerStatus.playerEquip.equipment[3].isUsed) return;
            Debug.Log("스킬 4 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[3]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (playerStatus.playerEquip.equipment[4].skillCode == 0 || playerStatus.playerEquip.equipment[4].isUsed) return;
            Debug.Log("스킬 5 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[4]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (playerStatus.playerEquip.equipment[5].skillCode == 0 || playerStatus.playerEquip.equipment[5].isUsed) return;
            Debug.Log("스킬 6 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[5]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (playerStatus.playerEquip.equipment[6].skillCode == 0 || playerStatus.playerEquip.equipment[6].isUsed) return;
            Debug.Log("스킬 7 입력");
            skillManager.SkillCheck(playerStatus.playerEquip.equipment[6]);
        }
        else
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(new Vector2(transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection, transform.position.y), new Vector2(arrowDirection, 0), Color.red, 3f * 0.1f);

        if (actionState == ActionState.NotMove) return;     // 피격 시 입력무시

        if (weaponType == 0) SpearAttack();
        if (weaponType == 1) GunAttack();

        Jump();
        Dodge();

        if (actionState == ActionState.IsAtk) return;       // 공격 중 입력무시
        if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시

        // 캐릭터 뒤집기
        if (inputDirection > 0 && isFaceRight)
        {
            Flip();
        }
        else if (inputDirection < 0 && !isFaceRight)
        {
            Flip();
        }

        Move();
        Run();
    }

    void SpearAttack()
    {
        if (inputAttackX)
        {
            inputAttackX = false;
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시

            if (actionState == ActionState.IsJump)
            {
                if (isJumpAttack) return;
                isJumpAttack = true;
                Debug.Log("input jump x");
                actionState = ActionState.IsJumpAttack;
                weaponSpear.JumpAttackX(inputArrow);
            }
            else
            {
                Debug.Log("input x");
                actionState = ActionState.IsAtk;
                weaponSpear.AttackX(inputArrow);
            }
        }
        else if (inputAttackY)
        {
            if (actionState == ActionState.IsJump) return;
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            actionState = ActionState.IsAtk;
            weaponSpear.AttackY(inputArrow);
        }
        else if (finalAttackY)
        {
            if (actionState == ActionState.IsJump) return;
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            finalAttackY = false;
            weaponSpear.AttackYFinal();
        }
    }

    void GunAttack()
    {
        if (inputAttackX)
        {
            inputAttackX = false;
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            if (actionState == ActionState.IsJump)
            {
                if (isJumpAttack) return;
                isJumpAttack = true;
                Debug.Log("input jump x");
                actionState = ActionState.IsJumpAttack;
                weaponGun.JumpAttackX(inputArrow);
            }
            else
            {
                Debug.Log("input x");
                actionState = ActionState.IsAtk;
                weaponGun.AttackX(inputArrow);
            }
        }
    }

    void Move()
    {
        if (inputDirection != 0)
        {
            if(actionState == ActionState.Idle)
                actionState = ActionState.IsMove;
            animator.SetBool("isWalk", true);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result(), rb.velocity.y);
        }
        else
        {
            if (actionState == ActionState.IsMove)
                actionState = ActionState.Idle;
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    void Run()
    {
        if (inputDirection > 0 && isRrun > 1)
        {
            if (actionState == ActionState.Idle)
                actionState = ActionState.IsMove;
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(inputDirection * (playerStatus.GetMoveSpeed_Result() + 3f), rb.velocity.y);
        }

        if (inputDirection < 0 && isLrun > 1)
        {
            if (actionState == ActionState.Idle)
                actionState = ActionState.IsMove;
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(inputDirection * (playerStatus.GetMoveSpeed_Result() + 3f), rb.velocity.y);
        }
    }
    void Jump()
    {
        if (!inputJump) return;
        inputJump = false;

        if (inputDodge) return;
        if (actionState == ActionState.IsAtk) return;       // 공격 중 입력무시
        if (currentJumpCount < 1 && actionState == ActionState.IsJump) return;

        isGround = false;
        GroundCheck.SetActive(false);
        actionState = ActionState.IsJump;
        
        --currentJumpCount;

        animator.SetBool("isLand", false);
        animator.SetTrigger("isJumpTrigger");
        animator.SetBool("isJump", true);

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0f, playerStatus.GetJumpPower()), ForceMode2D.Impulse);
    }
    void Dodge()
    {
        if (!inputDodge) return;
        inputDodge = false;

        actionState = ActionState.NotMove;
        
        GroundCheck.SetActive(false);
        StartCoroutine(DodgeIgnore(0.5f));

        animator.SetBool("isLand", false);
        animator.SetTrigger("isDodge");

        rb.velocity = Vector2.zero;
        if (inputDirection == arrowDirection)
        {
            rb.AddForce(new Vector2(-arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        }
        dodgable = false;
        invincible = true;

        StartCoroutine(DodgeCount());
        StartCoroutine(InvincibleCount());
    }

    public void StopPlayer()
    {
        actionState = ActionState.NotMove;
        rb.velocity = Vector2.zero;
        StartCoroutine(InputIgnore());
        Debug.Log("input ignore");
        animator.SetTrigger("PlayerStop");
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        weaponSpear.InputInit();
    }

    IEnumerator InputIgnore()
    {
        yield return new WaitForSeconds(0.5f);
        actionState = ActionState.Idle;
    }

    public void Hit(int attack)
    {
        if (invincible) return;

        if (actionState == ActionState.IsParrying)
        {
            animator.SetBool("is_x_Atk", true);
            Debug.Log("parrying");
        }
        else
        {
            actionState = ActionState.NotMove;

            playerStatus.DecreaseHP(attack);
            animator.SetTrigger("isHit");
            invincible = true;
            StartCoroutine(InvincibleCount());

            actionState = ActionState.Idle;
        }
    }
    public void Landing()
    {
        if (actionState == ActionState.IsAtk) return;
        isGround = true;
        isJumpAttack = false;
        Debug.Log("Landing");
        animator.SetBool("isJump", false);
        animator.SetBool("isJump_x_Atk", false);
        animator.SetBool("isLand", true);
        Debug.Log(currentJumpCount);
        currentJumpCount = (int)playerStatus.GetJumpCount();
        actionState = ActionState.Idle;
    }
    public void ParryingCheck()
    {
        animator.SetBool("is_x_Atk", true);
        Debug.Log("parrying");
    }

    IEnumerator DodgeIgnore(float time)
    {
        yield return new WaitForSeconds(time);
        GroundCheck.SetActive(true);
    }
    
    void Dead()
    {
        Debug.Log("Dead!!");
    }

    public void AttackDistance(float DistanceMulty)
    {
        playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection
            , transform.position.y + 0.1f), new Vector2(arrowDirection, 0), DistanceMulty, rayDashLayerMask);
        
        if (playerDashBotDistance)
        {
            float botDistance = playerDashBotDistance.point.x - (transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection);
            if (DistanceMulty > Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f)
                DistanceMulty = Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f;
            transform.position = new Vector2(transform.position.x + DistanceMulty * arrowDirection, transform.position.y);
        }
        else
            transform.position = new Vector2(transform.position.x + DistanceMulty * arrowDirection * 0.1f, transform.position.y);
        Debug.Log(DistanceMulty);
    }
    public void DashAttackDistance(float dashDistanceMulty)
    {
        playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection
            , transform.position.y), new Vector2(arrowDirection, 0), dashDistanceMulty, rayDashLayerMask);
        
        if (playerDashBotDistance)
        {
            float botDistance = playerDashBotDistance.point.x - (transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection);
            if (dashDistanceMulty > Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f)
                dashDistanceMulty = Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f;
            transform.position = new Vector2(transform.position.x + dashDistanceMulty * arrowDirection, transform.position.y);
        }
        else
            transform.position = new Vector2(transform.position.x + dashDistanceMulty * playerStatus.GetDashDistance_Result() * arrowDirection * 0.5f, transform.position.y);
        Debug.Log(dashDistanceMulty);
    }

    public void PlayerMoveSet()
    {
        if (!dodgable) return;
        actionState = ActionState.Idle;
    }
    public void PlayerJumpAttackEnd()
    {
        actionState = ActionState.IsJump;
    }

    public void SetAttackState(int _attackState)
    {
        if (weaponType == 0)
            weaponSpear.SetAttackState(_attackState);
        else
            weaponGun.SetAttackState(_attackState);
    }
    public void InputInit()
    {
        if(weaponType == 0)
            weaponSpear.InputInit();
        else
            weaponGun.InputInit();
    }
    public void SetAnimationAttackSpeed(float _attackSpeed)
    {
        animator.SetFloat("AttackSpeed", _attackSpeed);
    }
    
    public void Attack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY)
    {
        monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackPosX * GetArrowDirection())
            , transform.position.y + attackPosY), new Vector2(attackRangeX, attackRangeY), 0);

        if (monster != null)
        {
            overlap = monster.Length;
            for (int i = 0; i < overlap; ++i)
            {
                if (monster[i].CompareTag("Monster"))
                {
                    monster[i].gameObject.GetComponent<IsDamageable>().Hit(playerStatus.GetAttack_Result());
                }
            }
        }
    }
}
