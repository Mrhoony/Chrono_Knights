using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Storage : NPC_Control
{
    private void Update()
    {
        if (!inPlayer) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
            if (menu.GameMenuOnCheck()) return;

            menu.OpenStorage();
        }
    }
}
