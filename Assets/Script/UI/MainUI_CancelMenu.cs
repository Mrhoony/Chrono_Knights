using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI_CancelMenu : FocusUI
{
    public GameObject cancelMenu;
    public GameObject[] cancelMenus;

    private void Update()
    {
        if (!isUIOn) return;

        if (Input.GetKeyDown(KeyCode.Z))
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

        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(1); }
        FocusMove(cancelMenus[focused]);
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
        cancelMenu.SetActive(false);
    }
}
