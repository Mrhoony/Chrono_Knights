﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_InGameMenu : MonoBehaviour
{
    public GameObject[] Menus;
    public GameObject townUI;
    public GameObject _Player;

    public GameObject CancelMenu;
    public GameObject SettingsMenu;
    public GameObject KeySettingMenu;
    public GameObject playerStatusInfo;

    public Scrollbar[] sb;

    public bool InventoryOn;
    public bool storageOn;
    public bool cancelOn;

    enum content
    {
        DungeonKey = 1,
        Enchant = 2,
        Upgrade = 3,
        Buff = 4
    }

    int useContent;
    int Focused;

    private void Start()
    {
        InventoryOn = false;
        cancelOn = false;
        for (int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
        }
    }

    private void Update()
    {
        //인벤토리, 업적창, 스토리 관련
        if (!cancelOn && !storageOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!InventoryOn)
                {
                    InventoryOn = !InventoryOn;
                    OpenInGameMenu();
                }
                else
                {
                    InventoryOn = !InventoryOn;
                    CloseInGameMenu();
                }
            }
            if (InventoryOn)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    ChangeMenu(-1);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    ChangeMenu(1);
                }
            }
        }

        //인게임 세팅 관련 ( 사운드, 화면 크기 등)
        if (Input.GetButtonDown("Cancel"))
        {
            if (!cancelOn)
            {
                cancelOn = !cancelOn;
                OpenCancelMenu();
            }
            else
            {
                cancelOn = !cancelOn;
                CloseCancelMenu();
            }
        }
    }

    public void OpenInGameMenu()        // I로 인벤토리 열 때
    {
        Time.timeScale = 0;
        _Player.GetComponent<PlayerControl>().enabled = false;
        Focused = 0;
        Menus[0].SetActive(true);
        playerStatusInfo.SetActive(true);
        Menus[0].GetComponent<Menu_Inventory>().OpenInventory();
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().OnStatusMenu();
        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.0f;
            bar.value = 1.0f;
        }
    }
    public void CloseInGameMenu()       // I로 인벤토리 닫을 때
    {
        Menus[0].GetComponent<Menu_Inventory>().CloseInventory();
        Menus[Focused].SetActive(false);
        playerStatusInfo.SetActive(false);
        _Player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }
   
    // 강화 창에서 창고 열 경우
    public void OpenUpgradeStorage(int used)
    {
        storageOn = true;
        townUI = GameObject.Find("TownUI");
        useContent = used;
        Menus[3].SetActive(true);
        Menus[3].GetComponent<Menu_Storage>().OpenStorageWithUpgrade();
    }
    public void CloseUpgradeStorage(int focused)
    {
        storageOn = false;
        switch (useContent)
        {
            case (int)content.Enchant:
                townUI.GetComponent<Menu_TownUI>().townMenus[2].GetComponent<Menu_Enchant>().SetKey(focused);
                break;
            case (int)content.Upgrade:
                townUI.GetComponent<Menu_TownUI>().townMenus[3].GetComponent<Menu_Upgrade>().SetKey(focused);
                break;
        }
        Menus[3].SetActive(false);
    }
    
    // 일반적으로 창고를 열 경우
    public void OpenStorage()
    {
        storageOn = true;
        Time.timeScale = 0;
        _Player.GetComponent<PlayerControl>().enabled = false;
        Menus[3].SetActive(true);
        Menus[3].GetComponent<Menu_Storage>().OpenStorage();
    }
    public void CloseStorage()
    {
        storageOn = false;
        Menus[3].SetActive(false);
        _Player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }


    void ChangeMenu(int AdjustValue)
    {
        if (!(Focused + AdjustValue < 0 || Focused + AdjustValue >= Menus.Length))
        {
            Menus[Focused + AdjustValue].SetActive(true);
            Menus[Focused].SetActive(false);
            Focused += AdjustValue;
        }
    }
    
    public void OpenCancelMenu()
    {
        _Player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        CancelMenu.SetActive(true);
    }
    public void CloseCancelMenu()
    {
        CancelMenu.SetActive(false);
        Time.timeScale = 1;
        _Player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OpenSettings()
    {
        SettingsMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(200, 0, 0);
        SettingsMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(800, Screen.height);
        SettingsMenu.SetActive(true);
    }
    public void OpenSettings(int width, int height)
    {
        SettingsMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        SettingsMenu.SetActive(true);
    }
    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
    }

    public void OpenKeySettings()
    {
        KeySettingMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(800, Screen.height);
        KeySettingMenu.SetActive(true);
    }
    public void OpenKeySettings(int width, int height)
    {
        KeySettingMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        KeySettingMenu.SetActive(true);
    }
    public void CloseKeySettings()
    {
        KeySettingMenu.SetActive(false);
    }
}
