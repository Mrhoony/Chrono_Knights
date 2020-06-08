using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI_CancelMenu : FocusUI
{
    public GameObject cancelMenu;
    public GameObject[] cancelMenus;

    public GameObject settingMenu;
    public bool anyUIOpen;

    private void Update()
    {
        if (!isUIOn) return;
        if (anyUIOpen) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            switch (focused)
            {
                case 0:
                    {
                        CanvasManager.instance.CloseCancelMenu();
                        break;
                    }
                case 1:
                    {
                        OpenSettingMenu();
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        break;
                    }
            }
        }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
        {
            CloseCancelMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCancelMenu();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(1); }
        FocusMove(cancelMenus[focused]);
    }

    public void OpenSettingMenu()
    {
        anyUIOpen = true;
        settingMenu.SetActive(true);
        settingMenu.GetComponent<SystemUI_SettingMenu>().OpenSettingMenu();
    }

    public void CloseSettingMenu()
    {
        settingMenu.SetActive(false);
        anyUIOpen = false;
    }

    public void OpenCancelMenu()
    {
        focused = 0;
        cancelMenu.SetActive(true);
        cursor.SetActive(true);
        cursor.transform.position = new Vector2(cursor.transform.position.x, cancelMenus[focused].transform.position.y);
        isUIOn = true;
    }
    public void CloseCancelMenu()
    {
        isUIOn = false;
        cursor.SetActive(false);
    }
}
