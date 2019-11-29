using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject parentObject;
    private PlayerControl playerControl;
    private Monster_Control monsterControl;
    private BoxCollider2D box;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        box = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                if (parentObject.CompareTag("Player"))
                {
                    playerControl = parentObject.GetComponent<PlayerControl>();

                    if (playerControl.isDodge)
                    {
                        playerControl.isDodge = false;
                        playerControl.animator.SetTrigger("isLanding");
                    }
                    else
                    {
                        playerControl.jumping = false;
                        playerControl.animator.SetBool("isJump", false);
                        playerControl.animator.SetBool("isJump_x_Atk", false);
                        playerControl.animator.SetTrigger("isLanding");
                        //parentObject.GetComponent<PlayerControl>().currentJumpCount = parentObject.GetComponent<PlayerControl>().currentJumpCount;
                        playerControl.currentJumpCount = 3;
                    }
                }
                else if (parentObject.CompareTag("Monster"))
                {
                    monsterControl = parentObject.GetComponent<Monster_Control>();
                    monsterControl.animator.SetBool("isJumping", false);
                }
            }
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                if (parentObject.CompareTag("Player"))
                {
                    parentObject.GetComponent<PlayerControl>().jumping = false;
                    parentObject.GetComponent<PlayerControl>().currentJumpCount = parentObject.GetComponent<PlayerControl>().currentJumpCount;
                }
                else if (parentObject.CompareTag("Monster"))
                {
                    monsterControl = parentObject.GetComponent<Monster_Control>();
                    monsterControl.animator.SetBool("isJumping", false);
                }
            }
        }
    }
    */
}
