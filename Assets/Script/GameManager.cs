using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainMenu;
    public GameObject inGameMenu;
    public GameObject player;
    public GameObject playerStatView;
    public PlayerData playerData;
    public GameObject[] screenSize;
    public SystemData systemData;

    int screenNumber;

    BinaryFormatter bf;
    MemoryStream ms;
    int slotNum;
    string data;

    int screenWidth;
    int screenHeigth;

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
        Physics2D.IgnoreLayerCollision(10, 10);
        slotNum = 0;
    }

    public void Start()
    {
        player.transform.position = GameObject.Find("StartingPosition").transform.position;
        player.GetComponent<PlayerControl>().enabled = false;
        inGameMenu.SetActive(false);
        playerStatView.SetActive(false);
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
        playerData = player.GetComponent<PlayerStat>().playerData;
        playerData.currentDate = DungeonManager.instance.currentDate;

        bf.Serialize(ms, playerData);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("PlayerData" + slotNum, data);
        Debug.Log("save complete");
        inGameMenu.GetComponent<Menu_InGame>().CloseCancelMenu();
        DungeonManager.instance.SectionTeleport(false, true);
        mainMenu.SetActive(true);
    }
    public void LoadGame()
    {
        if(PlayerPrefs.HasKey("PlayerData" + slotNum))
        {
            data = PlayerPrefs.GetString("PlayerData" + slotNum, null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                player.GetComponent<PlayerStat>().playerData = (PlayerData)bf.Deserialize(ms);
                DungeonManager.instance.currentDate = playerData.currentDate;

                mainMenu.SetActive(false);
                inGameMenu.SetActive(true);
                playerStatView.SetActive(true);
                player.GetComponent<PlayerControl>().enabled = true;
                player.transform.localScale = new Vector3(1,1,0);

                mainMenu.GetComponent<MainMenu>().CloseLoad();
                CameraManager.instance.GameStartScreenSet();
                SceneManager.LoadScene("Town");
            }
        }
        else
        {
            // 새 슬롯에 시작할지 선택창
            
            mainMenu.SetActive(false);
            inGameMenu.SetActive(true);
            playerStatView.SetActive(true);
            player.GetComponent<PlayerStat>().NewStart();
            player.GetComponent<PlayerControl>().enabled = true;
            player.transform.localScale = new Vector3(1, 1, 0);

            mainMenu.GetComponent<MainMenu>().CloseLoad();
            CameraManager.instance.GameStartScreenSet();
            SceneManager.LoadScene("Town");
        }
    }
    public void DeleteSave()
    {
        if(PlayerPrefs.HasKey("PlayerData" + slotNum))
            PlayerPrefs.DeleteKey("PlayerData" + slotNum);
    }
    
    public void OpenSetting()
    {
        inGameMenu.GetComponent<Menu_InGame>().OpenSettings(screenWidth, screenHeigth);
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
}