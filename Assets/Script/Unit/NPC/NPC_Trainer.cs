using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trainer : NPC_Control
{
    public GameObject traningUI;
    public bool openTraningUI;

    // Update is called once per frame
    void Update()
    {
        if (!inPlayer) return;
        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
        if (menu.GameMenuOnCheck()) return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (openTraningUI) return;

            OpenTraningMenu();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (openTraningUI)
            {
                CloseTraningMenu();
                openTraningUI = false;
            }
        }
    }

    public void OpenTraningMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        openTraningUI = true;
        traningUI.SetActive(true);
        traningUI.GetComponent<Menu_Traning>().OpenTraningMenu(this);
    }

    public void CloseTraningMenu()
    {
        traningUI.GetComponent<Menu_Traning>().CloseTraningMenu();
        traningUI.SetActive(false);
        openTraningUI = false;
        player.GetComponent<PlayerControl>().enabled = true;
    }
}
