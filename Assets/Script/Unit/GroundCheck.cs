using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (parentObject.CompareTag("Player"))
            {
                Debug.Log("Land");
                parentObject.GetComponent<PlayerControl>().Landing();
            }
            else if (parentObject.CompareTag("Monster"))
            {
                parentObject.GetComponent<NormalMonsterControl>().Landing();
            }
        }
        /*
        if (collision.gameObject.CompareTag("Slope"))
        {
            if (parentObject.CompareTag("Player"))
            {
                playerControl = parentObject.GetComponent<PlayerControl>();
                
                playerControl.isSlope = true;
                playerControl.slopeDelay = 0.3f;

                if (playerControl.isDodge)
                {
                    playerControl.isDodge = false;
                    playerControl.animator.SetTrigger("isLanding");
                }
            }
            else if (parentObject.CompareTag("Monster"))
            {
                monsterControl = parentObject.GetComponent<Monster_Control>();
                monsterControl.animator.SetBool("isJumping", false);
            }
        }
        */
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (parentObject.CompareTag("Player"))
            {
                if(!parentObject.GetComponent<PlayerControl>().isGround)
                    parentObject.GetComponent<PlayerControl>().Landing();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (parentObject.CompareTag("Player"))
            {
                parentObject.GetComponent<PlayerControl>().isGround = false;
            }
        }
    }
}
