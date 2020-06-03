using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI_SettingMenu : FocusUI
{
    public GameObject[] SettingMenus;
    public SystemUI_CancelMenu sc;

    public GameObject keySettingMenu;
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
                        break;
                    }
                case 1:
                    {
                        break;
                    }
                case 2:
                    {
                        OpenKeySettingMenu();
                        break;
                    }
            }
        }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
        {
            CloseSettingMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettingMenu();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(1); }
        FocusMove(SettingMenus[focused]);
    }

    public void OpenKeySettingMenu()
    {
        anyUIOpen = true;
        keySettingMenu.SetActive(true);
        keySettingMenu.GetComponent<SystemUI_KeySettingMenu>().OpenKeySettingMenu(this);
    }

    public void CloseKeySettingMenu()
    {
        keySettingMenu.SetActive(false);
        anyUIOpen = false;
    }
    
    public void OpenSettingMenu()
    {
        focused = 0;
        cursor.SetActive(true);
        cursor.transform.position = new Vector2(cursor.transform.position.x, SettingMenus[focused].transform.position.y);
        isUIOn = true;
    }
    
    public void CloseSettingMenu()
    {
        isUIOn = false;
        cursor.SetActive(false);
        sc.CloseSettingMenu();
        CanvasManager.instance.isSettingOn = false;
    }
}
