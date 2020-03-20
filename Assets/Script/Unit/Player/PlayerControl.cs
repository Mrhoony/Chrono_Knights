using System.Collections;
using UnityEngine;

public enum GunEft {
    shot1, shot2, downshot
}

public enum AtkType
{
    notMove, oneStep, fowardDash, fowardBack, down,
    g_notMove, g_oneStep, g_backJump, g_down
}

public class PlayerControl : MovingObject
{
    public static TestDrawBox TDB = new TestDrawBox();

    public static PlayerControl instance;
    public BoxCollider2D playerCharacterCollider;
    public LayerMask rayDashLayerMask;
    public LayerMask rayGroundLayerMask;
    public GameObject GroundCheck;
    public PlayerStatus playerStatus;
    public SkillManager skillManager;
    public GameObject playerEffect;

    public GameObject[] gunEffect;
    public GameObject[] shotPoint;

    public Weapon_Spear weaponSpear;
    public Weapon_Gun weaponGun;

    public int weaponType;
    public int[] weaponMultyHit;
    public int multyHitCount;

    public float inputDirection;

    public int inputArrow;

    public float runDelay;
    public int isRrun;
    public int isLrun;
    
    public bool isGround;
    
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
        animator = GetComponentInChildren<Animator>();
        playerCharacterCollider = GetComponent<BoxCollider2D>();
        playerStatus = GetComponent<PlayerStatus>();
        weaponSpear = GetComponent<Weapon_Spear>();
        weaponSpear.Init(animator, rb);
        weaponMultyHit = weaponSpear.GetWeaponMultyHit();
        weaponGun = GetComponent<Weapon_Gun>();
        skillManager.Init();

        multyHitCount = 0;
        arrowDirection = 1;
        weaponType = 0;
        dodgable = true;

        Debug.Log("control awake");
    }

    // Update is called once per frame
    void Update()
    {
        if (CanvasManager.instance.GameMenuOnCheck()) return;       // UI 켜져 있을 때 입력 제한
        if (actionState == ActionState.IsDead) return;              // 죽었을 때 입력 제한
        if (actionState == ActionState.IsDodge) return;             // 회피중일 때 입력 제한

        inputDirection = Input.GetAxisRaw("Horizontal");

        if (actionState == ActionState.NotMove) return;       // notMove 가 아닐 때

        // x 공격 입력
        if (weaponType == 0)        // 현재 무기가 창이면
        {
            SpearAttack();
        }
        else if(weaponType == 1)    // 현재 무기가 총이면
        {
            GunAttack();
        }
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
        }              // shift 키 입력시 대쉬

        // 낙하 체크
        if (rb.velocity.y <= -0.5f)
        {
            if (actionState != ActionState.IsJump && !isGround)
            {
                actionState = ActionState.IsJump;
                --currentJumpCount;
            }
            GroundCheck.SetActive(true);
        }

        // 대쉬 딜레이
        RunCheck();
        
        if (actionState == ActionState.IsParrying) StartCoroutine(ParryingCount());     // 패링시 패링 쿨타임

        // 커맨드용 입력
        if (Input.GetKey(KeyCode.UpArrow))          inputArrow = 30;
        else if (Input.GetKey(KeyCode.DownArrow))   inputArrow = 40;
        else if (inputDirection != 0)               inputArrow = 10;
        else                                        inputArrow = 0;

        if (Input.GetButtonDown("Fire3") && dodgable) Dodge();      // 회피

        if (actionState == ActionState.IsAtk) return;

        if (Input.GetButtonDown("Jump")) Jump();                    // 점프

        if (actionState != ActionState.Idle) return;

        InputSkillButton();
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(weaponType == 0) // 스피어 -> 건
            {
                weaponType = 1;
                weaponGun.Init(animator, rb);
                weaponMultyHit = weaponGun.GetWeaponMultyHit();
            }
            else if (weaponType == 1)
            {
                weaponType = 0;
                weaponSpear.Init(animator, rb);
                weaponMultyHit = weaponSpear.GetWeaponMultyHit();
            }
        }       // 장착 무기 변경
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
        yield return new WaitForSeconds(0.3f);
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
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (playerStatus.playerEquip.equipment[1].skillCode == 0 || playerStatus.playerEquip.equipment[1].isUsed) return;
            Debug.Log("스킬 2 입력");
        }
        else
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (actionState != ActionState.Idle && actionState != ActionState.IsMove && actionState != ActionState.IsJump) return;     // 피격 시 입력무시
        
        Move();
        Run();
        // 캐릭터 뒤집기
        if (inputDirection > 0 && isFaceRight)
        {
            Flip();
        }
        else if (inputDirection < 0 && !isFaceRight)
        {
            Flip();
        }
    }

    void SpearAttack()
    {
        if (Input.GetButtonDown("Fire1") && !animator.GetBool("is_y_Atk"))
        {
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            if (actionState == ActionState.IsJump)
            {
                actionState = ActionState.IsJumpAttack;
                weaponSpear.JumpAttackX(inputArrow);
            }
            else
            {
                actionState = ActionState.IsAtk;
                weaponSpear.AttackX(inputArrow);
            }
        }

        if (Input.GetButton("Fire2"))
        {
            if (actionState == ActionState.Idle || actionState == ActionState.IsMove)
            {
                actionState = ActionState.IsAtk;
                weaponSpear.AttackY(inputArrow);
            }
        }
        if (Input.GetButtonUp("Fire2"))
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
    void GunAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (actionState == ActionState.IsJumpAttack) return;       // 공격 중 입력무시
            if (actionState == ActionState.IsJump)
            {
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

    void Jump()
    {
        if (actionState == ActionState.IsJumpAttack) return;
        if (currentJumpCount < 1 && actionState == ActionState.IsJump) return;

        actionState = ActionState.IsJump;
        isGround = false;
        GroundCheck.SetActive(false);
        
        --currentJumpCount;

        animator.SetBool("isLand", false);
        animator.SetTrigger("isJumpTrigger");
        animator.SetBool("isJump", true);

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0f, playerStatus.GetJumpPower()), ForceMode2D.Impulse);
    }
    void Dodge()
    {
        if (!dodgable) return;
        dodgable = false;
        actionState = ActionState.IsDodge;
        
        GroundCheck.SetActive(false);
        StartCoroutine(DodgeIgnore(0.5f));

        animator.SetBool("isLand", false);
        animator.SetTrigger("isDodge");
        
        if (inputDirection == arrowDirection)
        {
            rb.velocity = new Vector2(arrowDirection * playerStatus.GetDashDistance_Result() * 0.5f, 5f);
        }
        else
        {
            rb.velocity = new Vector2(-arrowDirection * playerStatus.GetDashDistance_Result() * 0.5f, 5f);
        }
        
        invincible = true;

        StartCoroutine(DodgeCount());
        StartCoroutine(InvincibleCount());
    }

    void Move()
    {
        if (inputDirection != 0)
        {
            if (actionState == ActionState.Idle)
                actionState = ActionState.IsMove;
            animator.SetBool("isWalk", true);
            animator.SetBool("isRun", false);
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
            animator.SetBool("isWalk", false);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result() * 2f, rb.velocity.y);
        }

        if (inputDirection < 0 && isLrun > 1)
        {
            if (actionState == ActionState.Idle)
                actionState = ActionState.IsMove;
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
            rb.velocity = new Vector2(inputDirection * playerStatus.GetMoveSpeed_Result() * 2f, rb.velocity.y);
        }
    }
    
    IEnumerator InputIgnore()
    {
        yield return new WaitForSeconds(0.5f);
        actionState = ActionState.Idle;
    }

    public void Hit(int attack)
    {
        if (invincible)
        {
            playerEffect.GetComponent<Animator>().SetTrigger("isDodge_Trigger");
            return;
        }
        else if (actionState == ActionState.IsParrying)
        {
            animator.SetBool("is_x_Atk", true);
            return;
        }
        else
        {
            StopPlayer();

            CameraManager.instance.CameraShake(playerStatus.DecreaseHP(attack));
            animator.SetTrigger("isHit");
            playerEffect.GetComponent<Animator>().SetTrigger("isHit_Trigger");

            rb.velocity = new Vector2(-arrowDirection * 3f, 2.5f);

            invincible = true;
            StartCoroutine(InvincibleCount());
        }
    }
    public void Landing()
    {
        isGround = true;
        animator.SetBool("isJump", false);
        animator.SetBool("isJump_x_Atk", false);
        animator.SetBool("isLand", true);
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

    public void PlayerJumpAttackEnd()
    {
        actionState = ActionState.IsJump;
    }

    public void StopPlayer()
    {
        actionState = ActionState.NotMove;
        rb.velocity = Vector2.zero;
        StartCoroutine(InputIgnore());
        animator.SetTrigger("PlayerStop");
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        InputInit();
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
        {
            weaponSpear.InputInit();
        }
        else
        {
            weaponGun.InputInit();
        }
    }
    public void MoveSet()
    {
        if (!dodgable) return;
        actionState = ActionState.Idle;
    }
    public void SetAnimationAttackSpeed(float _attackSpeed)
    {
        animator.SetFloat("AttackSpeed", _attackSpeed);
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
    
    public float Attack(float attackPosX, float attackPosY, float attackRangeX, float attackRangeY, AtkType _dashDisType)
    {
        Collider2D[] monster;
        float attackDistance = 0f;
        int attackState;
        if (weaponType == 0)
            attackState = weaponSpear.GetAttackState();
        else
            attackState = weaponGun.GetAttackState();

        for (int i = 0; i < weaponMultyHit[attackState - 1]; ++i)
        {
            switch (_dashDisType)
            {
                //움직임 없는 공격
                case AtkType.notMove:
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + attackPosX
                        , transform.position.y + attackPosY), new Vector2(attackRangeX, attackRangeY), 0);
                    break;
                //xx, xFx, xxx 등 한걸음 이동
                case AtkType.oneStep:
                    attackDistance = playerStatus.GetDashDistance_Result() * 0.25f;
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackDistance + 0.2f) * arrowDirection * 0.5f
                        , transform.position.y + attackPosY), new Vector2(attackDistance + 0.4f, attackRangeY), 0);
                    break;
                //xFxFx 등 
                case AtkType.fowardDash:
                    attackDistance = playerStatus.GetDashDistance_Result() * 0.5f;
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackDistance + 0.2f) * arrowDirection * 0.5f
                        , transform.position.y + attackPosY), new Vector2(attackDistance + 0.4f, attackRangeY), 0);
                    break;
                //앞뒤 동시 공격
                case AtkType.fowardBack:
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x
                        , transform.position.y + attackPosY), new Vector2(attackDistance + 0.6f, attackRangeY), 0);
                    TDB.DrawBox(transform.position, new Vector2(attackDistance + 0.6f, attackRangeY));
                    break;
                case AtkType.g_backJump:
                    attackDistance = playerStatus.GetDashDistance_Result() * 0.5f;
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackDistance + 0.5f) * arrowDirection * 0.5f
                        , transform.position.y + attackPosY), new Vector2(attackDistance + 1f, attackRangeY), 0);
                    break;
                default:
                    monster = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackPosX * arrowDirection)
                        , transform.position.y + attackPosY), new Vector2(attackRangeX, attackRangeY), 0);
                    break;
            }

            if (monster != null)
            {
                overlap = monster.Length;
                for (int j = 0; j < overlap; ++j)
                {
                    if (monster[j].CompareTag("Monster"))
                    {
                        monster[j].gameObject.GetComponent<IsDamageable>().Hit(playerStatus.GetAttack_Result());
                    }
                    else if (monster[j].CompareTag("BossMonster"))
                    {
                        monster[j].gameObject.GetComponent<IsDamageable>().Hit(playerStatus.GetAttack_Result());
                    }
                }
            }
        }
        return attackDistance;
    }

    public void AttackDistance(float _distanceMulty)
    {
        RaycastHit2D playerDashBotDistance = Physics2D.Raycast(new Vector2(transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection
            , transform.position.y + 0.1f), new Vector2(arrowDirection, 0), _distanceMulty, rayDashLayerMask);

        if (playerDashBotDistance)
        {
            float botDistance = playerDashBotDistance.point.x - (transform.position.x + playerCharacterCollider.size.x * 0.5f * arrowDirection);
            if (_distanceMulty > Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f)
                _distanceMulty = Mathf.Abs(botDistance) - playerCharacterCollider.size.x * 0.5f;
            transform.position = new Vector2(transform.position.x + _distanceMulty * arrowDirection, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + _distanceMulty * arrowDirection, transform.position.y);
        }
    }

    public void AttackDistanceForce(float _distanceMulty)
    {
        isGround = false;
        GroundCheck.SetActive(false);

        --currentJumpCount;

        animator.SetBool("isLand", false);

        rb.velocity = new Vector2(arrowDirection * -1 * (_distanceMulty * 2f + 5f), _distanceMulty * 2f + 5f);
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