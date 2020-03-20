using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Storage : NPC_Control
{
    private void Update()
    {
        if (!inPlayer) return;
        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
        if (canvasManager.GameMenuOnCheck()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            canvasManager.OpenStorage();
        }
    }
}
