using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (OpenUICheck()) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            DungeonManager.instance.ActiveInteractiveNPC(this);
        }
    }
    public override void OpenNPCUI()
    {
        canvasManager.OpenShopInventory();
    }
}
