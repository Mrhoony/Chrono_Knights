using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameStartPosition;
    public GameObject inGameMenu;
    public GameObject mainMenu;
    public GameObject player;
    public GameObject playerStatView;
    public PlayerData playerData;
    public GameObject[] screenSize;
    public SystemData systemData;
    
    public GameObject LoadSlot;
    public GameObject[] saveSlot;
    public bool openSaveSlot;
    public bool gameStart;
    public GameObject exteriorDoor;

    public GameObject SettingsMenu;

    public int focus;
    public int screenNumber;

    BinaryFormatter bf;
    MemoryStream ms;
    int slotNum;
    string data;

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
        slotNum = 0;

        focus = 0;
        gameStart = false;
        Time.timeScale = 1f;
    }

    public void Start()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        player.transform.position = gameStartPosition.transform.position;
        inGameMenu.SetActive(false);
        playerStatView.SetActive(false);
    }

    public void Update()
    {
        if (!gameStartPosition.GetComponent<GameStartPosition>().inPlayer) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!exteriorDoor.GetComponent<Teleport>().inPlayer)
            {
                if (!openSaveSlot)
                {
                    openSaveSlot = true;
                    inGameMenu.SetActive(false);
                    slotNum = focus + 1;
                    OpenLoad();
                }
                else
                {
                    LoadGame();
                }
            }
            else
            {
                StartGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (openSaveSlot)
            {
                CloseLoad();
                inGameMenu.SetActive(true);
                player.GetComponent<PlayerControl>().enabled = true;
                openSaveSlot = false;
            }
        }

        if (!openSaveSlot) return;
        
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
    }
    
    void FocusedSlot(int AdjustValue)
    {
        saveSlot[focus].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        
        if (focus + AdjustValue < 0) focus = 2;
        else if (focus + AdjustValue > 2) focus = 0;
        else focus += AdjustValue;

        saveSlot[focus].GetComponent<Image>().color = new Color(255, 255, 255, 100);
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
        playerData = player.GetComponent<PlayerStatus>().playerData;
        playerData.currentDate = DungeonManager.instance.currentDate;

        bf.Serialize(ms, playerData);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("PlayerData" + slotNum, data);
        Debug.Log("save complete");

        inGameMenu.GetComponent<MainUI_Menu>().CloseCancelMenu();
        //DungeonManager.instance.SectionTeleport(false, true);     // 던전 매니져에 메인 메뉴 씬으로 이동 요청
    }

    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("PlayerData" + slotNum))
        {
            data = PlayerPrefs.GetString("PlayerData" + slotNum, null);

            if (!string.IsNullOrEmpty(data))
            {
                CloseLoad();

                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                player.GetComponent<PlayerStatus>().playerData = (PlayerData)bf.Deserialize(ms);
                DungeonManager.instance.currentDate = playerData.currentDate;
                
                inGameMenu.SetActive(true);
                playerStatView.SetActive(true);
                mainMenu.SetActive(false);
                gameStart = true;
                player.GetComponent<PlayerControl>().enabled = true;
            }
        }
        else
        {
            CloseLoad();
            
            inGameMenu.SetActive(true);
            playerStatView.SetActive(true);
            mainMenu.SetActive(false);
            gameStart = true;

            player.GetComponent<PlayerStatus>().NewStart();
            player.GetComponent<PlayerControl>().enabled = true;
        }
    }

    public void StartGame()
    {
        DungeonManager.instance.GoToTown();     // 던전 매니져에 마을 씬으로 이동 요청
    }

    public void ComeBackHome()
    {
        DungeonManager.instance.ComeBackHome();     // 던전 매니져에 마을 씬으로 이동 요청
    }

    public void DeleteSave()
    {
        if(PlayerPrefs.HasKey("PlayerData" + slotNum))
            PlayerPrefs.DeleteKey("PlayerData" + slotNum);
    }

    public void OpenLoad()
    {
        player.GetComponent<PlayerControl>().notMove = true;
        player.GetComponent<PlayerControl>().enabled = false;
        openSaveSlot = true;
        LoadSlot.SetActive(true);
        saveSlot[0].GetComponent<Image>().color = new Color(255, 255, 255, 100);
    }

    public void CloseLoad()
    {
        saveSlot[focus].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        focus = 0;
        openSaveSlot = false;
        LoadSlot.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = true;
        player.GetComponent<PlayerControl>().notMove = false;
    }

    public void OpenSetting()
    {
        //inGameMenu.GetComponent<MainUI_Menu>().OpenSettings(screenWidth, screenHeigth);
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
        if (PlayerPrefs.HasKey("SystemData" + slotNum))
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