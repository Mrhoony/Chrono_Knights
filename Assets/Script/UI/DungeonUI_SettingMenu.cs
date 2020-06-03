using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUI_SettingMenu : FocusUI
{
    public GameObject[] SettingMenuSlot;

    private void Update()
    {
        if (!isUIOn) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (focused == 0)
            {
                DungeonManager.instance.isReturn = true;
                CanvasManager.instance.CloseDungeonMenu();
                CanvasManager.instance.CircleFadeOutStart();
            }
            else
            {
                Application.Quit();
            }
        }
        FocusMove(SettingMenuSlot[focused]);
    }
    public void SetActiveSettingMenu()
    {
        focused = 0;
        cursor.SetActive(true);
        isUIOn = true;
    }
    public void SetDisActiveSettingMenu()
    {
        isUIOn = false;
        cursor.SetActive(false);
    }
}
