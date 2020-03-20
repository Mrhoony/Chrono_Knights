using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trainer : NPC_Control
{
    // Update is called once per frame
    void Update()
    {
        if (OpenedUICheck()) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            townUI.OpenTrainingMenu();
        }
    }

}
