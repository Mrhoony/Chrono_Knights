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
    public GameObject player;
    public GameObject UI;
    
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(this);

        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    public void Start()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        UI.SetActive(false);
    }

    public void Update()
    {
        /*
        if (teleport.GetComponent<Teleport>().teleportOn)
        {
            teleport.GetComponent<Teleport>().teleportOn = false;
            TeleportStart();
            Debug.Log("teleport");
        }
        */
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
        /*
        startingPosition = GameObject.Find("StartingPosition");
        teleport = GameObject.Find("Entrance");

        if (SceneManager.GetActiveScene().buildIndex == 1)
            teleport.SetActive(true);
        else
            teleport.SetActive(false);
        */
    }

    public void SaveGame(int slotNum)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        // 유저 정보
        PlayerStat ps = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
        bf.Serialize(ms, ps);
        string data = Convert.ToBase64String(ms.GetBuffer());
        PlayerPrefs.SetString("PlayerStatus" + slotNum, data);
    }

    public void LoadGame(int slotNum)
    {
        string data = PlayerPrefs.GetString("PlayerStatus" + slotNum, null);

        if (!string.IsNullOrEmpty(data))
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(data));

            // 유저 정보
            PlayerStat ps = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
            ps = (PlayerStat)bf.Deserialize(ms);
        }
        UI.SetActive(true);
        player.GetComponent<PlayerControl>().enabled = true;
        SceneManager.LoadScene("Town");
    }
}