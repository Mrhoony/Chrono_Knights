using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject[] monsterList;
    GameObject[] currentStageMonsterList;
    GameObject portal;
    GameObject[] spawner;

    public int currentStage;
    public int dangerous;
    public int repeat;

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
        dangerous = 0;
        repeat = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCoolTime >= 0)
            spawnCoolTime -= Time.deltaTime;

        if (spawnCoolTime < 0 && spawnCount < monsterList.Length)
        {
            spawnCoolTime = Random.Range(2f, 3f);
            randomX = Random.Range(-1f, 1f);
            MonsterSpawn();
        }
        /*if(monsterList.Length > 0)
        {
            foreach (GameObject monster in monsterList)
            {
                if (monster.GetComponent<MonsterControl>().isDead)
                    monster.SetActive(false);
            }
        }*/
    }

    public void MonsterKill()
    {
        ++allKillCount;
        if(monsterList.Length == allKillCount)
        {
            if (portal.name == "Village")
            {
                portal.SetActive(true);
            }
        }
    }

    void MonsterSpawn()
    {
        monsterList[spawnCount].SetActive(true);
        monsterList[spawnCount].GetComponent<MonsterControl>().ActivateMonster(spawner[Random.Range(0, spawner.Length)].transform.position.x + randomX, spawner[Random.Range(0, spawner.Length)].transform.position.y);
        spawnCount += 1;
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
        portal = GameObject.FindGameObjectWithTag("Portal");
        spawner = GameObject.FindGameObjectsWithTag("Spawn"); 
        spawnCoolTime = Random.Range(2f, 3f);
        randomX = Random.Range(-1f, 1f);
        currentStage = 0;
        dangerous = SceneManager.GetActiveScene().buildIndex * 10 * repeat;

        int i = 0;

        foreach (GameObject monster in monsterList)
        {
            if(monster.GetComponent<MonsterControl>().degreeOfRisk >= repeat)
            {
                currentStageMonsterList[i] = monster;
                ++i;
            }
        }

        portal.SetActive(false);

        allKillCount = 0;
        spawnCount = 0;
        dungeonClear = false;
    }
}
