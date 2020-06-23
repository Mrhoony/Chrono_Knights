using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainUICursor;
    public GameObject saveUICursor;
    public float cursorSpd;

    public KeyBindManager keyBindManager;
    public CameraManager cameraManager;
    
    public GameObject player;
    public PlayerStatus playerStat;
    public GameObject playerStatView;
    public GameObject bedBlind;

    #region save, load
    public DungeonManager dungeonManager;
    public CanvasManager canvasManager;
    public Menu_Storage storage;
    public Menu_Inventory inventory;

    public GameObject mainMenu;
    public GameObject startMenu;
    public GameObject[] startMenus;
    public GameObject saveSlot;
    public GameObject[] saveSlots;
    public GameObject deleteSave;
    public DataBase dataBase;

    BinaryFormatter bf;
    MemoryStream ms;
    public int gameSlotFocus;
    public int saveSlotFocus;
    public int deleteSlotFocus;
    public string data;
    
    public bool openSaveSlot;
    public bool openSaveDeleteSelect;
    public bool gameStart;
    #endregion
    
    public SystemData systemData;
    private int screenNumber;
    
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
        CanvasManager.instance.DebugText("gameManager awake start");

        Physics2D.IgnoreLayerCollision(5, 10);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(8, 14);
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 13);
        Physics2D.IgnoreLayerCollision(10, 14);
        Physics2D.IgnoreLayerCollision(13, 13);
        Physics2D.IgnoreLayerCollision(13, 14);
        Physics2D.IgnoreLayerCollision(14, 14);
        
        //QualitySettings.vSyncCount = 0;                 // 동기화 수치 고정
        Application.targetFrameRate = 60;               // 최대 프레임 조절
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        dataBase = new DataBase();

        Init();

        CanvasManager.instance.DebugText("gameManager awake");
    }
    private void Init()
    {
        LoadSystemData();
        canvasManager.CanvasManagerInit();
        CanvasManager.instance.DebugText("canvasmanager awake");
        dungeonManager.DungeonManagerInit();
        CanvasManager.instance.DebugText("dungeonmanager awake");

        dataBase.Init();
        storage.Init();
        inventory.Init();

        gameStart = false;
        saveSlotFocus = 0;
        gameSlotFocus = 0;
       
        Debug.Log("gameManagerInit");

        canvasManager.FadeInStart();
        player.GetComponent<PlayerControl>().enabled = false;
        OpenStartMenu();
    }

    public void Update()
    {
        if (gameStart) return;
        if (CanvasManager.instance.isCancelOn || CanvasManager.instance.isSettingOn) return;

        if (openSaveSlot)
        {
            if (openSaveDeleteSelect)
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    DeleteSave();
                    CloseSaveDelete();
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
                {
                    CloseSaveDelete();
                    canvasManager.inGameMenu.SetActive(true);
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { deleteSlotFocus = FocusedSlot(deleteSlotFocus, 1, 1); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { deleteSlotFocus = FocusedSlot(deleteSlotFocus, -1, 1); }
            }
            else
            {
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
                {
                    LoadGame();
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
                {
                    CloseLoad();
                    canvasManager.inGameMenu.SetActive(true);
                }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Jump"]))
                {
                    OpenSaveDelete();
                }

                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { saveSlotFocus = FocusedSlot(saveSlotFocus, 1, 2); }
                if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { saveSlotFocus = FocusedSlot(saveSlotFocus, -1, 2); }
                SaveCursorMove();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
            {
                switch (gameSlotFocus)
                {
                    case 0:
                        OpenLoad();
                        break;
                    case 1:
                        canvasManager.OpenSettings();
                        break;
                    case 2:
                        ExitGame();
                        break;
                }
            }

            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { gameSlotFocus = FocusedSlot(gameSlotFocus, - 1, 2); }
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { gameSlotFocus  = FocusedSlot(gameSlotFocus, 1, 2); }
            MainCursorMove();
        }
    }

    public void SleepGame()
    {
        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
        canvasManager.PlayerMoveStop();
        SaveGame();         // 게임을 세이브
    }

    void SaveCursorMove()
    {
        saveUICursor.transform.position = Vector2.Lerp(saveUICursor.transform.position,
            new Vector2(saveSlots[saveSlotFocus].transform.position.x, saveUICursor.transform.position.y), Time.deltaTime * cursorSpd);
    }
    void MainCursorMove()
    {
        mainUICursor.transform.position = Vector2.Lerp(mainUICursor.transform.position,
           new Vector2(mainUICursor.transform.position.x, startMenus[gameSlotFocus].transform.position.y), Time.deltaTime * cursorSpd);
    }
    int FocusedSlot(int _Focus, int AdjustValue, int _MaxFocused)
    {
        if (_Focus + AdjustValue < 0) _Focus = _MaxFocused;
        else if (_Focus + AdjustValue > _MaxFocused) _Focus = 0;
        else _Focus += AdjustValue;

        return _Focus;
    }

    public void OpenSaveDelete()
    {
        deleteSlotFocus = 0;
        deleteSave.SetActive(true);
        openSaveDeleteSelect = true;
    }
    public void CloseSaveDelete()
    {
        deleteSave.SetActive(false);
        openSaveDeleteSelect = false;
    }

    public void SaveGame()
    {
        gameStart = false;

        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 유저 정보
        dataBase.playerData = playerStat.playerData;
        dataBase.SaveGameData(dungeonManager.GetCurrentDate(), dungeonManager.GetTrainigPossible(), dungeonManager.GetEventFlag(), dungeonManager.GetStoryProgress());
        dataBase.SaveStorageData(storage.GetStorageItemCodeList());
        dataBase.SaveInventoryData(inventory.GetCurrentMoney());
        
        bf.Serialize(ms, dataBase);
        data = Convert.ToBase64String(ms.GetBuffer());
        PlayerPrefs.SetString("SaveSlot" + saveSlotFocus.ToString(), data);

        storage.SaveStorageClear(); // 창고 초기화
        dataBase.Init();            // 플레이어 스테이터스 초기화
        Database_Game.instance.skillManager.SkillListInit();    // 스킬 리스트 초기화

        player.GetComponent<PlayerControl>().enabled = false;
        canvasManager.CanvasManagerInit();
        OpenStartMenu();

        CameraManager.instance.currentMap.GetComponent<Map_ObjectSetting>().SaveGame();

        Debug.Log("save");
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveSlot" + saveSlotFocus.ToString()))
        {
            data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));
                
                dataBase = (DataBase)bf.Deserialize(ms);         // 유저 정보
            }
        }
        else
        {
            dataBase.Init();
        }
        
        playerStat.SetPlayerData(dataBase.playerData);
        dungeonManager.LoadGamePlayDate(dataBase.GetCurrentDate(), dataBase.GetTrainingPossible(), dataBase.GetEventFlag(), dataBase.GetStoryProgress());
        storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.playerData.playerEquipment.equipment[(int)EquipmentType.Bag].itemRarity);
        inventory.LoadInventoryData(dataBase.playerData.playerEquipment.equipment[(int)EquipmentType.Bag].itemRarity, dataBase.GetCurrentMoney());
        
        CloseLoad();
        CloseStartMenu();

        canvasManager.inGameMenu.SetActive(true);
        canvasManager.hpBarSet.SetActive(true);

        CameraManager.instance.currentMap.GetComponent<Map_ObjectSetting>().LoadGame();

        player.GetComponent<PlayerControl>().SetCurrentJumpCount();
        player.GetComponent<PlayerControl>().enabled = true;

        StartCoroutine(GameStartDelay(true));

        Debug.Log("load");
    }
    public void DeleteSave()
    {
        if(PlayerPrefs.HasKey("SaveSlot" + saveSlotFocus.ToString()))
        {
            PlayerPrefs.DeleteKey("SaveSlot" + saveSlotFocus.ToString());
            LoadSlotInformation(saveSlotFocus);
            data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);
            Debug.Log("saveDelete");
        }
    }
    
    public IEnumerator GameStartDelay(bool _GameStart)
    {
        yield return new WaitForSeconds(0.2f);
        gameStart = _GameStart;
    }

    public void OpenStartMenu()
    {
        mainMenu.SetActive(true);
        CloseLoad();
        gameSlotFocus = 0;
        mainUICursor.transform.position = new Vector2(mainUICursor.transform.position.x, startMenus[gameSlotFocus].transform.position.y);
        mainUICursor.SetActive(true);
    }
    public void CloseStartMenu()
    {
        startMenu.SetActive(false);
        mainUICursor.SetActive(false);
        mainMenu.SetActive(false);
    }
    public void OpenLoad()
    {
        openSaveSlot = true;
        startMenu.SetActive(false);
        saveSlot.SetActive(true);
        saveSlotFocus = 0;
        saveUICursor.transform.position = new Vector2(saveSlots[saveSlotFocus].transform.position.x, saveUICursor.transform.position.y);
        saveUICursor.SetActive(true);

        for (int i = 0; i < 3; ++i)
        {
            LoadSlotInformation(i);
        }
    }
    public void LoadSlotInformation(int i)
    {
        if (PlayerPrefs.HasKey("SaveSlot" + i.ToString()))
        {
            Debug.Log("hasData");
            data = PlayerPrefs.GetString("SaveSlot" + i.ToString(), null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                dataBase = (DataBase)bf.Deserialize(ms);
                saveSlots[i].transform.GetChild(0).GetComponent<Text>().text = dataBase.GetCurrentDate().ToString() + " 일";
            }
        }
        else
        {
            Debug.Log("NoData");
            saveSlots[i].transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }
    public void CloseLoad()
    {
        saveUICursor.SetActive(false);
        saveSlot.SetActive(false);
        startMenu.SetActive(true);
        openSaveSlot = false;
    }

    public void SaveSystemData()
    {
        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 시스템 정보

        bf.Serialize(ms, systemData);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("SystemData", data);

        systemData = new SystemData();
        Debug.Log("save system data complete");
    }
    public void LoadSystemData()
    {
        if (PlayerPrefs.HasKey("SystemData"))
        {
            data = PlayerPrefs.GetString("SystemData", null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                systemData = (SystemData)bf.Deserialize(ms);

                cameraManager.Init(systemData.cameraShakeOnOff, systemData.screenSizeNumber);
                keyBindManager.Init(systemData.currentDictionary);
            }
        }
        else
        {
            cameraManager.Init();
            keyBindManager.Init();
        }
        Debug.Log("load system data complete");
    }
    public void ScreenSizeSelect(bool LR)
    {
        if (LR)
        {
            ++screenNumber;
            Screen.fullScreen = !Screen.fullScreen;
        }
        else
        {
            --screenNumber;
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public bool GetGameStart()
    {
        return gameStart;
    }

    // MainMenu Scene 나오게 설정
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void FirstLoad()
    {
        if (SceneManager.GetActiveScene().name.CompareTo("MainMenu") != 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}