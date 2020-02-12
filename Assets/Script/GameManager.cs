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
    public CanvasManager canvanManager;
    public Menu_Storage storage;
    public Menu_Inventory inventory;
    public Menu_Traning traning;

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
        storage = canvanManager.storage.GetComponent<Menu_Storage>();
        inventory = canvanManager.Menus[0].GetComponent<Menu_Inventory>();
        dataBase = new DataBase();

        Init();

        Debug.Log("gameManager awake");
    }

    private void Init()
    {
        dataBase.Init();

        player.GetComponent<PlayerControl>().enabled = false;

        storage.Init();
        inventory.Init();

        gameStart = false;
        saveSlotFocus = 0;
        gameSlotFocus = 0;

        playerStatView.SetActive(false);
        canvanManager.inGameMenu.SetActive(false);
        Debug.Log("gameManager Start");
    }

    public void Update()
    {
        if (canvanManager.GameMenuOnCheck()) return;

        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (DungeonManager.instance.useTeleportSystem == 9)
            {
                if (gameStart)
                {
                    if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                    gameStart = false;
                    SaveGame();
                }
                else
                {
                    if (!openSaveSlot)
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
                canvanManager.inGameMenu.SetActive(true);
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

    public bool GetGameStart()
    {
        return gameStart;
    }
    
    void SaveFocusedSlot(int AdjustValue)
    {
        saveSlots[saveSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        saveSlots[saveSlotFocus].transform.position = new Vector3(saveSlots[saveSlotFocus].transform.position.x
            , saveSlots[saveSlotFocus].transform.position.y - 5f, saveSlots[saveSlotFocus].transform.position.z);

        if (saveSlotFocus + AdjustValue < 0) saveSlotFocus = 2;
        else if (saveSlotFocus + AdjustValue > 2) saveSlotFocus = 0;
        else saveSlotFocus += AdjustValue;

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
        dataBase.SaveGameData(dungeonManager.GetCurrentDate(), dungeonManager.GetEventFlag());
        dataBase.SaveStorageData(storage.GetStorageItemCodeList(), storage.GetStorageAvailableSlot());
        dataBase.SaveInventoryData(inventory.GetTakeItemSlot(), inventory.GetAvailableSlot());
        
        bf.Serialize(ms, dataBase);
        data = Convert.ToBase64String(ms.GetBuffer());
        
        storage.SaveStorageClear();

        PlayerPrefs.SetString("SaveSlot" + saveSlotFocus.ToString(), data);
        player.GetComponent<PlayerControl>().enabled = false;
        bedBlind = GameObject.Find("BackGroundSet/Base/bg_mainScene_blind");
        canvanManager.inGameMenu.SetActive(false);
        bedBlind.SetActive(true);
        playerStatView.SetActive(false);
        startButton.SetActive(true);

        Debug.Log("Save");
    }
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("SaveSlot" + saveSlotFocus.ToString()))
        {
            data = PlayerPrefs.GetString("SaveSlot" + saveSlotFocus.ToString(), null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                dataBase = (DataBase)bf.Deserialize(ms);
                
                playerStat.SetPlayerData(dataBase.playerData);
                dungeonManager.LoadGamePlayDate(dataBase.GetCurrentDate(), dataBase.GetEventFlag());
                storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.GetAvailableStorageSlot());
                inventory.LoadInventoryData(dataBase.GetTakeKeySlot(), dataBase.GetAvailableInventorySlot());
            }
        }
        else
        {
            Debug.Log("NoData");
            playerStat.SetPlayerData(dataBase.playerData);
            dungeonManager.LoadGamePlayDate(dataBase.GetCurrentDate(), dataBase.GetEventFlag());
            storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.GetAvailableStorageSlot());
            inventory.LoadInventoryData(dataBase.GetTakeKeySlot(), dataBase.GetAvailableInventorySlot());
        }
        CloseLoad();
        
        canvanManager.inGameMenu.SetActive(true);
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
            Debug.Log("saveDelete");
        }
    }
    
    public void CloseLoadSlot()
    {
        saveSlot.SetActive(false);
    }

    public void GameStartMenuSelect()
    {
        switch (gameSlotFocus)
        {
            case 0:
                OpenLoad();
                break;
            case 1:
                canvanManager.OpenSettings();
                break;
            case 2:
                Debug.Log("game over");
                break;
        }
    }

    public void OpenLoad()
    {
        startButton.SetActive(false);
        saveSlot.SetActive(true);

        player.GetComponent<PlayerControl>().enabled = false;
        openSaveSlot = true;
        saveSlotFocus = 0;
        saveSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
        saveSlots[0].transform.position = new Vector3(saveSlots[0].transform.position.x, saveSlots[0].transform.position.y + 5f, saveSlots[0].transform.position.z);
        
        for(int i = 0; i < 3; ++i)
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
                    Debug.Log(dataBase.GetCurrentDate().ToString() + " 일");
                }
            }
            else
            {
                Debug.Log("NoData");
            }
        }
    }
    public void CloseLoad()
    {
        saveSlots[saveSlotFocus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        saveSlots[saveSlotFocus].transform.position = new Vector3(saveSlots[saveSlotFocus].transform.position.x
            , saveSlots[saveSlotFocus].transform.position.y + 5f, saveSlots[saveSlotFocus].transform.position.z);
        saveSlotFocus = 0;
        openSaveSlot = false;
        CloseLoadSlot();
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