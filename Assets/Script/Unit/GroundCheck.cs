﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private GameObject parentObject;
    private PlayerControl playerControl;
    private Monster_Control monsterControl;
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
                playerControl = parentObject.GetComponent<PlayerControl>();

                if (playerControl.isDodge)
                {
                    playerControl.isDodge = false;
                    playerControl.animator.SetTrigger("isLanding");
                }
                else
                {
                    playerControl.isGround = true;
                }
            }
            else if (parentObject.CompareTag("Monster"))
            {
                monsterControl = parentObject.GetComponent<Monster_Control>();
                monsterControl.animator.SetBool("isJumping", false);
            }
        }
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
