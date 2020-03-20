using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (OpenedUICheck()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            canvasManager.OpenShopInventory();
        }
    }
}
