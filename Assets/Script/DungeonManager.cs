using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject player;
    public GameObject playerStat;

    public int currentDate;
    public bool newDay;

    public GameObject[] mapList;
    public int selectedMapNum = 0;
    public GameObject startingPosition;

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

        currentDate = 1;
        currentStage = 0;
        monsterCount = 0;
        newDay = true;
        //newDay = false;
        dungeonClear = false;
    }

    private void Init()
    {
        currentStage = 0;
        dungeonClear = false;
    }
    
    public void Teleport()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            if (sectionClear)
                SectionTeleport(false, false);
            else
                if(dungeonClear)
                    DungeonTeleport();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SectionTeleport(false, false);
        }
    }

    public void DungeonTeleport()
    {
        FloorInit(2);
    }

    public void SectionTeleport(bool isDead, bool exit)
    {
        if (exit)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (!isDead)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SceneManager.LoadScene("TopFirstFloor");
                }
                else if (SceneManager.GetActiveScene().buildIndex > 1)
                {
                    SceneManager.LoadScene("Town");
                    ++currentDate;
                    newDay = true;
                    player.GetComponent<PlayerStat>().Init();
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else
            {
                SceneManager.LoadScene("Town");
                player.GetComponent<PlayerStat>().Init();
            }
        }
    }

    public void PlayerDie()
    {
        SectionTeleport(true, false);
    }

    void FloorInit(int keyMul)
    {
        ++currentStage;

        dungeonClear = false;
        sectionClear = false;

        spawnerCount = 0;
        selectedMapNum = Random.Range(0, mapList.Length);

        startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
        player.transform.position = startingPosition.transform.position;

        CameraManager.instance.SetCameraBound(mapList[selectedMapNum].transform.Find("BackGroundBound").GetComponent<BoxCollider2D>());  // 카메라 설정
        mapList[selectedMapNum].transform.Find("BackGroundBound").transform.position = new Vector2(Random.Range(-1f, 0), Random.Range(-2f, 0));

        if (currentStage > 0)
            sectionClear = true;

        // 몬스터 스포너 등록
        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
        {
            if (child.gameObject.CompareTag("Spawn"))
            {
                ++spawnerCount;
            }
        }

        spawner = new GameObject[spawnerCount];
        spawnerCount = 0;

        foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
        {
            if (child.gameObject.CompareTag("Spawn"))
            {
                spawner[spawnerCount] = child.gameObject;
                ++spawnerCount;
            }
        }

        keyMul = 1;
        monsterCount = Random.Range(10, 10 * keyMul);
        currentStageMonsterList = new GameObject[monsterCount];
        // 몬스터 스폰
        for (int i = 0; i < monsterCount; ++i)
        {
            randomX = Random.Range(-1, 2);
            currentStageMonsterList[i] = Instantiate(monsterList[Random.Range(0, monsterList.Length)], new Vector2(spawner[Random.Range(0, spawner.Length)].transform.position.x + randomX
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
            FloorInit(2);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            CameraManager.instance.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
            GameObject stp = GameObject.FindGameObjectWithTag("StartPosition");
            player.transform.position = stp.transform.position;
            player.GetComponent<PlayerControl>().pStat.Init();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            player.transform.position = GameObject.Find("StartingPosition").transform.position;
            player.GetComponent<PlayerControl>().enabled = false;
            playerStat.SetActive(false);
        }
    }
}
