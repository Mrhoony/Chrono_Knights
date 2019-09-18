using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MovingObject
{
    public static PlayerControl instance;
    public GameObject GroundCheck;
    public PlayerStat pStat;

    public float inputDirection;
    public int arrowDirection;
    
    private float runDelay;
    private int isRrun;
    private int isLrun;
    
    public bool jumping;
    public bool isDownJump;
    public bool isCollision;
    public bool isGround;

    public Queue inputAttack = new Queue();
    public bool isAtk;
    private bool attackLock;
    private int inputArrow;
    private int commandCount = 1;
    public int attackPattern;
    public bool inputY;
    public int atkState = 1;

    public bool isDodge;
    private float dodgeCount;
    public bool dodging;
    public bool isDamagable;
    public float isDamagableCount;

    public int jumpCount = 1;
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
        pStat = GetComponent<PlayerStat>();

        currentJumpCount = jumpCount;
        arrowDirection = 1;
        dodgeCount = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");

        if(inputDirection != 0)
            animator.SetBool("isWalk", true);
        else
            animator.SetBool("isWalk", false);
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputArrow = 30;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            inputArrow = 40;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            inputArrow = 10;
        }
        else
        {
            inputArrow = 0;
        }
        
        // 캐릭터 뒤집기
        if (!notMove)
        {
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
        }
        
        // 대쉬 딜레이
        RunCheck();
        if (runDelay > 0)
        {
            runDelay -= Time.deltaTime;
            if (runDelay <= 0 && (isRrun < 2 && isLrun < 2))
            {
                isRrun = 0;
                isLrun = 0;
            }
        }

        // 회피
        if (dodgeCount > 0)
            dodgeCount -= Time.deltaTime;
        if (Input.GetButtonDown("Fire3") && dodgeCount <= 0) isDodge = true;

        // 무적 시간
        if(isDamagableCount > 0)
        {
            isDamagableCount -= Time.deltaTime;
            if (isDamagableCount <= 0)
                isDamagable = false;
        }

        // 공격
        if (!dodging)
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
                    if (attackPattern != 0)
                    {
                        inputAttack.Clear();
                    }
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

            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    notMove = true;
                    animator.SetBool("isJump_x_Atk", true);
                }
            }
        }
        
        Jump();

        if (jumping && rb.velocity.y < -0.2f && !isDownJump && !isCollision)
        {
            GroundCheck.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            pStat.SetBuff(1);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if ((currentJumpCount < 1 && jumping) || notMove || inputY || dodging)
                return;

            rb.velocity = Vector2.zero;

            isGround = false;

            if (!jumping && !dodging)
                animator.SetTrigger("isJumpTrigger");
            animator.SetBool("isJump", true);
            GroundCheck.SetActive(false);

            isDownJump = false;
            jumping = true;
            rb.AddForce(new Vector2(0f, pStat.jumpPower), ForceMode2D.Impulse);

            --currentJumpCount;
        }
    }

    IEnumerator DodgeIgnore(float time)
    {
        yield return new WaitForSeconds(time);

        GroundCheck.SetActive(true);
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
        if (!notMove)
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
    }

    private void FixedUpdate()
    {
        Move();
        Run();
        Dodge();
    }

    void Move()
    {
        if (!notMove && !dodging)
        {
            rb.velocity = new Vector2(inputDirection * pStat.moveSpeed, rb.velocity.y);
        }
    }

    void Run()
    {
        if (!notMove)
        {
            if (inputDirection > 0 && isRrun > 1)
            {
                animator.SetBool("isRun", true);
                rb.velocity = new Vector2(inputDirection * (pStat.moveSpeed + 3f), rb.velocity.y);
            }
            else if (inputDirection < 0 && isLrun > 1)
            {
                animator.SetBool("isRun", true);
                rb.velocity = new Vector2(inputDirection * (pStat.moveSpeed + 3f), rb.velocity.y);
            }
        }
    }

    void Dodge()
    {
        if (isDodge && !dodging)
        {
            dodging = true;
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

    public void Hit(int monsterAtk)
    {
        if (isDamagable)
            return;

        notMove = true;
        isDamagable = true;
        isDamagableCount = 1f;

        Debug.Log("Hit");

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
            if (Mathf.Abs(rb.velocity.x) > 0)
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
