using System.Collections;
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
    public bool inventoryNotChange;
    public bool CancelOn;

    enum content
    {
        DungeonKey = 1,
        Enchant = 2,
        Upgrade = 3,
        Buff = 4
    }

    int useContent;
    int Focused = 0;

    private void Start()
    {
        InventoryOn = false;
        CancelOn = false;
        inventoryNotChange = false;
        for (int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
        }
    }

    private void Update()
    {
        //인벤토리, 업적창, 스토리 관련
        if (!CancelOn)
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
                }
            }
            if (InventoryOn && !inventoryNotChange)
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
            if (!CancelOn)
            {
                CancelOn = !CancelOn;
                OpenCancelMenu();
            }
            else
            {
                CancelOn = !CancelOn;
                CloseCancelMenu();
            }
        }
    }
    public void OpenInGameMenu()
    {
        Time.timeScale = 0;
        _Player.GetComponent<PlayerControl>().enabled = false;
        Focused = 0;
        Menus[Focused].SetActive(true);
        playerStatusInfo.SetActive(true);
        Menus[Focused].GetComponent<Menu_Inventory>().OpenInventory();
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().OnStatusMenu();
        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.0f;
            bar.value = 1.0f;
        }
    }
    public void CloseInGameMenu()
    {
        Menus[Focused].SetActive(false);
        playerStatusInfo.SetActive(false);
        _Player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }

    // 인벤토리
    public void OpenInventory(int used)
    {
        inventoryNotChange = true;
        InventoryOn = true;
        useContent = used;

        Focused = 0;
        Menus[Focused].SetActive(true);
        Menus[Focused].GetComponent<Menu_Inventory>().OpenInventory();

        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.0f;
            bar.value = 1.0f;
        }
    }
    // 강화 창에서 인벤토리 열 경우
    public void OpenUpgradeInventory(int used)
    {
        townUI = GameObject.Find("TownUI");
        OpenInventory(used);
    }

    public void OpenStorage()
    {
        Time.timeScale = 0;
        _Player.GetComponent<PlayerControl>().enabled = false;
        Menus[3].SetActive(true);
        Menus[3].GetComponent<Menu_Storage>().OpenStorage(Menus[0]);
    }

    public void CloseStorage()
    {
        Menus[3].SetActive(false);
        _Player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }

    public void CloseInventory(int focused)
    {
        switch (useContent)
        {
            case (int)content.Enchant:
                townUI.GetComponent<Menu_TownUI>().townMenus[2].GetComponent<Menu_Enchant>().SetKey(focused);
                break;
            case (int)content.Upgrade:
                townUI.GetComponent<Menu_TownUI>().townMenus[3].GetComponent<Menu_Upgrade>().SetKey(focused);
                break;
        }

        InventoryOn = false;
        inventoryNotChange = false;
        Menus[Focused].SetActive(false);
    }
    public void CloseInventory()
    {
        InventoryOn = false;
        inventoryNotChange = false;
        CloseInGameMenu();
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

    public void SetTownMenu()
    {
        townUI = GameObject.Find("TownUI");
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
