using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (!MainUI_InGameMenu.cancelOn && !MainUI_InGameMenu.InventoryOn)
        {
            if (inPlayer)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                }

                if (Input.GetButtonDown("Fire2"))
                {
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }
}
