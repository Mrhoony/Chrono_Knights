using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Storage : NPC_Control
{
    public bool onStorage;

    private void Awake()
    {
        base.Awake();
        onStorage = false;
    }

    private void Update()
    {
        if (!inPlayer) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
            menu = GameObject.Find("UI").GetComponent<CanvasManager>();
            if (menu.isInventoryOn || menu.isCancelOn) return;

            if (!onStorage)
            {
                PlayerControl.instance.StopPlayer();
                onStorage = true;
                menu.OpenStorage();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onStorage)
            {
                onStorage = false;
            }
        }
    }
}
