using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    
    public GameObject player;

    public GameObject[] mapList;
    public int selectedMapNum = 0;
    public GameObject startingPosition;

    public int day;

    public GameObject[] monsterList;
    public GameObject[] currentStageMonsterList;

    public GameObject[] spawner;
    int spawnerCount;
    int spawn;
    
    float randomX;

    public int currentStage;
    public bool dungeonClear;   // 던전 클리어시
    public bool sectionClear;   // 페이즈 클리어시

    public int monsterCount;        // 최대 몬스터 수
    public int allKillCount;    // 총 몬스터 킬 수

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);

        player = GameObject.Find("Player Character");

        day = 0;
        currentStage = 0;
        monsterCount = 0;
        dungeonClear = false;
    }

    private void Init()
    {
        currentStage = 0;
        dungeonClear = false;
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

    public void Teleport()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            if (sectionClear)
                SectionTeleport();
            else
                if(dungeonClear)
                    DungeonTeleport();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SectionTeleport();
        }
    }

    public void DungeonTeleport()
    {
        FloorInit();
    }

    public void SectionTeleport()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene("TopFirstFloor");
        }
        else if(SceneManager.GetActiveScene().buildIndex > 1)
        {
            SceneManager.LoadScene("Town");
            player.GetComponent<PlayerStat>().Init();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void PlayerDie()
    {
        SectionTeleport();
    }

    void FloorInit()
    {
        ++currentStage;

        dungeonClear = false;
        sectionClear = false;

        spawnerCount = 0;
        selectedMapNum = Random.Range(0, mapList.Length);

        startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
        player.transform.position = startingPosition.transform.position;

        CameraManager.instance.SetCameraBound(mapList[selectedMapNum].transform.Find("BackGround").GetComponent<BoxCollider2D>());  // 카메라 설정

        if (currentStage > 0)
            sectionClear = true;

        // 몬스터 스포너 등록
        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
        {
            if (child.gameObject.tag == "Spawn")
            {
                ++spawnerCount;
            }
        }

        spawner = new GameObject[spawnerCount];
        spawnerCount = 0;

        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
        {
            if (child.gameObject.tag == "Spawn")
            {
                spawner[spawnerCount] = child.gameObject;
                ++spawnerCount;
            }
        }

        // 몬스터 스폰
        for(int i = 0; i < Random.Range(2,5); ++i)
        {
            randomX = Random.Range(-1, 2);
            Instantiate(monsterList[Random.Range(0, monsterList.Length)], new Vector2(spawner[Random.Range(0, spawner.Length)].transform.position.x + randomX
                                                                                    , spawner[Random.Range(0, spawner.Length)].transform.position.y), Quaternion.identity);
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
        if(SceneManager.GetActiveScene().buildIndex > 1)
        {
            mapList = GameObject.FindGameObjectsWithTag("BaseMap");
            FloorInit();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            CameraManager.instance.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
            GameObject stp = GameObject.FindGameObjectWithTag("StartPosition");
            player.transform.position = stp.transform.position;
        }
    }
}
