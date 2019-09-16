using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject parentObject;
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
            if (collision.gameObject.tag == "Ground")
            {
                if (parentObject.tag == "Player")
                {
                    if (parentObject.GetComponent<PlayerControl>().isDodge)
                    {
                        parentObject.GetComponent<PlayerControl>().isDodge = false;
                        parentObject.GetComponent<PlayerControl>().animator.SetTrigger("isLanding");
                    }
                    else
                    {
                        parentObject.GetComponent<PlayerControl>().isGround = true;
                        parentObject.GetComponent<PlayerControl>().jumping = false;
                        parentObject.GetComponent<PlayerControl>().animator.SetBool("isJump", false);
                        parentObject.GetComponent<PlayerControl>().animator.SetBool("isJump_x_Atk", false);
                        parentObject.GetComponent<PlayerControl>().animator.SetTrigger("isLanding");
                        parentObject.GetComponent<PlayerControl>().currentJumpCount = parentObject.GetComponent<PlayerControl>().jumpCount;
                    }
                }
                else if (parentObject.tag == "Monster")
                {
                    parentObject.GetComponent<MonsterControl>().animator.SetBool("isJumping", false);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Ground")
            {
                if (parentObject.tag == "Player")
                {
                    parentObject.GetComponent<PlayerControl>().isGround = true;
                    parentObject.GetComponent<PlayerControl>().jumping = false;
                    parentObject.GetComponent<PlayerControl>().currentJumpCount = parentObject.GetComponent<PlayerControl>().jumpCount;
                }
                else if (parentObject.tag == "Monster")
                {
                    parentObject.GetComponent<MonsterControl>().animator.SetBool("isJumping", false);
                }
            }
        }
    }
}
