using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_InGame : MonoBehaviour
{
    public static Menu_InGame instance;
    public GameObject[] Menus;
    public GameObject _Player;

    public GameObject CancelMenu;
    public GameObject SettingsMenu;
    public GameObject KeySettingMenu;

    public Scrollbar[] sb;

    public bool InventoryOn;
    public bool CancelOn;
    public bool upgradeOn;

    int Focused = 0;
    int count = 0;

    public void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InventoryOn = false;
        CancelOn = false;
        upgradeOn = false;
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
                    CloseInGameMenu();
                }
            }
            if (InventoryOn && !upgradeOn)
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
        _Player.GetComponent<PlayerControl>().enabled = false;
        Focused = 0;
        Menus[Focused].SetActive(true);
        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.0f;
            bar.value = 1.0f;
        }
    }
    public void CloseInGameMenu()
    {
        Menus[Focused].SetActive(false);
        _Player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OpenInventory()
    {
        upgradeOn = true;
        Focused = 0;
        Menus[Focused].SetActive(true);
        Menus[Focused].GetComponent<Menu_Inventory>().OpenInventory();
        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.0f;
            bar.value = 1.0f;
        }
    }
    public void CloseInventory()
    {
        upgradeOn = false;
        Menus[Focused].SetActive(false);
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
        _Player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
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
