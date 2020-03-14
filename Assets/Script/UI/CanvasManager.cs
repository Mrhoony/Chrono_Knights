using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum content
{
    DungeonKey = 1,
    Enchant = 2,
    Upgrade = 3,
    Buff = 4
}

public class CanvasManager : MonoBehaviour
{
    #region 오브젝트 등록
    public static CanvasManager instance;
    public GameManager gm;

    public GameObject[] Menus;      // UI 메뉴들
    public GameObject storage;
    public GameObject townUI;       // 마을 UI

    public GameObject inGameMenu;
    public GameObject playerStatusInfo;
    public GameObject CancelMenu;
    public GameObject SettingsMenu;
    public GameObject KeySettingMenu;
    public Scrollbar[] sb;
    public GameObject fadeInOut;
    public bool isFadeInOut;
    #endregion

    public bool isInventoryOn;
    public bool isStorageOn;
    public bool isCancelOn;
    public bool isDungeonCancelOn;
    public bool isLoadSlotOn;
    private int useContent;
    private int focus;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        isInventoryOn = false;
        isStorageOn = false;
        isCancelOn = false;
        for (int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
            storage.SetActive(false);
        }
        isFadeInOut = false;
    }

    private void Update()
    {
        if (!gm.GetGameStart()) return;

        //인게임 세팅 관련 ( 사운드, 화면 크기 등)
        if (Input.GetButtonDown("Cancel"))
        {
            if (isInventoryOn)
            {
                CloseInGameMenu();
                isInventoryOn = false;
            }
            else if (isStorageOn)
            {
                CloseStorage();
                isStorageOn = false;
            }
            else if(isCancelOn)
            {
                CloseCancelMenu();
                isCancelOn = false;
            }
            else if(!isCancelOn && !DungeonManager.instance.inDungeon)
            {
                PlayerControl.instance.StopPlayer();
                isCancelOn = true;
                OpenCancelMenu();
            }
            else if (isDungeonCancelOn)
            {
                //CloseDungeonMenu();
                isDungeonCancelOn = false;
            }
            else if(!isDungeonCancelOn && DungeonManager.instance.inDungeon)
            {
                isDungeonCancelOn = true;
                //OpenDungeonMenu();
                DungeonManager.instance.PlayerIsDead();
            }
        }

        if (isLoadSlotOn || isCancelOn) return;

        // 인벤토리, 업적창, 스토리 관련
        if (!isStorageOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!isInventoryOn)
                {
                    isInventoryOn = true;
                    OpenInGameMenu(false);
                }
                else
                {
                    CloseInGameMenu();
                    isInventoryOn = false;
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

    public bool GameMenuOnCheck()
    {
        if (isCancelOn || isInventoryOn || isStorageOn) return true;
        else return false;
    }
    
    public void FadeInStart()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);

        if (GameManager.instance.GetGameStart())
            PlayerControl.instance.enabled = true;

        Color fadeColor = fadeInOut.GetComponent<Image>().color;
        while (fadeColor.a > 0f)
        {
            fadeColor.a -= Time.deltaTime / 1f;
            fadeInOut.GetComponent<Image>().color = fadeColor;
            yield return null;
        }
        if (fadeColor.a < 0f) fadeColor.a = 0f;
        fadeInOut.GetComponent<Image>().color = fadeColor;

        fadeInOut.SetActive(false);
        DungeonManager.instance.isSceneLoading = false;
    }
    public void FadeOutStart(bool sceneLoad)
    {
        StartCoroutine(FadeOut(sceneLoad));
    }
    IEnumerator FadeOut(bool sceneLoad)
    {
        PlayerControl.instance.enabled = false;
        fadeInOut.SetActive(true);
        
        Color fadeColor = fadeInOut.GetComponent<Image>().color;
        while (fadeColor.a < 1f)
        {
            fadeColor.a += Time.deltaTime / 1f;
            fadeInOut.GetComponent<Image>().color = fadeColor;
            yield return null;
        }
        if (fadeColor.a > 1f) fadeColor.a = 1f;
        fadeInOut.GetComponent<Image>().color = fadeColor;
        
        if (sceneLoad)
            DungeonManager.instance.SceneLoad();
        else
        {
            DungeonManager.instance.FloorSetting();
            FadeInStart();
        }
    }

    public void OpenInGameMenu(bool _useItemInDungeon)        // I로 인벤토리 열 때
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        isInventoryOn = true;
        focus = 0;

        playerStatusInfo.SetActive(true);
        Menus[0].SetActive(true);
        Menus[0].GetComponent<Menu_Inventory>().OpenInventory(_useItemInDungeon);
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().OnStatusMenu();

        foreach (Scrollbar bar in sb)
        {
            bar.size = 0.1f;
            bar.value = 1;
        }
    }

    public void CloseInGameMenu()       // I로 인벤토리 닫을 때
    {
        Menus[0].GetComponent<Menu_Inventory>().CloseInventory();
        Menus[focus].SetActive(false);
        playerStatusInfo.SetActive(false);
        isInventoryOn = false;
        PlayerControl.instance.enabled = true;
    }

    public void SetTownUI()
    {
        townUI = GameObject.Find("TownUI");
    }

    // 강화 창에서 창고 열 경우
    public void OpenUpgradeStorage(int used)
    {
        isStorageOn = true;
        useContent = used;
        storage.SetActive(true);
        storage.GetComponent<Menu_Storage>().OpenStorageWithUpgrade();
    }
    public void CloseUpgradeStorage(int focused)
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
        isStorageOn = false;
        storage.SetActive(false);
    }
    public void CloseUpgradeStorage()
    {
        isStorageOn = false;
        storage.SetActive(false);
    }

    // 일반적으로 창고를 열 경우
    public void OpenStorage()
    {
        if (isCancelOn) return;
        isStorageOn = true;

        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        storage.SetActive(true);
        storage.GetComponent<Menu_Storage>().OpenStorage();
    }
    public void CloseStorage()
    {
        storage.SetActive(false);
        PlayerControl.instance.enabled = true;
        isStorageOn = false;
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
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        CancelMenu.SetActive(true);
    }
    public void CloseCancelMenu()
    {
        CancelMenu.SetActive(false);
        PlayerControl.instance.enabled = true;
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
