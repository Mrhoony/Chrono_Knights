using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum content
{
    Shop = 0,
    Training = 1,
    Enchant = 2,
    Upgrade = 3,
}

public class CanvasManager : MonoBehaviour
{
    #region 오브젝트 등록
    public static CanvasManager instance;
    public GameManager gm;

    public GameObject hpBarSet;
    public GameObject[] Menus;      // UI 메뉴들
    public GameObject storage;
    public GameObject inGameMenu;
    public GameObject playerStatusInfo;
    public GameObject CancelMenu;
    public GameObject SettingsMenu;
    public GameObject KeySettingMenu;
    public GameObject RootBag;

    public GameObject fadeInOut;
    public GameObject circleFadeOut;

    public GameObject talkBoxNPC;
    public TalkBox talkBox;
    public ChatDialog dialogBox;

    public GameObject dungeonCancleMenu;
    public GameObject gameOverWindow;

    public Text debugText;

    #endregion

    public TownUI townUI;       // 마을 UI
    public DungeonUI dungeonUI;

    #region UIOpenCheck
    public bool isInventoryOn;
    public bool isStorageOn;
    public bool isCancelOn;

    public bool isSettingOn;

    public bool isChatBoxOn;
    public bool isTalkBoxOn;

    public bool isShopOn;
    public bool isTrainigOn;
    public bool isEnchantOn;
    public bool isUpgradeOn;

    public bool isDungeonUIOn;
    public bool isGameOverUIOn;
    public bool isTrialCardSelectOn;
    #endregion

    public delegate void FadeInStartMethod();
    public FadeInStartMethod fadeInStartMethod;

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

    public void CanvasManagerInit()
    {
        isChatBoxOn = false;
        isTalkBoxOn = false;

        isInventoryOn = false;
        isStorageOn = false;
        isCancelOn = false;

        isShopOn = false;
        isTrainigOn = false;
        isEnchantOn = false;
        isUpgradeOn = false;

        isDungeonUIOn = false;

        hpBarSet.SetActive(false);
        for (int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
        }
        inGameMenu.SetActive(false);
        storage.SetActive(false);
    }

    private void Update()
    {
        if (!gm.GetGameStart()) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (isGameOverUIOn)
            {
                CloseGameOverMenu();
            }
        }
        if (isTalkBoxOn)
        {
            talkBox.gameObject.transform.position = Camera.main.WorldToScreenPoint(talkBoxNPC.transform.position) + Vector3.up * 100f;
        }

        if (DungeonManager.instance.isSceneLoading) return;

        //인게임 세팅 관련 ( 사운드, 화면 크기 등)
        if (Input.GetButtonDown("Cancel"))              // esc 를 눌렀을 때
        {
            if (isGameOverUIOn)
            {
                CloseGameOverMenu();
                return;
            }

            if (DungeonManager.instance.inDungeon)          // 던전 안에 있을 때
            {
                if (!isDungeonUIOn)        // 던전 메뉴가 꺼져있고 던전 내부일 경우 던전 메뉴 온
                {
                    OpenDungeonMenu();
                }
                else
                {
                    CloseDungeonMenu();
                }
            }
            else                                            // 던전 밖에 있을 때
            {
                if (TownUIOnCheck())
                {
                    if (isShopOn)
                    {
                        CloseShopInventory();
                    }
                    else if (isTrainigOn)
                    {
                        CloseTrainingMenu();
                    }
                    else if (isEnchantOn)
                    {
                        CloseEnchantMenu();
                    }
                    else if (isUpgradeOn)
                    {
                        CloseUpgradeMenu();
                    }
                }
                else
                {
                    if (isInventoryOn)                          // 인벤토리가 켜져있으면 인벤토리를 오프
                    {
                        CloseInGameMenu();
                    }
                    else if (isStorageOn)                       // 창고가 켜져있으면 창고를 오프
                    {
                        storage.GetComponent<Menu_Storage>().CloseStorage();
                    }
                    else if (isSettingOn)
                    {
                        CloseSettings();
                    }
                    else if (isCancelOn)                        // 메뉴가 켜져있으면 메뉴를 오프
                    {
                        CloseCancelMenu();
                    }
                    else                 // 메뉴창이 꺼져있으면 메뉴를 온
                    {
                        OpenCancelMenu();
                    }
                }
            }
        }
        
        if (TownUIOnCheck() || isDungeonUIOn) return;
        if (isGameOverUIOn) return;

        // 인벤토리, 업적창, 스토리 관련
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Inventory"]))
        {
            if (!isInventoryOn)
            {
                isInventoryOn = true;
                OpenInGameMenu(false);
            }
            else
            {
                isInventoryOn = false;
                CloseInGameMenu();
            }
        }
        if (isInventoryOn)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["QuickSlotLeft"]))
            {
                ChangeMenu(-1);
            }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["QuickSlotRight"]))
            {
                ChangeMenu(1);
            }
        }
    }
    public bool GameMenuOnCheck()
    {
        if (isCancelOn || isInventoryOn || isStorageOn || isChatBoxOn || !GameManager.instance.GetGameStart()) return true;
        else return false;
    }
    public bool TownUIOnCheck()
    {
        if (isShopOn || isTrainigOn || isEnchantOn || isUpgradeOn) return true;
        else return false;
    }

    #region fade in, out
    public void FadeInStart()
    {
        DungeonManager.instance.isSceneLoading = true;
        System.GC.Collect();
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
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
    public void FadeOutStart()
    {
        DungeonManager.instance.isSceneLoading = true;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
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

        if(fadeInStartMethod != null)
        {
            fadeInStartMethod();
            fadeInStartMethod = null;
        }

        Debug.Log("FadeOutEnd");
    }
    public void CircleFadeOutStart()
    {
        DungeonManager.instance.isSceneLoading = true;
        StartCoroutine(CircleFadeOut());
    }
    IEnumerator CircleFadeOut()
    {
        PlayerControl.instance.enabled = false;
        circleFadeOut.SetActive(true);
        fadeInOut.SetActive(true);

        circleFadeOut.transform.position = new Vector3(
            Camera.main.WorldToScreenPoint(PlayerControl.instance.gameObject.transform.position).x,
            Camera.main.WorldToScreenPoint(PlayerControl.instance.gameObject.transform.position).y,
            circleFadeOut.transform.position.z);

        Color fadeColor = fadeInOut.GetComponent<Image>().color;
        fadeColor.a = 1f;
        fadeInOut.GetComponent<Image>().color = fadeColor;
        
        Vector3 fadeOutScale = circleFadeOut.transform.localScale;
        while (fadeOutScale.x > 1f)
        {
            fadeOutScale.x -= 1f;
            fadeOutScale.y -= 1f;
            circleFadeOut.transform.localScale = fadeOutScale;

            yield return null;
        }
        circleFadeOut.SetActive(false);
        fadeOutScale.x = 100f;
        fadeOutScale.y = 100f;
        circleFadeOut.transform.localScale = fadeOutScale;
        
        Time.timeScale = 1f;
        DungeonManager.instance.OpenGameOverResult();
    }
    #endregion
    
    public void BossKillSlowMotionMethod()
    {
        StartCoroutine(BossKillSlowMotion());
    }
    IEnumerator BossKillSlowMotion()
    {
        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(2f);

        Time.timeScale = 1f;

        yield return new WaitForSeconds(1f);

        CanvasManager.instance.OpenTrialCardSelectMenu();
    }

    public void SetDialogText(string _DialogName, string _DialogContent, ScenarioManager _ScenearioManager)
    {
        isChatBoxOn = true;
        PlayerMoveStop();
        dialogBox.gameObject.SetActive(true);
        dialogBox.SetDialogText(_DialogName, _DialogContent, _ScenearioManager);
    }
    public void OneByOneTextSkip()
    {
        dialogBox.OneByOneTextSkip();
    }
    public void CloseDialogBox()
    {
        Debug.Log("close dialogbox");
        dialogBox.gameObject.SetActive(false);
        StartCoroutine(InputKeyDelay());
    }
    public bool DialogBoxOn()
    {
        return isChatBoxOn;
    }
    public void SetTalkBoxText(GameObject _NPC, string _Text)
    {
        isTalkBoxOn = true;
        talkBoxNPC = _NPC;
        talkBox.gameObject.SetActive(true);
        talkBox.SetDialogText(_Text);
    }
    public void CloseTalkBox()
    {
        talkBox.gameObject.SetActive(false);
        isTalkBoxOn = false;
    }

    #region 던전 UI
    public void OpenDungeonMenu()
    {
        isDungeonUIOn = true;
        fadeInOut.SetActive(true);

        Time.timeScale = 0f;
        Color fadeColor = fadeInOut.GetComponent<Image>().color;
        fadeColor.a = 0.5f;
        fadeInOut.GetComponent<Image>().color = fadeColor;

        dungeonCancleMenu.SetActive(true);
        dungeonCancleMenu.GetComponent<DungeonUI_SettingMenu>().SetActiveSettingMenu();
    }
    public void CloseDungeonMenu()
    {
        Color fadeColor = fadeInOut.GetComponent<Image>().color;
        fadeColor.a = 0f;
        fadeInOut.GetComponent<Image>().color = fadeColor;
        Time.timeScale = 1f;

        fadeInOut.SetActive(false);
        dungeonCancleMenu.GetComponent<DungeonUI_SettingMenu>().SetDisActiveSettingMenu();
        dungeonCancleMenu.SetActive(false);
        isDungeonUIOn = false;
    }

    public void OpenGameOverMenu(float playTime)
    {
        isDungeonUIOn = true;
        isGameOverUIOn = true;

        int hour = (int)(playTime / 3600f);
        int minute = (int)(playTime % 3600f / 60f);
        float second = playTime % 3600f % 60f;

        gameOverWindow.SetActive(true);
        gameOverWindow.transform.GetChild(0).GetComponent<Text>().text = hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
        gameOverWindow.transform.GetChild(1).GetComponent<Text>().text = "";
        gameOverWindow.transform.GetChild(2).GetComponent<Text>().text = "";
        gameOverWindow.transform.GetChild(3).GetComponent<Text>().text = "";
        gameOverWindow.transform.GetChild(4).GetComponent<Text>().text = "";
        gameOverWindow.transform.GetChild(5).GetComponent<Text>().text = "";
        gameOverWindow.transform.GetChild(6).GetComponent<Text>().text = "";
    }
    public void CloseGameOverMenu()
    {
        isDungeonUIOn = false;
        isGameOverUIOn = false;
        gameOverWindow.SetActive(false);
        if (DungeonManager.instance.isReturn)
        {
            DungeonManager.instance.ReturnToTown();
        }
    }

    public void OpenTrialCardSelectMenu()
    {
        PlayerMoveStop();
        dungeonUI.OpenTrialCardSelectMenu();
    }
    public void CloseTrialCardSelectMenu()
    {
        StartCoroutine(PlayerMoveEnable());
    }
    #endregion

    #region Town UI
    public void OpenShopInventory()
    {
        isShopOn = true;
        PlayerMoveStop();
        townUI.OpenShopMenu(Menus[0].GetComponent<Menu_Inventory>());
        Menus[0].SetActive(true);
        Menus[0].GetComponent<Menu_Inventory>().OpenInventory(townUI.townMenus[0].GetComponent<TownUI_Shop>());
    }
    public void CloseShopInventory()
    {
        townUI.CloseShopMenu();
        Menus[0].GetComponent<Menu_Inventory>().CloseInventory();
        Menus[0].SetActive(false);
        isShopOn = false;
        StartCoroutine(PlayerMoveEnable());
    }

    public void OpenTrainingMenu()
    {
        isTrainigOn = true;
        PlayerMoveStop();
        townUI.OpenTrainingMenu();
    }
    public void CloseTrainingMenu()
    {
        isTrainigOn = false;
        StartCoroutine(PlayerMoveEnable());
    }

    public void OpenEnchantMenu()
    {
        isEnchantOn = true;
        PlayerMoveStop();
        townUI.OpenEnchantMenu();
    }
    public void CloseEnchantMenu()
    {
        isEnchantOn = false;
    }

    public void OpenUpgradeMenu()
    {
        isUpgradeOn = true;
        townUI.OpenUpgradeMenu();
    }
    public void CloseUpgradeMenu()
    {
        isUpgradeOn = false;
    }

    public void OpenUpgradeStorage(int used)
    {
        isStorageOn = true;
        useContent = used;
        storage.SetActive(true);
        storage.GetComponent<Menu_Storage>().OpenStorage(true);
    }
    public void CloseUpgradeStorage(int focused)
    {
        switch (useContent)
        {
            case (int)content.Enchant:
                townUI.GetComponent<TownUI>().townMenus[2].GetComponent<TownUI_Enchant>().SetKey(focused);
                break;
            case (int)content.Upgrade:
                townUI.GetComponent<TownUI>().townMenus[3].GetComponent<TownUI_Upgrade>().SetKey(focused);
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

    #endregion

    #region MainUI
    public void SetPlayerStatusInfo(PlayerEquipment.Equipment[] _Equipment)
    {
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().EquipmentSkillOptionCheck(_Equipment);
    }
    public void OpenInGameMenu(bool _isInDungeon)        // I로 인벤토리 열 때
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;

        isInventoryOn = true;
        focus = 0;

        Menus[0].SetActive(true);
        Menus[0].GetComponent<Menu_Inventory>().OpenInventory(_isInDungeon, playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>());
        playerStatusInfo.SetActive(true);
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().OnStatusMenu(Menus[0].GetComponent<Menu_Inventory>());
    }
    public void CloseInGameMenu()       // I로 인벤토리 닫을 때
    {
        Menus[0].GetComponent<Menu_Inventory>().CloseInventory();
        playerStatusInfo.GetComponent<MainUI_PlayerStatusInfo>().CloseStatusInfo();
        Menus[focus].SetActive(false);
        playerStatusInfo.SetActive(false);
        isInventoryOn = false;

        StartCoroutine(PlayerMoveEnable());
    }

    public void OpenStorage()
    {
        if (isCancelOn) return;
        isStorageOn = true;
        PlayerMoveStop();
        storage.SetActive(true);
        storage.GetComponent<Menu_Storage>().OpenStorage(false);
    }
    public void CloseStorage()
    {
        isStorageOn = false;
        storage.SetActive(false);
        StartCoroutine(PlayerMoveEnable());
    }
    
    public void OpenCancelMenu()
    {
        PlayerMoveStop();
        CancelMenu.SetActive(true);
        CancelMenu.GetComponent<SystemUI_CancelMenu>().OpenCancelMenu();
        isCancelOn = true;
    }
    public void CloseCancelMenu()
    {
        if (CancelMenu.GetComponent<SystemUI_CancelMenu>().anyUIOpen) return;

        isCancelOn = false;
        CancelMenu.GetComponent<SystemUI_CancelMenu>().CloseCancelMenu();
        CancelMenu.SetActive(false);
        StartCoroutine(PlayerMoveEnable());
    }
    
    public void OpenSettings()
    {
        isSettingOn = true;
        SettingsMenu.SetActive(true);
        SettingsMenu.GetComponent<SystemUI_SettingMenu>().OpenSettingMenu();
    }
    public void CloseSettings()
    {
        if (SettingsMenu.GetComponent<SystemUI_SettingMenu>().anyUIOpen) return;

        SettingsMenu.SetActive(false);
        isSettingOn = false;
    }
    #endregion

    public void RootBagUI() {
        if (RootBag.activeInHierarchy) return;
        RootBag.SetActive(true);
    }
    
    public void PlayerMoveStop()
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
    }
    public IEnumerator PlayerMoveEnable()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerControl.instance.enabled = true;
    }
    public IEnumerator InputKeyDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isChatBoxOn = false;
    }

    void ChangeMenu(int AdjustValue)
    {
        if (!(focus + AdjustValue < 0 || focus + AdjustValue >= Menus.Length))
        {
            Menus[focus + AdjustValue].SetActive(true);
            Menus[focus].SetActive(false);
            focus += AdjustValue;
        }
    }
    
    public void SetTownUI()
    {
        townUI = GameObject.Find("TownUI").GetComponent<TownUI>();
    }
    public void SetDungeonUI()
    {
        dungeonUI = GameObject.Find("DungeonUI").GetComponent<DungeonUI>();
    }

    public void DebugText(string _Text)
    {
        debugText.text += _Text + "\r\n";
    }
}
