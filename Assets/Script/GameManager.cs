﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject startButton;
    public GameObject player;
    public PlayerStatus playerStat;
    public GameObject playerStatView;
    public GameObject bedBlind;

    #region save, load
    public DataBase dataBase;
    public CanvasManager canvanManager;
    public Menu_Storage storage;
    public Menu_Inventory inventory;
    public Button[] saveSlot;

    BinaryFormatter bf;
    MemoryStream ms;
    public int focus;
    public int slotNum;
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
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(this);

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
        canvanManager = GameObject.Find("UI").GetComponent<CanvasManager>();
        storage = canvanManager.Menus[3].GetComponent<Menu_Storage>();
        inventory = canvanManager.Menus[0].GetComponent<Menu_Inventory>();
    }

    private void Start()
    {
        dataBase = new DataBase();
        canvanManager = GameObject.Find("UI").GetComponent<CanvasManager>();

        player.GetComponent<PlayerControl>().enabled = false;
        storage.Init();
        inventory.Init();
        canvanManager.inGameMenu.SetActive(false);
        playerStatView.SetActive(false);

        gameStart = false;

        focus = 0;
        slotNum = 1;
    }

    public void Update()
    {
        if (canvanManager.isCancelOn || canvanManager.isInventoryOn || canvanManager.isStorageOn) return;

        if (SceneManager.GetActiveScene().buildIndex != 0) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (DungeonManager.instance.useTeleportSystem == 9)
            {
                if (gameStart)
                {
                    SaveGame();
                }
                else
                {
                    if (!openSaveSlot)
                    {
                        OpenLoad();
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
                player.GetComponent<PlayerControl>().enabled = true;
            }
        }

        if (!openSaveSlot) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
    }
    
    void FocusedSlot(int AdjustValue)
    {
        saveSlot[focus].GetComponent<Image>().color = new Color(1, 1, 1, 1);

        if (focus + AdjustValue < 0) focus = 2;
        else if (focus + AdjustValue > 2) focus = 0;
        else focus += AdjustValue;

        slotNum = focus + 1;

        saveSlot[focus].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
    }
    public void SelectSlot(int _slotNum)
    {
        slotNum = _slotNum;
    }

    public void SaveGame()
    {
        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 유저 정보
        dataBase.playerData = playerStat.playerData;
        dataBase.SaveCurrentDate(DungeonManager.instance.currentDate);
        storage.SaveStorageData(dataBase);
        inventory.SaveInventoryData(dataBase);

        bf.Serialize(ms, dataBase);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("SaveSlot" + slotNum, data);
        player.GetComponent<PlayerControl>().enabled = false;
        gameStart = false;
        GameObject.Find("bg_mainScene_blind");
        bedBlind.SetActive(true);
        startButton.SetActive(true);

        Debug.Log("Save");
    }
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("SaveSlot" + slotNum))
        {
            data = PlayerPrefs.GetString("SaveSlot" + slotNum, null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                dataBase = (DataBase)bf.Deserialize(ms);
                
                playerStat.SetPlayerData(dataBase.playerData);
                DungeonManager.instance.currentDate = dataBase.GetcurrentDate();
                storage.LoadStorageData(dataBase.GetStorageItemCodeList(), dataBase.GetAvailableStorageSlot());
                inventory.LoadInventoryData(dataBase.GetTakeKeySlot(), dataBase.GetAvailableInventorySlot());
            }
        }
        else
        {
            playerStat.NewStart(dataBase.playerData);
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
        if(PlayerPrefs.HasKey("SaveSlot" + slotNum))
            PlayerPrefs.DeleteKey("SaveSlot" + slotNum);
    }

    public void OpenLoad()
    {
        if (!canvanManager.OpenLoadSlot()) return;
        startButton.SetActive(false);
        
        player.GetComponent<PlayerControl>().enabled = false;
        openSaveSlot = true;
        focus = 0;
        slotNum = focus + 1;
        saveSlot[0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
    }

    public void CloseLoad()
    {
        saveSlot[focus].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        focus = 0;
        openSaveSlot = false;
        canvanManager.CloseLoadSlot();
        startButton.SetActive(true);
        player.GetComponent<PlayerControl>().enabled = true;
        
    }

    public void OpenSetting()
    {
        //inGameMenu.GetComponent<MainUI_InGameMenu>().OpenSettings(screenWidth, screenHeigth);
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

        PlayerPrefs.SetString("SystemData" + slotNum.ToString(), data);
        Debug.Log("save complete");
    }
    public void SettingSet()
    {
        if (PlayerPrefs.HasKey("SystemData" + slotNum.ToString()))
        {
            data = PlayerPrefs.GetString("SystemData" + slotNum.ToString(), null);

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