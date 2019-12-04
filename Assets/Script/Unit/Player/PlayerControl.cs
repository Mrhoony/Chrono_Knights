using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public GameObject GroundCheck;
    public PlayerStatus pStat;

    public Vector2 movement;
    public float inputDirection;
    public int arrowDirection;
    
    private float runDelay;
    private int isRrun;
    private int isLrun;

    public bool isJump;
    public bool jumping;
    public bool isDownJump;
    public bool isGround;
    public bool isSlope;
    public float slopeDelay;
    public bool parrying;
    public bool isParrying;

    public Queue inputAttack = new Queue();
    public bool isAtk;
    public bool attackLock;
    public int inputArrow;
    public int commandCount = 1;
    public int attackPattern;
    public bool inputY;
    public int atkState = 1;

    public float parryingCount;
    public bool isDodge;
    public float dodgeCount;
    public bool dodging;
    public bool isDamagable;
    public float isDamagableCount;
    
    public int currentJumpCount;

    public bool isBlock;

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
        pStat = GetComponent<PlayerStatus>();
    }

    public void Start()
    {
        currentJumpCount = pStat.jumpCount;
        arrowDirection = 1;
        dodgeCount = 0.5f;
        slopeDelay = 0;
        parryingCount = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!jumping)
        {
            if (Input.GetButtonDown("Fire1") && commandCount <= atkState && !inputY)
            {
                if (!isAtk)
                    rb.velocity = Vector2.zero;

                notMove = true;
                inputAttack.Enqueue(inputArrow + commandCount);
                ++commandCount;

                if (!attackLock)
                    StartCoroutine(Attack());

                if (commandCount > 3)
                    commandCount = 1;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                notMove = true;
                inputY = true;

                if (attackPattern != 0) inputAttack.Clear();

                inputAttack.Enqueue(inputArrow + 5);
                if (!attackLock)
                    StartCoroutine(Attack());
            }

            if (Input.GetButtonUp("Fire2") && inputY)
            {
                notMove = true;
                if (animator.GetBool("is_y_Atk"))
                {
                    inputAttack.Enqueue(0);
                    if (!attackLock)
                        StartCoroutine(Attack());
                }
            }

            if (inputDirection != 0) animator.SetBool("isWalk", true);
            else
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                notMove = true;
                animator.SetBool("isJump_x_Atk", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            pStat.SetBuff(1);
        }

        if (notMove) return;

        inputDirection = Input.GetAxisRaw("Horizontal");
        
        // 대쉬 딜레이
        RunCheck();

        // 회피
        if (dodgeCount > 0) dodgeCount -= Time.deltaTime;

        if (parrying)
        {
            parryingCount -= Time.deltaTime;
            if (parryingCount < 0f)
            {
                parrying = false;
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
        
        if (Input.GetButtonDown("Jump")) isJump = true;

        // 캐릭터 뒤집기
        if (inputDirection > 0 && isFaceRight)
        {
            Flip();
            arrowDirection *= -1;
        }
        else if (inputDirection < 0 && !isFaceRight)
        {
            Flip();
            arrowDirection *= -1;
        }

        if (Input.GetButtonDown("Fire3") && dodgeCount <= 0) isDodge = true;

        // 무적 시간
        if(isDamagableCount > 0)
        {
            isDamagableCount -= Time.deltaTime;
            if (isDamagableCount <= 0)
                isDamagable = false;
        }
        //if (isDownJump) return;
        
        if (jumping)
        {
            if (rb.velocity.y <= -0.5f)
            {
                if (slopeDelay > 0)
                    GroundCheck.SetActive(false);
                else
                    GroundCheck.SetActive(true);
            }
        }

        if(slopeDelay > 0)
        {
            slopeDelay -= Time.deltaTime;
        }

        if (isSlope)
        {
            if (slopeDelay <= 0)
            {
                slopeDelay = 0;
                isSlope = false;
                jumping = false;
                animator.SetBool("isJump", false);
                animator.SetBool("isJump_x_Atk", false);
                animator.SetTrigger("isLanding");
                currentJumpCount = pStat.jumpCount;
            }
        }
    }
    
    public void InputInit()
    {
        inputAttack.Clear();
        animator.SetBool("is_x_Atk", false);
        animator.SetBool("is_xx_Atk", false);
        animator.SetBool("is_xFx_Atk", false);
        animator.SetBool("is_xxx_Atk", false);
        animator.SetBool("is_xFxFx_Atk", false);
        commandCount = 1;
        atkState = 1;
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
        if (notMove) return;
        if (dodging) return;

        Dodge();
        
        if (isDodge) return;

        Move();
        Run();
        Jump();
    }

    void Move()
    {
        rb.velocity = new Vector2(inputDirection * pStat.moveSpeed, rb.velocity.y);
    }
    void Run()
    {
        if (inputDirection > 0 && isRrun > 1)
        {
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(inputDirection * (pStat.moveSpeed + 3f), rb.velocity.y);
        }

        if (inputDirection < 0 && isLrun > 1)
        {
            animator.SetBool("isRun", true);
            rb.velocity = new Vector2(inputDirection * (pStat.moveSpeed + 3f), rb.velocity.y);
        }
    }
    void Jump()
    {
        if (isJump)
        {
            isJump = false;

            if ((currentJumpCount < 1 && jumping))
                return;

            isDownJump = false;
            jumping = true;

            rb.velocity = Vector2.zero;

            animator.SetTrigger("isJumpTrigger");
            animator.SetBool("isJump", true);
            GroundCheck.SetActive(false);

            rb.AddForce(new Vector2(0f, pStat.jumpPower), ForceMode2D.Impulse);
            isGround = false;

            --currentJumpCount;

            //StartCoroutine(JumpIgnore(pStat.jumpPower * 0.1f));
        }
    }
    void Dodge()
    {
        if (isDodge && !dodging)
        {
            dodging = true;
            dodgeCount = 0.5f;
            animator.SetTrigger("isDodge");
            rb.velocity = Vector2.zero;

            GroundCheck.SetActive(false);
            StartCoroutine(DodgeIgnore(0.5f));

            if (inputDirection != arrowDirection)
                rb.AddForce(new Vector2(-arrowDirection * 4f, 5f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(arrowDirection * 4f, 5f), ForceMode2D.Impulse);

            isDamagable = true;
            notMove = true;
        }
    }
    
    IEnumerator DodgeIgnore(float time)
    {
        yield return new WaitForSeconds(time);
        GroundCheck.SetActive(true);
    }

    /*
    IEnumerator JumpIgnore(float time)
    {
        yield return new WaitForSeconds(time);
        GroundCheck.SetActive(true);
    }
    */
    public void Hit(int monsterAtk)
    {
        if (isDamagable)
            return;

        if (parrying)
        {
            isParrying = true;
            animator.SetBool("is_x_Atk", true);
            isParrying = false;
            Debug.Log("parrying");
        }

        notMove = true;
        animator.SetTrigger("isHit");
        isDamagable = true;
        isDamagableCount = 1f;
        
        pStat.DecreaseHP(monsterAtk);
    }

    void Dead()
    {
        Debug.Log("Dead!!");
    }
    
    IEnumerator Attack()
    {
        attackLock = !attackLock;
        do
        {
            if (rb.velocity.x * rb.velocity.x > 0)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("isRun", false);
                isRrun = 0;
                isLrun = 0;
            }
            isAtk = true;
            switch (inputAttack.Dequeue())
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
                    parrying = true;
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
            }
            isAtk = false;
            yield return null;
        } while (inputAttack.Count > 0);
        attackLock = !attackLock;
    }
}
