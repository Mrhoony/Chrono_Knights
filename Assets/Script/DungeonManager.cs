using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MarkerVariableNumber
{
    MonsterModifier,
    DropModifier,
    SpecialMonster,
    DamageBuffOnFloorModifier,
    DamageBuffOnMonsterModifier,
    DamageBuffOnPlayerModifier,
    PosHPOnMonsterModifier,
    NegHPOnMonsterModifier,
    PosDashSpeedOnPlayerModifier,
    NegDashSpeedOnPlayerModifier,
    PosDamageOnPlayerModifier,
    NegDamageOnPlayerModifier
}
public class MarkerVariable
{
    public MarkerVariableNumber markerVariableNumber;
    public int[] markerVariable = new int[12];
    public int[] markerPreVariable = new int[12];
    
    public void Reset() // 던전 초기화 시 실행필수
    {
        markerVariable[0] = 1;
        markerVariable[1] = 1;
        markerVariable[2] = 0;
        markerVariable[3] = 1;
        markerVariable[4] = 1;
        markerVariable[5] = 1;

        markerVariable[6] = 1;
        markerVariable[7] = 1;
        markerVariable[8] = 1;
        markerVariable[9] = 1;
        markerVariable[10] = 1;
        markerVariable[11] = 1;
    }
}

/*
 * 층, 소환몹갯수로 구성된 클래스
 * FloorDatas.Floor, FloorDatas.SpawnAmount로 접근
 * 선언은 Awake 함수 내부, 값설정은 선언시 1회 설정, 이후변경불가
 */
public class FloorData
{
    int floor;
    int spawnAmount;

    public FloorData(int Floor, int SpawnAmount)
    {
        floor = Floor;
        spawnAmount = SpawnAmount;
    }

    public int Floor
    {
        get { return floor; }
    }
    public int SpawnAmount
    {
        get { return spawnAmount; }
    }
}

public class DungeonManager : MonoBehaviour
{
    #region 오브젝트 등록
    public static DungeonManager instance;
    public new CameraManager camera;
    private CanvasManager menu;
    public GameObject playerStatView;
    public GameObject player;
    private PlayerStatus playerStatus;
    private EnemyStatus[] enemyStatus;
    private GameObject mark;
    public MarkerVariable marker_Variable;   // Marker로부터 전달받는 값 저장공간
    public Marker marker;
    public FloorData[] FloorDatas;
    #endregion

    public int markerRandom;
    private Sprite[] markSprite;

    private GameObject[] teleportPoint;
    private Vector2 entrance;            // 텔레포트 위치
    public int useTeleportSystem;       // 텔레포트 사용 방법 0~4 입구, 10 사용 안함
    public int currentDate;

    private bool newDay;
    
    #region 던전 생성 관련
    private GameObject[] mapList;
    public GameObject[] monsterList;
    public GameObject[] currentStageMonsterList;
    public GameObject[] spawner;

    private int selectedMapNum = 0;
    private int spawnerCount;
    private int spawn;
    private float randomX;
    
    private bool usedKey;
    private bool bossSetting;
    public int currentStage;
    public int bossStageCount;
    public bool floorRepeat;

    public bool dungeonClear;   // 던전 클리어시
    public bool phaseClear;   // 페이즈 클리어시

    public int monsterCount;        // 최대 몬스터 수
    public int currentMonsterCount;
    public int allKillCount;    // 총 몬스터 킬 수
    #endregion

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        camera = CameraManager.instance;
        playerStatus = player.GetComponent<PlayerStatus>();
        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
        markSprite = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");

        marker = new Marker();
        marker_Variable = new MarkerVariable();
        marker_Variable.Reset();

        /*
         * FloorData 선언
         */
        FloorDatas = new FloorData[71];
        FloorDatas[0] = new FloorData(0, 0); // 0층, 안씀
        for (int Floor = 1; Floor <= 70; Floor++)
        {
            FloorDatas[Floor] = new FloorData(Floor, Floor * 2);
        }
    }
    private void Start()
    {
        currentStage = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        bossStageCount = 0;
        bossSetting = false;
        newDay = false;
        dungeonClear = false;
        floorRepeat = false;
    }
    public void Update()
    {
        if(menu.isCancelOn || menu.isInventoryOn || menu.isStorageOn) return;

        if (Input.GetButtonDown("Fire1"))           // 공격키를 눌렀을 때
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)      // 메인 메뉴 화면에서
            {
                if (useTeleportSystem == 1)    // 캐릭터가 문 앞에 있을 때 마을로 간다
                {
                    GoToTown();
                    dungeonClear = false;
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)      // 마을 화면에서
            {
                if (useTeleportSystem == 0)          // 캐릭터가 집 문앞에 있을 경우 집으로 들어간다
                {
                    ComeBackHome();
                }
                else if (useTeleportSystem == 1)    // 캐릭터가 숲 입구 (임시 던전 입구)에 있을 경우 숲(던전)으로 간다
                {
                    menu.Menus[0].GetComponent<Menu_Inventory>().DeleteStorageItem();
                    SectionTeleport(false, false);
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)      // 마을 - 숲 화면에서
            {
                if (useTeleportSystem == 0)          // 캐릭터가 집 문앞에 있을 경우 집으로 들어간다
                {
                }
                else if (useTeleportSystem == 1)    // 캐릭터가 숲 입구 (임시 던전 입구)에 있을 경우 숲(던전)으로 간다
                {
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)      // 숲 - 타워 화면에서
            {
                if (useTeleportSystem == 0)          // 캐릭터가 집 문앞에 있을 경우 집으로 들어간다
                {
                }
                else if (useTeleportSystem == 1)    // 캐릭터가 숲 입구 (임시 던전 입구)에 있을 경우 숲(던전)으로 간다
                {
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                if (useTeleportSystem == 8)         // 던전 포탈 앞에 서있을 경우 다음던전 또는 집으로 이동한다.
                {
                    if (!dungeonClear) return;
                    if (usedKey)            // 키를 쓴경우
                    {
                        mapList = GameObject.FindGameObjectsWithTag("BaseMap");

                        for (int i = 0; i < 2; ++i)
                        {
                            if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                                entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                        }
                        FloorSetting();
                        player.transform.position = entrance;
                    }
                    else                    // 키를 안쓴경우 반응x (임시)집으로
                    {
                        menu.Menus[0].GetComponent<Menu_Inventory>().PutInBox(false);
                        ReturnToTown();
                        ComeBackHome();
                    }
                }
            }
        }
    }

    public void ReturnToTown()
    {
        newDay = true;
        bossSetting = false;
        dungeonClear = false;
        floorRepeat = false;

        currentStage = 0;
        bossStageCount= 0;
        monsterCount = 0;
        currentMonsterCount = 0;
    }

    public bool useKeyInDungeon(Key _key)
    {
        if (usedKey) return false;
        usedKey = true;
        // 키가 가진 것들을 가지고 체크
        switch (_key.Type)
        {
            case ItemType.Number:
                marker.ExecuteMarker(_key.Value);
                break;
            case ItemType.ReturnTown:
                // 마을로 돌아간다. 클리어 정보창 표시
                SectionTeleport(false, true);
                break;
            case ItemType.FreePassNextFloor:
                ++currentStage;
                FloorSetting();
                break;
            case ItemType.FreePassThisFloor:
                FloorSetting();
                break;
            case ItemType.BossFloor:
                bossSetting = true;
                break;
            case ItemType.ReturnPreFloor:
                marker_Variable.markerVariable = marker_Variable.markerPreVariable;
                break;
            case ItemType.RepeatThisFloor:
                floorRepeat = true;
                break;
        }
        return true;
    }

    // 씬 이동 (마을로, 계층이동)
    public void SectionTeleport(bool isDead, bool dungeonEscape)
    {
        if (dungeonEscape)
        {
            SceneManager.LoadScene(0);
            newDay = true;
            dungeonClear = false;
            playerStatus.Init();
            playerStatus.HPInit();
        }
        else
        {
            if (!isDead)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    SceneManager.LoadScene("TowerFirstFloor");
                }
                else if (SceneManager.GetActiveScene().buildIndex == 4)
                {
                    SceneManager.LoadScene("MainMenu");
                    newDay = true;
                    NewDayCheck();
                    dungeonClear = false;
                    ++currentDate;
                    playerStatus.Init();
                    playerStatus.HPInit();
                }
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
                newDay = true;
                NewDayCheck();
                dungeonClear = false;
                playerStatus.Init();
                playerStatus.HPInit();
            }
        }
    }

    // 층 이동 시 나타날 층 세팅
    public void FloorSetting()
    {
        phaseClear = false;
        dungeonClear = true;
        usedKey = false;
        spawnerCount = 0;
        selectedMapNum = Random.Range(0, mapList.Length);
        
        ++currentStage;
        // 다음층이 보스층인걸 미리 알면 순서 변경
        ++bossStageCount;
        if (!bossSetting)
        {
            if ((bossStageCount % 5 - 2) > 0)  // 보스스테이지 설정
            {
                int bossStage = bossStageCount % 5;
                if (bossStageCount == 5)
                    bossStage = 5;

                if (bossStageCount * 20 > Random.Range(50, 81))
                    bossSetting = true;
            }
        }

        if (bossSetting)    // 보스 층 일때
        {
            Debug.Log("Boss");
            //BossStageSetting();
            bossStageCount = 0;
            bossSetting = false;
        }
        else if (floorRepeat)    // 맵 반복시
        {
            player.transform.position = entrance;
            mark.GetComponent<SpriteRenderer>().sprite = markSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

            currentMonsterCount = monsterCount;

            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y);
                currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterInit();
            }
            floorRepeat = false;
        }
        else            // 일반 맵일경우
        {
            player.transform.position = entrance;
            // 초기 맵 랜덤 선택
            mapList[selectedMapNum].GetComponent<BackgroundScrolling>().backGroundImage.transform.position
                = new Vector2(Random.Range(-0.5f, 0.5f), -(currentStage * 0.2f));
            
            // (임시)하나만 클리어 해도 마을로
            if (currentStage > 0)
                phaseClear = true;

            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            monsterCount = FloorDatas[currentStage].SpawnAmount * marker_Variable.markerVariable[0];
            currentMonsterCount = monsterCount;
            currentStageMonsterList = new GameObject[monsterCount];
            enemyStatus = new EnemyStatus[monsterCount];

            // 몬스터 스폰
            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterList[Random.Range(0, monsterList.Length)], new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
                enemyStatus[i] = currentStageMonsterList[i].GetComponent<EnemyStatus>();
            }
        }
        SetFloorStatus();

        markerRandom = Random.Range(0, 12);
        marker.thisMarker = (Markers)markerRandom;
        mark = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().teleporter.transform.GetChild(0).gameObject;
        mark.GetComponent<SpriteRenderer>().sprite = markSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

        marker_Variable.markerPreVariable = marker_Variable.markerVariable;
        marker_Variable.Reset();
    }

    public void SetFloorStatus()
    {
        switch (marker.thisMarker)
        {
            case Markers.SetDamageBuffOnFloor_NF:
                playerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.DamageBuffOnFloorModifier], true);
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                playerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.PosDamageOnPlayerModifier], true);
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                break;
            case Markers.SetPosHPOnMonster_NF:
                break;
            case Markers.SetNegHPOnMonster_NF:
                break;
            case Markers.SetPosDamageOnPlayer_NF:
                playerStatus.SetAttackAdd_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.PosDamageOnPlayerModifier], true);
                break;
            case Markers.SetNegDamageOnPlayer_NF:
                playerStatus.SetAttackAdd_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.PosDamageOnPlayerModifier], false);
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.PosDashSpeedOnPlayerModifier], true);
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[(int)MarkerVariableNumber.PosDashSpeedOnPlayerModifier], false);
                break;
        }
    }
    
    public void GoToTown()
    {
        SceneManager.LoadScene("Town");
        camera.SetHeiWid(1280, 720);
    }
    public void ComeBackHome()
    {
        SceneManager.LoadScene("MainMenu");
        camera.SetHeiWid(640, 360);
    }
    public bool NewDayCheck()
    {
        if (newDay)
        {
            return true;
        }
        return false;
    }

    // 씬 이동 후 초기화
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
        useTeleportSystem = 10;

        camera = CameraManager.instance;
        camera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");
        int teleportCount = teleportPoint.Length;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)      // 메인 메뉴 씬 일 때
        {
            if (!GameManager.instance.gameStart)
            {
                for (int i = 0; i < teleportCount; ++i)
                {
                    if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                        entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                }
                player.GetComponent<PlayerControl>().enabled = false;
            }
            else
            {
                for (int i = 0; i < teleportCount; ++i)
                {
                    if (teleportPoint[i].GetComponent<Teleport>().useSystem == 1)
                        entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                }
            }
            playerStatus.Init();
            playerStatus.HPInit();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)  // 마을 화면 일 때
        {
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)  // 마을 - 숲 화면 일 때
        {
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)  // 숲 - 타워 화면 일 때
        {
            mapList = GameObject.FindGameObjectsWithTag("BaseMap");
            //SelectedKey(1, 1, false);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)  // 타워 첫번째 층 일 때
        {
            dungeonClear = false;

            mapList = GameObject.FindGameObjectsWithTag("BaseMap");

            for (int i = 0; i < 2; ++i)
            {
                if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                    entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
            }
            FloorSetting();
        }

        player.transform.position = entrance;
    }
}
/*
public void PlayerDie()
{
    SectionTeleport(true, false);
}

// 보스 층 세팅
public void BossStageSetting()
{

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

*/
