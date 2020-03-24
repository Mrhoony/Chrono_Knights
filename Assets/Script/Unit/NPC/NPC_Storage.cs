using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Storage : NPC_Control
{
    private void Update()
    {
        if (OpenStorageCheck()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            canvasManager.OpenStorage();
        }
    }

    public bool OpenStorageCheck()
    {
        if (!inPlayer) return true;
        if (player.GetComponent<PlayerControl>().GetActionState() != ActionState.Idle)
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
            return true;
        }
        player.GetComponent<PlayerControl>().playerInputKey.SetActive(true);
        if (canvasManager.GameMenuOnCheck()) return true;
        return false;
    }
}
