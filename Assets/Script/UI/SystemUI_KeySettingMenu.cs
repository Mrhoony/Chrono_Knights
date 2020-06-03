using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI_KeySettingMenu : FocusUI
{
    public KeyBindManager keyBindManager;
    public SystemUI_SettingMenu ss;
    public GameObject[] keyList;
    public bool isKeySetting;

    private void Update()
    {
        if (!isUIOn) return;
        if (isKeySetting) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(focused < 13)
            {
                Debug.Log("SettingWait");
                isKeySetting = true;
                keyBindManager.KeyBindSelect(((KeyBindsName)focused).ToString(), this);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseKeySettingMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeySettingMenu();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(1); }
        FocusMove(keyList[focused]);
    }

    public void SettingKeyList()
    {
        for (int i = 0; i < keyList.Length; ++i)
        {
            if (keyBindManager.KeyBinds.ContainsKey(((KeyBindsName)i).ToString()))
            {
                keyList[i].transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = keyBindManager.KeyBinds[((KeyBindsName)i).ToString()].ToString();
                Debug.Log("has key");
            }
            else
            {
                Debug.Log("has not key");
            }
        }
    }

    public void KeySettingEnd()
    {
        keyList[focused].transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = keyBindManager.KeyBinds[((KeyBindsName)focused).ToString()].ToString();
        
        Invoke("KeyInputDelay", 0.2f);
    }

    public void KeyInputDelay()
    {
        isKeySetting = false;
    }

    public void OpenKeySettingMenu(SystemUI_SettingMenu _ss)
    {
        keyBindManager = KeyBindManager.instance;
        ss = _ss;
        focused = 0;
        SettingKeyList();
        cursor.SetActive(true);
        cursor.transform.position = new Vector2(cursor.transform.position.x, keyList[focused].transform.position.y);
        isUIOn = true;
    }
    public void CloseKeySettingMenu()
    {
        isUIOn = false;
        cursor.SetActive(false);
        ss.CloseKeySettingMenu();
    }
}
