using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUI_SettingMenu : MonoBehaviour
{
    public GameObject[] SettingMenuSlot;

    int focused;
    bool isUIOn;

    private void Update()
    {
        if (isUIOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(focused == 0)
                {
                    DungeonManager.instance.isReturn = true;
                    CanvasManager.instance.CircleFadeOutStart();
                }
                CanvasManager.instance.CloseResume();
            }
        }
    }
    public void SetActiveSettingMenu()
    {
        isUIOn = true;
        focused = 0;
        SettingMenuSlot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
    public void SetDisActiveSettingMenu()
    {
        isUIOn = false;
        SettingMenuSlot[focused].transform.GetChild(0).gameObject.SetActive(false);
    }

    void FocusedSlot(int AdjustValue)
    {
        SettingMenuSlot[focused].transform.GetChild(0).gameObject.SetActive(false);

        focused += AdjustValue;
        if (focused < 0) focused = 1;
        else if (focused > 1) focused = 0;

        SettingMenuSlot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
