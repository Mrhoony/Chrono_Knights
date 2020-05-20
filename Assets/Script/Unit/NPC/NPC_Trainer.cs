using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trainer : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (OpenUICheck()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DungeonManager.instance.ActiveInteractiveNPC(this);
        }
    }
    public override void OpenNPCUI()
    {
        canvasManager.OpenTrainingMenu();
    }
}
