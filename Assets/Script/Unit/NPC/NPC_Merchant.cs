using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (!menu.isCancelOn && !menu.isInventoryOn)
        {
            if (inPlayer)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                }
            }
        }
    }
}
