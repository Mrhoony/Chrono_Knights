using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUI_SettingMenu : FocusUI
{
    public GameObject[] SettingMenuSlot;

    private void Update()
    {
        if (!isUIOn) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }

        if (Input.GetKeyDown(KeyCode.Z))
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
