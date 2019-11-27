using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;
    public GameObject player;
    public GameObject playerStatView;
    public MainUI_Menu MainUI_Menu;

    public PlayerStatus pStat;

    public int currentDate;
    public bool newDay;
    public bool possible_Traning;

    public GameObject[] mapList;
    public int selectedMapNum = 0;
    public GameObject startingPosition;

    public Teleport teleport;

    GameObject choice;
    Sprite[] choiceSprite;
    bool bossSetting;

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

        MainUI_Menu = GameObject.Find("UI/Menus").GetComponent<MainUI_Menu>();
        pStat = player.GetComponent<PlayerStatus>();
        choiceSprite = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");

        currentDate = 1;
        currentStage = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        newDay = false;
        dungeonClear = false;
        possible_Traning = true;
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
    
    // 퀵슬롯에서 키 선택시
    public void SelectedKey(int selectQuestion, int keyMul, bool freePass)
    {
        if (freePass)
            currentStage += 2;
        else
            ++currentStage;

        switch (selectQuestion)
        {
            case 0:               // 마을로
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SectionTeleport(false, true);
                }
                else if (SceneManager.GetActiveScene().buildIndex > 1)
                {
                    SectionTeleport(false, false);
                }
                break;
            case 1:                 // 몹 배수
                FloorSetting(keyMul, true, false);
                break;
            case 2:              // 프리패스
                FloorSetting(1, true, false);
                break;
            case 3:                  // 보스전
                BossStageSetting();
                break;
            case 4:                // 맵 반복
                FloorSetting(1, true, true);
                break;
        }
    }

    // 층 이동 시 나타날 층 세팅
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

            if (bossStage * 20 > Random.Range(0, 81))
                bossSetting = true;
        }

        if (bossSetting)    // 보스 층 일때
        {
            BossStageSetting();
        }
        else if (repeat)    // 맵 반복시
        {
            choice.GetComponent<SpriteRenderer>().sprite = choiceSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

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
        else            // 일반 맵일경우
        {
            // 초기 맵 랜덤 세팅
            startingPosition = mapList[selectedMapNum].transform.Find("Base/StartingPosition").gameObject;
            teleport = mapList[selectedMapNum].transform.Find("Base/BG_Teleporter/Entrance").GetComponent<Teleport>();
            mapList[selectedMapNum].transform.Find("BackGroundBound").transform.position = new Vector2(Random.Range(-1f, 0), Random.Range(-2f, 0));
            choice = mapList[selectedMapNum].transform.Find("Base/BG_Teleporter/Choice").gameObject;
            CameraManager.instance.SetCameraBound(mapList[selectedMapNum].transform.Find("BackGroundBound").GetComponent<BoxCollider2D>());  // 카메라 설정

            choice.GetComponent<SpriteRenderer>().sprite = choiceSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

            player.transform.position = startingPosition.transform.position;
            
            // (임시)하나만 클리어 해도 마을로
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

    // 보스 층 세팅
    public void BossStageSetting()
    {

    }

    // 씬 이동 (마을로, 계층이동)
    public void SectionTeleport(bool isDead, bool exit)
    {
        if (exit)
        {
            newDay = true;
            SceneManager.LoadScene("TopFirstFloor");
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
    
    // 몬스터 죽을 때 마다 카운트
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
            SelectedKey(1, 1, false);
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
