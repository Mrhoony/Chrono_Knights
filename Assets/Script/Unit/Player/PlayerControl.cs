using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public GameObject GroundCheck;
    public PlayerStatus playerStatus;
    public Weapon_Spear weaponSpear;
    public Weapon_Gun weaponGun;    // Gun_Controller.cs

    public int weaponType;

    public float inputDirection;

    public int inputArrow;
    public bool inputAttackX;
    public bool inputAttackY;
    public bool finalAttackY;
    public float attackSpeed;

    public float runDelay;
    public int isRrun;
    public int isLrun;
    public bool isFall;
    
    public bool inputJump;
    public bool jumpping;
    public bool inputDodge;
    
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
        playerStatus = GetComponent<PlayerStatus>();
        weaponSpear = GetComponent<Weapon_Spear>();
        weaponSpear.Init(animator, rb);
        weaponGun = GetComponent<Weapon_Gun>(); // weaponGun 선언

        Init();

        Debug.Log("control awake");
    }

    public void Init()
    {
        playerStatus.Init();
        currentJumpCount = (int)playerStatus.GetJumpCount();
        arrowDirection = 1;
        weaponType = 0;
        attackSpeed = 1;
        isFall = false;
        dodgable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponType == 0)
        {
            if (Input.GetButtonDown("Fire1") && !animator.GetBool("is_y_Atk")) inputAttackX = true;
            if (Input.GetButtonUp("Fire2") && inputAttackY)
            {
                inputAttackY = false;
                finalAttackY = true;
            }
        }

        if (actionState == ActionState.NotMove) return;             // notMove 가 아닐 때

        inputDirection = Input.GetAxisRaw("Horizontal");

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
            if (!isFall)
            {
                if (!jumpping)
                {
                    --currentJumpCount;
                    isFall = true;
                    animator.SetTrigger("isFallTrigger");
                }
            }
            GroundCheck.SetActive(true);
        }

        // 대쉬 딜레이
        RunCheck();
        
        if (actionState == ActionState.IsParrying) StartCoroutine(ParryingCount());

        if (Input.GetKey(KeyCode.UpArrow))
            inputArrow = 30;
        else if (Input.GetKey(KeyCode.DownArrow))
            inputArrow = 40;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            inputArrow = 10;
        else
            inputArrow = 0;

        if (Input.GetButtonDown("Fire3") && dodgable) inputDodge = true;

        if (actionState == ActionState.IsAtk) return;

        if (Input.GetButtonDown("Fire2")) inputAttackY = true;
        if (Input.GetButtonDown("Jump")) inputJump = true;

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(weaponType == 0) // 스피어 -> 건
            {
                weaponType = 1;
                weaponSpear.enabled = false;    // 스피어 오프
                weaponGun.enabled = true;       // 건 온
                animator.runtimeAnimatorController = Resources.Load(weaponGun.ControllerPath) as RuntimeAnimatorController; // 건 애니메이터 변경
            }
            else if (weaponType == 1)
            {
                weaponType = 0;
                weaponGun.enabled = false;
                weaponSpear.enabled = true;
                animator.runtimeAnimatorController = Resources.Load(weaponGun.ControllerPath) as RuntimeAnimatorController; // 스피어 애니메이터 변경
            }
        }
    }
    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
    IEnumerator ParryingCount()
    {
        yield return new WaitForSeconds(0.2f);
        actionState = ActionState.Idle;
    }
    IEnumerator DodgeCount()
    {
        yield return new WaitForSeconds(3f);
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

    private void FixedUpdate()
    {
        if (actionState == ActionState.NotMove) return;     // 피격 시 입력무시

        if (weaponType == 0) SpearAttack();
        if (weaponType == 1) SpearAttack();

        Dodge();

        if (actionState == ActionState.IsAtk) return;       // 공격 중 입력무시
        
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
        Jump();
    }

    void SpearAttack()
    {
        if (inputAttackX)
        {
            if (actionState == ActionState.IsJump)
            {
                weaponSpear.JumpAttackX(inputArrow);
                isFall = false;
            }
            else
            {
                weaponSpear.AttackX(inputArrow);
                inputAttackX = false;
            }
            actionState = ActionState.IsAtk;
        }
        else if (inputAttackY)
        {
            weaponSpear.AttackY(inputArrow);
            actionState = ActionState.IsAtk;
        }
        else if (finalAttackY)
        {
            finalAttackY = false;
            weaponSpear.AttackYFinal();
        }
    }

    void Move()
    {
        if (inputDirection != 0)
        {
            animator.SetBool("isWalk", true);
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
            rb.velocity = new Vector2(inputDirection * (playerStatus.GetMoveSpeed_Result() + 3f), rb.velocity.y);
        }

        if (inputDirection < 0 && isLrun > 1)
        {
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(inputDirection * (playerStatus.GetMoveSpeed_Result() + 3f), rb.velocity.y);
        }
    }
    void Jump()
    {
        if (!inputJump) return;
        inputJump = false;
        if (currentJumpCount < 1) return;
        jumpping = true;
        --currentJumpCount;
        actionState = ActionState.IsJump;

        animator.SetBool("isLand", false);
        animator.SetTrigger("isJumpTrigger");
        animator.SetBool("isJump", true);
        GroundCheck.SetActive(false);

        rb.AddForce(new Vector2(0f, playerStatus.GetJumpPower()), ForceMode2D.Impulse);
    }
    void Dodge()
    {
        if (!inputDodge) return;
        inputDodge = false;

        actionState = ActionState.NotMove;
        rb.velocity = Vector2.zero;
        
        GroundCheck.SetActive(false);
        StartCoroutine(DodgeIgnore(0.5f));

        animator.SetBool("isLand", false);
        animator.SetTrigger("isDodge");
        if (inputDirection != arrowDirection)
            rb.AddForce(new Vector2(-arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        dodgable = false;
        invincible = true;

        StartCoroutine(DodgeCount());
        StartCoroutine(InvincibleCount());
    }

    public void fly()
    {
        animator.SetBool("isLand", false);
    }

    public void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("PlayerStop");
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
        Debug.Log("Landing");
        currentJumpCount = (int)playerStatus.GetJumpCount();
        actionState = ActionState.Idle;
        animator.SetBool("isJump", false);
        animator.SetBool("isJump_x_Atk", false);
        animator.SetBool("isLand", true);
        isFall = false;
        jumpping = false;
        Debug.Log(currentJumpCount);
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
    public void DashAttackDistance(float dashDistanceMulty)
    {
        transform.position = new Vector2(transform.position.x + dashDistanceMulty * playerStatus.GetDashDistance_Result() * arrowDirection * 0.1f, transform.position.y);
    }
    public void PlayerMoveSet()
    {
        actionState = ActionState.Idle;
    }
}
