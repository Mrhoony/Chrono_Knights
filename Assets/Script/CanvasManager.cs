using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    #region 오브젝트 등록
    public static CanvasManager instance;

    public GameObject[] Menus;      // UI 메뉴들
    public GameObject townUI;       // 마을 UI
    public GameObject player;

    public GameObject inGameMenu;
    public GameObject playerStatusInfo;
    public GameObject CancelMenu;
    public GameObject SettingsMenu;
    public GameObject KeySettingMenu;
    public GameObject LoadSlots;
    public Scrollbar[] sb;
    #endregion

    public bool isInventoryOn;
    public bool isStorageOn;
    public bool isCancelOn;
    public bool isLoadSlotOn;
    private int useContent;
    private int focus;

    private enum content
    {
        DungeonKey = 1,
        Enchant = 2,
        Upgrade = 3,
        Buff = 4
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        isInventoryOn = false;
        isStorageOn = false;
        isCancelOn = false;
        for (int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
        }
    }

    private void Update()
    {
        //인게임 세팅 관련 ( 사운드, 화면 크기 등)
        if (Input.GetButtonDown("Cancel"))
        {
            if (isInventoryOn)
            {
                isInventoryOn = false;
                CloseInGameMenu();
            }
            else if (isStorageOn)
            {
                isStorageOn = false;
                CloseStorage();
            }
            else if(isCancelOn)
            {
                isCancelOn = false;
                OpenCancelMenu();
            }
            else
            {
                isCancelOn = false;
                CloseCancelMenu();
            }
        }

        if (isLoadSlotOn || isCancelOn) return;

        //인벤토리, 업적창, 스토리 관련
        if (!isStorageOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!isInventoryOn)
                {
                    isInventoryOn = true;
                    OpenInGameMenu();
                }
                else
                {
                    isInventoryOn = false;
                    CloseInGameMenu();
                }
            }
            if (isInventoryOn)
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
    }

    public void OpenInGameMenu()        // I로 인벤토리 열 때
    {
        Time.timeScale = 0;
        player.GetComponent<PlayerControl>().enabled = false;
        focus = 0;

        Menus[0].SetActive(true);
        playerStatusInfo.SetActive(true);
        Menus[0].GetComponent<Menu_Inventory>().OpenInventory();
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().OnStatusMenu();

        foreach (Scrollbar bar in sb)
        {
            Debug.Log("barSize");
            bar.size = 0;
            bar.value = 1;
        }
    }
    public void CloseInGameMenu()       // I로 인벤토리 닫을 때
    {
        Menus[0].GetComponent<Menu_Inventory>().CloseInventory();
        Menus[focus].SetActive(false);
        playerStatusInfo.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }

    // 강화 창에서 창고 열 경우
    public void OpenUpgradeStorage(int used)
    {
        isStorageOn = true;
        townUI = GameObject.Find("TownUI");
        useContent = used;
        Menus[3].SetActive(true);
        Menus[3].GetComponent<Menu_Storage>().OpenStorageWithUpgrade();
    }
    public void CloseUpgradeStorage(int focused)
    {
        isStorageOn = false;
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
        if (isCancelOn) return;

        isStorageOn = true;
        Time.timeScale = 0;
        player.GetComponent<PlayerControl>().enabled = false;
        Menus[3].SetActive(true);
        Menus[3].GetComponent<Menu_Storage>().OpenStorage();
    }
    public void CloseStorage()
    {
        isStorageOn = false;
        Menus[3].SetActive(false);
        player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
    }
    
    void ChangeMenu(int AdjustValue)
    {
        if (!(focus + AdjustValue < 0 || focus + AdjustValue >= Menus.Length))
        {
            Menus[focus + AdjustValue].SetActive(true);
            Menus[focus].SetActive(false);
            focus += AdjustValue;

            foreach (Scrollbar bar in sb)
            {
                Debug.Log("barSize");
                bar.size = 0;
                bar.value = 1;
            }
        }
    }

    public void OpenCancelMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        CancelMenu.SetActive(true);
    }
    public void CloseCancelMenu()
    {
        CancelMenu.SetActive(false);
        Time.timeScale = 1;
        player.GetComponent<PlayerControl>().enabled = true;
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

    public bool OpenLoadSlot()
    {
        if (isCancelOn || isInventoryOn || isStorageOn) return false;
        LoadSlots.SetActive(true);
        return true;
    }
    public void CloseLoadSlot()
    {
        LoadSlots.SetActive(false);
    }
}
