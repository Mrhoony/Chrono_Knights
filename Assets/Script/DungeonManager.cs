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

    public GameObject[] monsterList;
    public GameObject[] currentStageMonsterList;

    public GameObject[] spawner;
    public GameObject startingPosition;

    public int currentStage;
    
    float randomX;

    int selectedScene;

    public bool dungeonClear;   // 던전 클리어시
    public bool sectionClear;   // 페이즈 클리어시

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

        player = GameObject.Find("Player Character");

        currentStage = 0;
        selectedScene = 0;
        dungeonClear = true;
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void FloorInit()
    {
        int i = 0;
        ++currentStage;
        selectedMapNum = Random.Range(0, mapList.Length);

        startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
        player.transform.position = startingPosition.transform.position;

        CameraManager.instance.SetCameraBound(mapList[selectedMapNum].transform.Find("BackGround").GetComponent<BoxCollider2D>());

        if (currentStage > 0)
            sectionClear = true;

        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
        {
            if (child.tag == "Spawn")
            {
                spawner[i] = child.gameObject;
                ++i;
            }
        }

        dungeonClear = false;
        sectionClear = false;
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
            CameraManager.instance.SetCameraBound(GameObject.Find("BackGroundSet/BackGround").GetComponent<BoxCollider2D>());
            GameObject stp = GameObject.FindGameObjectWithTag("StartPosition");
            player.transform.position = stp.transform.position;
        }
    }
}
