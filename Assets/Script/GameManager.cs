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
    public GameObject playerStat;
    PlayerData pd;
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
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(10, 10);
        slotNum = 0;
    }

    public void Start()
    {
        player.transform.position = GameObject.Find("StartingPosition").transform.position;
        player.GetComponent<PlayerControl>().enabled = false;
        playerStat.SetActive(false);
    }

    public void SaveGame()
    {
        bf = new BinaryFormatter();
        ms = new MemoryStream();

        // 유저 정보
        pd = player.GetComponent<PlayerStat>().pd;
        bf.Serialize(ms, pd);
        data = Convert.ToBase64String(ms.GetBuffer());

        PlayerPrefs.SetString("PlayerData" + slotNum, data);
    }

    public void LoadGame(int _slotNum)
    {
        slotNum = _slotNum;
        if(PlayerPrefs.HasKey("PlayerData" + slotNum))
        {
            data = PlayerPrefs.GetString("PlayerData" + slotNum, null);

            if (!string.IsNullOrEmpty(data))
            {
                bf = new BinaryFormatter();
                ms = new MemoryStream(Convert.FromBase64String(data));

                // 유저 정보
                pd = GameObject.Find("PlayerCharacter").GetComponent<PlayerData>();
                pd = (PlayerData)bf.Deserialize(ms);

                player.SetActive(true);
                playerStat.SetActive(true);
                player.GetComponent<PlayerControl>().enabled = true;
                SceneManager.LoadScene("Town");
            }
        }
        else
        {
            player.GetComponent<PlayerStat>().NewStart();
            playerStat.SetActive(true);
            player.GetComponent<PlayerControl>().enabled = true;
            SceneManager.LoadScene("Town");
        }

    }

    public void DeleteSave(int _slotNum)
    {
        if(!PlayerPrefs.HasKey("PlayerData" + slotNum))
            PlayerPrefs.DeleteKey("PlayerData" + slotNum);
    }
}