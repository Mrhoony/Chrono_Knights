using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject player;
    public GameObject playerStatView;
    public Menu_InGame menu_ingame;

    public PlayerStat pStat;

    public int currentDate;
    public bool newDay;
    public bool possible_Traning;

    public GameObject[] mapList;
    public int selectedMapNum = 0;
    public GameObject startingPosition;

    GameObject choice;
    Sprite[] choiceSprite;

    enum EntryChoice
    {
        multy,
        devide,
        mReturn,
        freePass,
        boss,
        repeat
    }

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
    public int currentMonsterCount;
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

        menu_ingame = Menu_InGame.instance;
        pStat = player.GetComponent<PlayerStat>();
        choiceSprite = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");

        currentDate = 1;
        currentStage = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        newDay = false;
        dungeonClear = false;
        possible_Traning = true;

    }

    bool isStand = false;

    public void isStanding(bool StandingCheck)
    {
        isStand = StandingCheck;
    }

    public void Teleport()  // 인수로 Key 받음
    {
        if(isStand == true) // 플레이어가 텔레포터 앞에 서 있으면
        {
            choice.GetComponent<Question>().Execute();  // 스크립트 내부 수정 필요
            //
                /* 다음 층으로 이동하는 코드 */
            //
        }

        #region 미사용 코드
        /*
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            if (sectionClear)
                SectionTeleport(false, false);
            else
                if (dungeonClear)
                FloorInit(EntryChoice.multy, 1, false); // 키 배수
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SectionTeleport(false, false);
        }
        */
        #endregion
    }

    public void SectionTeleport(bool isDead, bool exit)
    {
        if (exit)
        {
            SceneManager.LoadScene("MainMenu");
            dungeonClear = true;
        }
        else
        {
            if (!isDead)
            {
                // 임시로 클리어시 무조건 마을로
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SceneManager.LoadScene("TopFirstFloor");
                }
                else if (SceneManager.GetActiveScene().buildIndex > 1)
                {
                    SceneManager.LoadScene("Town");
                    ++currentDate;
                    newDay = true;
                    NewDayCheck();
                    pStat.Init();
                    pStat.HPInit();
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
            else
            {
                NewDayCheck();
                SceneManager.LoadScene("Town");
                dungeonClear = true;
                pStat.Init();
                pStat.HPInit();
            }
        }
    }

    public void PlayerDie()
    {
        SectionTeleport(true, false);
    }

    public void NewDayCheck()
    {
        if (newDay)
        {
            possible_Traning = true;
        }
    }
    
    void FloorInit(EntryChoice selectChoice,int keyMul, bool freePass)
    {
        if (freePass)
            currentStage += 2;
        else
            ++currentStage;

        switch (selectChoice)
        {
            case EntryChoice.multy:
                FloorSetting(keyMul, true, false);
                break;
            case EntryChoice.devide:
                FloorSetting(keyMul, false, false);
                break;
            case EntryChoice.freePass:
                FloorSetting(1, true, false);
                break;
            case EntryChoice.boss:
                BossStageSetting();
                break;
            case EntryChoice.repeat:
                FloorSetting(1, true, true);
                break;
            case EntryChoice.mReturn:
                SectionTeleport(false, true);
                break;
        }

        choice.GetComponent<SpriteRenderer>().sprite = choiceSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈
        choice.GetComponent<Question>();
        // choice.GetComponent<MarkQuestion>(); // 텔레포터 지문 스크립트를 받음
    }

    public void FloorSetting(int keyMul, bool multy, bool repeat)
    {
        dungeonClear = false;
        sectionClear = false;
        spawnerCount = 0;
        selectedMapNum = Random.Range(0, mapList.Length);

        if((currentStage - 2) > 0)  // 보스스테이지 설정
        {
            int bossStage = currentStage % 5;
            if (currentStage == 5)
                bossStage = 5;

            if (bossStage * 20 > Random.Range(0,81))
                BossStageSetting();
        }


        if (repeat)
        {
            player.transform.position = startingPosition.transform.position;

            currentMonsterCount = monsterCount;

            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y);
                currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterInit();
            }
        }
        else
        {
            startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
            player.transform.position = startingPosition.transform.position;

            choice = mapList[selectedMapNum].transform.Find("Base/BG_Teleporter/Choice").gameObject;

            CameraManager.instance.SetCameraBound(mapList[selectedMapNum].transform.Find("BackGroundBound").GetComponent<BoxCollider2D>());  // 카메라 설정
            mapList[selectedMapNum].transform.Find("BackGroundBound").transform.position = new Vector2(Random.Range(-1f, 0), Random.Range(-2f, 0));

            // 하나만 클리어 해도 마을로
            if (currentStage > 0)
                sectionClear = true;

            // 몬스터 스포너 등록
            foreach (Transform child in mapList[selectedMapNum].transform.Find("Base").transform)
            {
                if (child.gameObject.CompareTag("Spawn")) ++spawnerCount;
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

            monsterCount = Random.Range(3, 10);

            if (multy)
                monsterCount *= keyMul;
            else
                monsterCount /= keyMul;

            currentMonsterCount = monsterCount;

            // 키 변수
            currentStageMonsterList = new GameObject[monsterCount];

            // 몬스터 스폰
            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterList[Random.Range(0, monsterList.Length)],new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
            }
        }
    }

    public void BossStageSetting()
    {

    }

    public void MonsterDie()
    {
        --currentMonsterCount;
        if (currentMonsterCount <= 0)   // 몬스터가 다 죽으면
        {
            Rect rect = choice.GetComponent<SpriteRenderer>().sprite.rect;
            rect.size = new Vector2(22f, 22f);
            dungeonClear = true;
        }
    }

    public bool GetDungeonClear()
    {
        return dungeonClear;
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
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            mapList = GameObject.FindGameObjectsWithTag("BaseMap");
            FloorInit(EntryChoice.multy, 1, false);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            CameraManager.instance.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
            GameObject stp = GameObject.FindGameObjectWithTag("StartPosition");
            player.transform.position = stp.transform.position;
            dungeonClear = true;
            pStat.Init();
            pStat.HPInit();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            player.transform.position = GameObject.Find("StartingPosition").transform.position;
            player.GetComponent<PlayerControl>().enabled = false;
            playerStatView.SetActive(false);
        }
    }
}
