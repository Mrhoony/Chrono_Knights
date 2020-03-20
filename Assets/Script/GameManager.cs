using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
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
        OpenStartButton();

        Debug.Log("gameManager Init");
    }

    public void Update()
    {
        if (canvasManager.GameMenuOnCheck()) return;
        if (SceneManager.GetActiveScene().buildIndex != 0) return;      // 씬 넘버가 0일때만 실행
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (DungeonManager.instance.useTeleportSystem == 9)         // 침대 앞에 있을 경우
            {
                if (gameStart)          // 게임 실행중이면
                {
                    if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                    gameStart = false;
                    SaveGame();         // 게임을 세이브
                }
                else                    // 게임이 실행중이 아니면
                {
                    if (!openSaveSlot)      // 세이브창이 켜져있지 않으면
                    {
                        GameStartMenuSelect();
                    }
                    else
                    {
                        LoadGame();
                    }
                }
            }
        }

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
        }
        if (!gameStart && !openSaveSlot)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { GameStartFocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { GameStartFocusedSlot(1); }
        }
    }

    void SaveFocusedSlot(int AdjustValue)
    {
        saveSlots[saveSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        saveSlots[saveSlotFocus].transform.position = new Vector3(saveSlots[saveSlotFocus].transform.position.x
            , saveSlots[saveSlotFocus].transform.position.y - 5f, saveSlots[saveSlotFocus].transform.position.z);

        if (saveSlotFocus + AdjustValue < 0) saveSlotFocus = 2;
        else if (saveSlotFocus + AdjustValue > 2) saveSlotFocus = 0;
        else saveSlotFocus += AdjustValue;

        data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);

        saveSlots[saveSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        saveSlots[saveSlotFocus].transform.position = new Vector3(saveSlots[saveSlotFocus].transform.position.x
            , saveSlots[saveSlotFocus].transform.position.y + 5f, saveSlots[saveSlotFocus].transform.position.z);
    }
    void GameStartFocusedSlot(int AdjustValue)
    {
        startButtons[gameSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        startButtons[gameSlotFocus].transform.position = new Vector3(startButtons[gameSlotFocus].transform.position.x
            , startButtons[gameSlotFocus].transform.position.y - 5f, startButtons[gameSlotFocus].transform.position.z);

        if (gameSlotFocus + AdjustValue < 0) gameSlotFocus = 2;
        else if (gameSlotFocus + AdjustValue > 2) gameSlotFocus = 0;
        else gameSlotFocus += AdjustValue;

        startButtons[gameSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        startButtons[gameSlotFocus].transform.position = new Vector3(startButtons[gameSlotFocus].transform.position.x
            , startButtons[gameSlotFocus].transform.position.y + 5f, startButtons[gameSlotFocus].transform.position.z);
    }

    public void SaveGame()
    {
        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 유저 정보
        dataBase.playerData = playerStat.playerData;
        dataBase.SaveGameData(dungeonManager.GetCurrentDate(), dungeonManager.GetTrainigPossible(), dungeonManager.GetEventFlag());
        dataBase.SaveStorageData(storage.GetStorageItemCodeList(), storage.GetStorageItemSkillCodeList(), storage.GetStorageAvailableSlot());
        dataBase.SaveInventoryData(inventory.GetAvailableSlot());
        
        bf.Serialize(ms, dataBase);
        data = Convert.ToBase64String(ms.GetBuffer());
        PlayerPrefs.SetString("SaveSlot" + saveSlotFocus.ToString(), data);

        storage.SaveStorageClear();
        dataBase.Init();
        player.GetComponent<PlayerControl>().enabled = false;
        bedBlind = GameObject.Find("BackGroundSet/Base/bg_mainScene_blind");
        canvasManager.inGameMenu.SetActive(false);
        bedBlind.SetActive(true);
        playerStatView.SetActive(false);

        OpenStartButton();

        Debug.Log("Save");
    }
    public void LoadGame()
    {
        startButtons[gameSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        startButtons[gameSlotFocus].transform.position = new Vector3(startButtons[gameSlotFocus].transform.position.x
            , startButtons[gameSlotFocus].transform.position.y - 5f, startButtons[gameSlotFocus].transform.position.z);

        if (PlayerPrefs.HasKey("SaveSlot" + saveSlotFocus.ToString()))
        {
            data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                dataBase = (DataBase)bf.Deserialize(ms);
                
                playerStat.SetPlayerData(dataBase.playerData);
                dungeonManager.LoadGamePlayDate(dataBase.GetCurrentDate(), dataBase.GetTrainingPossible(), dataBase.GetEventFlag());
                storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.GetStorageItemSkillCodeList(), dataBase.GetAvailableStorageSlot());
                inventory.LoadInventoryData(dataBase.GetAvailableInventorySlot());
            }
        }
        else
        {
            dataBase.Init();

            playerStat.SetPlayerData(dataBase.playerData);
            dungeonManager.LoadGamePlayDate(dataBase.GetCurrentDate(), dataBase.GetTrainingPossible(), dataBase.GetEventFlag());
            storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.GetStorageItemSkillCodeList(), dataBase.GetAvailableStorageSlot());
            inventory.LoadInventoryData(dataBase.GetAvailableInventorySlot());
        }
        CloseLoad();

        canvasManager.inGameMenu.SetActive(true);
        playerStatView.SetActive(true);
        startButton.SetActive(false);
        gameStart = true;

        bedBlind.SetActive(false);
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

    public void GameStartMenuSelect()
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
                Debug.Log("game over");
                break;
        }
    }
    public void OpenStartButton()
    {
        startButton.SetActive(true);

        startButtons[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        startButtons[0].transform.position = new Vector3(startButtons[0].transform.position.x
            , startButtons[0].transform.position.y + 5f, startButtons[0].transform.position.z);
    }

    public void OpenLoad()
    {
        startButton.SetActive(false);
        saveSlot.SetActive(true);

        openSaveSlot = true;
        saveSlotFocus = 0;
        saveSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        saveSlots[0].transform.position = new Vector3(saveSlots[0].transform.position.x, saveSlots[0].transform.position.y + 5f, saveSlots[0].transform.position.z);
        
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
        saveSlots[saveSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        saveSlots[saveSlotFocus].transform.position = new Vector3(saveSlots[saveSlotFocus].transform.position.x
            , saveSlots[saveSlotFocus].transform.position.y + 5f, saveSlots[saveSlotFocus].transform.position.z);
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
        Debug.Log("Quit"); // Application.Quit()은 에디터 상에서 작동x로 Debug.log로 동작 확인, 빌드시 삭제
        //Application.Quit();
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