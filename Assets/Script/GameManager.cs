using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainUICursor;
    public GameObject saveUICursor;
    public float cursorSpd;

    public GameObject startButton;
    public GameObject[] startButtons;

    public GameObject player;
    public PlayerStatus playerStat;
    public GameObject playerStatView;
    public GameObject bedBlind;

    #region save, load
    public DungeonManager dungeonManager;
    public CanvasManager canvasManager;
    public Menu_Storage storage;
    public Menu_Inventory inventory;

    public GameObject saveSlot;
    public GameObject[] saveSlots;
    public DataBase dataBase;

    BinaryFormatter bf;
    MemoryStream ms;
    public int gameSlotFocus;
    public int saveSlotFocus;
    public string data;
    
    public bool openSaveSlot;
    public bool gameStart;
    #endregion

    public GameObject[] screenSize;
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

        Physics2D.IgnoreLayerCollision(5, 10);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(8, 14);
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 13);
        Physics2D.IgnoreLayerCollision(10, 14);
        Physics2D.IgnoreLayerCollision(13, 13);
        Physics2D.IgnoreLayerCollision(13, 14);
        Physics2D.IgnoreLayerCollision(14, 14);

        QualitySettings.vSyncCount = 0;                 // 동기화 수치 고정
        Application.targetFrameRate = 60;               // 최대 프레임 조절
        Screen.SetResolution(1280, 720, true);          // 화면 해상도 1280 x 720 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        playerStat = player.GetComponent<PlayerStatus>();
        storage = canvasManager.storage.GetComponent<Menu_Storage>();
        inventory = canvasManager.Menus[0].GetComponent<Menu_Inventory>();
        dataBase = new DataBase();

        Init();

        Debug.Log("gameManager awake");
    }

    private void Init()
    {
        player.GetComponent<PlayerControl>().enabled = false;

        dataBase.Init();
        storage.Init();
        inventory.Init();

        gameStart = false;
        saveSlotFocus = 0;
        gameSlotFocus = 0;

        playerStatView.SetActive(false);
        canvasManager.inGameMenu.SetActive(false);
        canvasManager.FadeInStart();
        startButton.SetActive(true);

        Debug.Log("gameManager Init");
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;      // 씬 넘버가 0일때만 실행
        if (canvasManager.GameMenuOnCheck()) return;
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (openSaveSlot)
            {
                CloseLoad();
                canvasManager.inGameMenu.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (openSaveSlot)
            {
                DeleteSave();
            }
        }

        if (openSaveSlot)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { SaveFocusedSlot(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { SaveFocusedSlot(-1); }
            SaveCursorMove();
        }
        else if (!gameStart && !openSaveSlot)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { GameStartFocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { GameStartFocusedSlot(1); }
            MainCursorMove();
        }
    }

    public void GameStart()
    {
        if (gameStart)          // 게임 실행중이면
        {
            if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
            canvasManager.PlayerMoveStop();
            gameStart = false;
            SaveGame();         // 게임을 세이브
        }
        else                    // 게임이 실행중이 아니면
        {
            if (!openSaveSlot)      // 세이브창이 켜져있지 않으면
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
            else
            {
                LoadGame();
            }
        }
    }

    void MainCursorMove() {
        mainUICursor.transform.position = Vector2.Lerp(mainUICursor.transform.position,
           new Vector2(mainUICursor.transform.position.x, startButtons[gameSlotFocus].transform.position.y), Time.deltaTime * cursorSpd);
    }
    void SaveCursorMove() {
        saveUICursor.transform.position = Vector2.Lerp(saveUICursor.transform.position,
            new Vector2(saveSlots[saveSlotFocus].transform.position.x, saveUICursor.transform.position.y), Time.deltaTime * cursorSpd);
    }

    void SaveFocusedSlot(int AdjustValue)
    {
        if (saveSlotFocus + AdjustValue < 0) saveSlotFocus = 2;
        else if (saveSlotFocus + AdjustValue > 2) saveSlotFocus = 0;
        else saveSlotFocus += AdjustValue;

        data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);
    }
    void GameStartFocusedSlot(int AdjustValue)
    {
        if (gameSlotFocus + AdjustValue < 0) gameSlotFocus = 2;
        else if (gameSlotFocus + AdjustValue > 2) gameSlotFocus = 0;
        else gameSlotFocus += AdjustValue;
    }

    public void SaveGame()
    {
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
        bedBlind = GameObject.Find("BackGround/Base/bg_mainScene_blind");
        canvasManager.inGameMenu.SetActive(false);
        bedBlind.SetActive(true);
        playerStatView.SetActive(false);
        startButton.SetActive(true);

        Debug.Log("Save");
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

        canvasManager.inGameMenu.SetActive(true);
        playerStatView.SetActive(true);
        startButton.SetActive(false);
        gameStart = true;

        bedBlind.SetActive(false);
        player.GetComponent<PlayerControl>().SetCurrentJumpCount();
        player.GetComponent<PlayerControl>().enabled = true;

        Debug.Log("Load");
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
    
    public void OpenLoad()
    {
        startButton.SetActive(false);
        saveSlot.SetActive(true);

        openSaveSlot = true;
        saveSlotFocus = 0;
        
        for(int i = 0; i < 3; ++i)
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
        openSaveSlot = false;
        saveSlot.SetActive(false);
        startButton.SetActive(true);
    }

    public void SettingSave()
    {
        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 시스템 정보
        systemData = new SystemData();
        systemData.Init();

        bf.Serialize(ms, systemData);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("SystemData", data);
        Debug.Log("save complete");
    }
    public void SettingSet()
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
            }
        }
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