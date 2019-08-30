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
    GameObject DM;
    GameObject player;
    GameObject teleport;
    GameObject startingPosition;

    int selectedFloor;
    bool[] floorFlag;

    string currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(this);

        player = GameObject.Find("Player Character");
        DM = GameObject.Find("DungeonManager");
        
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(10, 10);
        selectedFloor = 0;
        floorFlag = new bool[100];
    }

    public void Start()
    {
        DM.SetActive(false);
    }

    public void JoinDungeon()
    {
        DM.SetActive(true);
    }

    public void OutDungeon()
    {
        DM.SetActive(false);
    }

    public void Update()
    {
        if (teleport.GetComponent<Teleport>().teleportOn)
        {
            teleport.GetComponent<Teleport>().teleportOn = false;
            TeleportStart();
            Debug.Log("teleport");
        }
    }

    // 씬에서 씬, 마을 에서 던전, 던전 층 이동시
    public void TeleportStart()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Town")
        {
            JoinDungeon();
            SceneManager.LoadScene("TopFirstFloor");
            PlayerControl.instance.transform.position = startingPosition.transform.position;
        }
        else if (currentSceneName == "TopFirstFloor")
        {
            selectedFloor = 2;
            if(selectedFloor >= 2)
            {
                if (floorFlag[selectedFloor - 2])
                {
                    Debug.Log("floor " + selectedFloor + " join");
                    SceneManager.LoadScene(((selectedFloor - 2) % 4 + 2));
                    PlayerControl.instance.transform.position = startingPosition.transform.position;
                    if (selectedFloor > 6)
                    {
                        DM.GetComponent<DungeonManager>().repeat = (int)((selectedFloor - 2) * 0.25 + 1);
                    }
                }
                else
                    Debug.Log("join fail");
            }else if(selectedFloor == 0)
            {
                DM.SetActive(false);
                SceneManager.LoadScene(selectedFloor);
                PlayerControl.instance.transform.position = startingPosition.transform.position;
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerControl.instance.transform.position = startingPosition.transform.position;
        }
    }
    
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startingPosition = GameObject.Find("StartingPosition");
        teleport = GameObject.Find("Entrance");

        if (SceneManager.GetActiveScene().buildIndex == 1)
            teleport.SetActive(true);
        else
            teleport.SetActive(false);
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        // 유저 정보
        PlayerStat ps = GameObject.Find("Player Character").GetComponent<PlayerStat>();
        bf.Serialize(ms, ps);
        string data = Convert.ToBase64String(ms.GetBuffer());
        PlayerPrefs.SetString("PlayerStatus", data);
    }

    public void LoadGame()
    {
        string data = PlayerPrefs.GetString("PlayerStatus", null);

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));

            // 유저 정보
            PlayerStat ps = GameObject.Find("Player").GetComponent<PlayerStat>();
            ps = (PlayerStat)bf.Deserialize(ms);
        }
    }
}