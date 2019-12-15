using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public GameObject GroundCheck;
    public PlayerStatus playerStatus;
    
    private float inputDirection;
    
    private float runDelay;
    private int isRrun;
    private int isLrun;

    private int inputArrow;
    private bool inputAttackX;
    public bool inputAttackY;
    public bool inputJump;
    private bool inputDodge;
    
    private Queue inputAttackList = new Queue();
    private bool attackLock;
    private int commandCount;
    private int attackState;
    private int attackPattern;

    private float parryingCount;
    private float dodgeCount;
    private bool invincible;
    private float invincibleCount;

    public int currentJumpCount;

    private bool isBlock;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    public void Start()
    {
        currentJumpCount = (int)playerStatus.GetJumpCount();
        arrowDirection = 1;
        dodgeCount = 0.5f;
        parryingCount = 0.2f;
        commandCount = 1;
        attackState = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // 무적 시간
        if (invincibleCount > 0)
        {
            invincibleCount -= Time.deltaTime;
            if (invincibleCount <= 0)
                invincible = false;
        }

        if (Input.GetButtonDown("Fire1") && !animator.GetBool("is_y_Atk")) inputAttackX = true;

        if (Input.GetButtonUp("Fire2") && inputAttackY) inputAttackY = false;
        
        if (rb.velocity.y <= -0.5f)
        {
            GroundCheck.SetActive(true);
        }

        if (actionState == ActionState.NotMove) return;             // notMove 가 아닐 때

        inputDirection = Input.GetAxisRaw("Horizontal");
        
        // 대쉬 딜레이
        RunCheck();

        // 회피
        if (dodgeCount > 0) dodgeCount -= Time.deltaTime;

        if (actionState == ActionState.IsParrying)
        {
            parryingCount -= Time.deltaTime;
            if (parryingCount < 0f)
            {
                actionState = ActionState.Idle;
                parryingCount = 0.2f;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
            inputArrow = 30;
        else if (Input.GetKey(KeyCode.DownArrow))
            inputArrow = 40;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            inputArrow = 10;
        else
            inputArrow = 0;

        if (Input.GetButtonDown("Fire3") && dodgeCount <= 0) inputDodge = true;

        if (actionState == ActionState.IsAtk) return;             // notMove 가 아닐 때

        if (Input.GetButtonDown("Fire2")) inputAttackY = true;
        if (Input.GetButtonDown("Jump")) inputJump = true;
    }
    
    public void InputInit()
    {
        inputAttackList.Clear();
        animator.SetBool("is_x_Atk", false);
        animator.SetBool("is_xx_Atk", false);
        animator.SetBool("is_xFx_Atk", false);
        animator.SetBool("is_xxx_Atk", false);
        animator.SetBool("is_xFxFx_Atk", false);
        animator.SetBool("is_y_Atk", false);
        commandCount = 1;
        attackState = 1;
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
        if (actionState == ActionState.NotMove) return;

        Dodge();
        AttackX();
        AttackY();
        
        if (actionState == ActionState.IsAtk) return;

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

    void AttackX()
    {
        if (inputAttackX)
        {
            inputAttackX = false;

            if (actionState == ActionState.IsJump)
            {
                inputAttackList.Enqueue(inputArrow + 6);
                if (!attackLock)
                    StartCoroutine(AttackList());
            }
            else
            {
                if (commandCount <= attackState)
                {
                    actionState = ActionState.IsAtk;
                    inputAttackList.Enqueue(inputArrow + commandCount);
                    ++commandCount;

                    if (!attackLock)
                        StartCoroutine(AttackList());

                    if (commandCount > 3)
                        commandCount = 1;
                }
            }
        }
    }
    void AttackY()
    {
        if (inputAttackY)
        {
            if (actionState == ActionState.IsJump) return;
            actionState = ActionState.IsAtk;

            if (attackPattern != 0) inputAttackList.Clear();

            inputAttackList.Enqueue(inputArrow + 5);
            if (!attackLock)
                StartCoroutine(AttackList());
        }
        else
        {
            if (animator.GetBool("is_y_Atk"))
            {
                inputAttackList.Enqueue(0);
                if (!attackLock)
                    StartCoroutine(AttackList());
            }
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
        --currentJumpCount;
        actionState = ActionState.IsJump;

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

        GroundCheck.SetActive(false);
        StartCoroutine(DodgeIgnore(0.5f));

        animator.SetTrigger("isDodge");
        rb.velocity = Vector2.zero;
        if (inputDirection != arrowDirection)
            rb.AddForce(new Vector2(-arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(arrowDirection * 4f, 5f), ForceMode2D.Impulse);
        dodgeCount = 0.5f;
        
        invincible = true;
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
            animator.SetTrigger("isHit");
            invincible = true;
            invincibleCount = 1f;

            playerStatus.DecreaseHP(attack);
            actionState = ActionState.Idle;
        }
    }
    public void Landing()
    {
        actionState = ActionState.Idle;
        animator.SetBool("isJump", false);
        animator.SetBool("isJump_x_Atk", false);
        animator.SetTrigger("isLanding");
        currentJumpCount = (int)playerStatus.GetJumpCount();
    }
    public void ParryingCheck()
    {
        animator.SetBool("is_x_Atk", true);
        Debug.Log("parrying");
    }
    public void PlayerMoveSet()
    {
        actionState = ActionState.Idle;
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
        rb.velocity = new Vector2(dashDistanceMulty * arrowDirection, rb.velocity.y);
    }
    
    IEnumerator AttackList()
    {
        attackLock = !attackLock;
        do
        {
            actionState = ActionState.IsAtk;
            if (rb.velocity.x * rb.velocity.x > 0)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isRun", false);
                isRrun = 0;
                isLrun = 0;
            }
            switch (inputAttackList.Dequeue())
            {
                case 1:
                case 11:
                    attackPattern = 0;
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 31:
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 41:
                    actionState = ActionState.IsParrying;
                    animator.SetBool("is_x_Atk", true);
                    break;
                case 2:
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 12:
                    attackPattern = 1;
                    animator.SetBool("is_xFx_Atk", true);
                    break;
                case 32:
                    attackPattern = 2;
                    animator.SetBool("is_xx_Atk", true);
                    break;
                case 3:
                    if(attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 13:
                    if (attackPattern == 0)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else if (attackPattern == 1)
                    {
                        animator.SetBool("is_xFxFx_Atk", true);
                    }
                    break;
                case 33:
                    if (attackPattern == 1)
                    {
                        animator.SetBool("is_xxx_Atk", true);
                    }
                    else
                        animator.SetBool("is_xxx_Atk", true);
                    break;
                case 5:
                    animator.SetBool("is_y_Atk", true);
                    break;
                case 15:
                case 35:
                case 45:
                    animator.SetBool("is_y_Atk", true);
                    break;
                case 0:
                    animator.SetBool("is_y_up_Atk", true);
                    break;
                case 6:
                case 16:
                case 36:
                case 46:
                    animator.SetBool("isJump_x_Atk", true);
                    break;
            }
            yield return null;
        } while (inputAttackList.Count > 0);
        attackLock = !attackLock;
    }

    public void SetAttackState(int _attackState)
    {
        attackState = _attackState;
    }
    public void AttackMotionCheck()
    {
        if (attackPattern == 0)
        {
            if (!animator.GetBool("is_xx_Atk"))
            {
                InputInit();
                PlayerMoveSet();
            }
        }
        else if (attackPattern == 1)
        {
            if (!animator.GetBool("is_xFx_Atk"))
            {
                InputInit();
                PlayerMoveSet();
            }
        }
        else if (attackPattern == 2)
        {
            if (!animator.GetBool("is_xFx_Atk"))
            {
                InputInit();
                PlayerMoveSet();
            }
        }
    }
}
