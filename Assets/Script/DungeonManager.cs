using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public GameObject[] mapList;
    int selectedMapNum;

    public GameObject[] monsterList;
    GameObject[] currentStageMonsterList;

    GameObject[] spawner;
    GameObject startingPosition;

    public int currentStage;

    float spawnCoolTime = 1f;
    float randomX;
    int spawnCount;

    public bool dungeonClear;   // 던전 클리어시

    public int poolSize;        // 최대 몬스터 수
    public int allKillCount;    // 총 몬스터 킬 수

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(this);

        currentStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(spawnCoolTime >= 0)
            spawnCoolTime -= Time.deltaTime;

        if (spawnCoolTime < 0 && spawnCount < monsterList.Length)
        {
            spawnCoolTime = Random.Range(2f, 3f);
            randomX = Random.Range(-1f, 1f);
            MonsterSpawn();
        }

        ////

        if(monsterList.Length > 0)
        {
            foreach (GameObject monster in monsterList)
            {
                if (monster.GetComponent<MonsterControl>().isDead)
                    monster.SetActive(false);
            }
        }
        */
    }

    void MonsterSpawn()
    {
        monsterList[spawnCount].SetActive(true);
        monsterList[spawnCount].GetComponent<MonsterControl>().ActivateMonster(spawner[Random.Range(0, spawner.Length)].transform.position.x + randomX, spawner[Random.Range(0, spawner.Length)].transform.position.y);
        spawnCount += 1;
    }

    void FloorInit()
    {
        int i = 0;
        selectedMapNum = Random.Range(0, mapList.Length);

        startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base"))
        {
            if (child.tag == "Spawn")
            {
                spawner[i] = child.gameObject;
                ++i;
            }
        }

        allKillCount = 0;
        spawnCount = 0;
        dungeonClear = false;

        PlayerControl.instance.transform.position = startingPosition.transform.position;
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
        if(SceneManager.GetActiveScene().name != "Town")
        {
            mapList = GameObject.FindGameObjectsWithTag("BaseMap");
            FloorInit();
        }
    }
}
