using UnityEngine;
using UnityEngine.SceneManagement;

public enum Markers
{
    // NF = Next Floor, TF = This Floor
    // Pos = Positive, Neg = Negative
    SetMonster_NF = 0,
    SetDrop_NF,
    SetSpecialMonster_NF,
    SetDamageBuffOnFloor_NF,
    SetDamageBuffOnMonster_NF,
    SetDamageBuffOnPlayer_NF,

    SetPosHPOnMonster_NF,
    SetNegHPOnMonster_NF,
    SetPosDashSpeedOnPlayer_NF,
    SetNegDashSpeedOnPlayer_NF,
    SetPosAttackSpeedOnPlayer_NF,
    SetNegAttackSpeedOnPlayer_NF
}

public class Marker
{
    public Markers thisMarker = Markers.SetMonster_NF;

    // Marker 생성방법 필요

    /*
        아래 모든 함수는 인수로 키값이 필요
        DungeonManger.cs에서 Marker_Variable 클래스 생성
        Execute() 실행시 DungeonManager.cs 내에 Marker_Variable 클래스에 Execute()의 결과값을 받아서 계산하여 적용
    */

    public void ExecuteMarker(int keyValue)
    {
        switch (thisMarker)
        {
            case Markers.SetMonster_NF:
                {
                    SetMonster_NF(keyValue);
                }
                break;
            case Markers.SetDrop_NF:
                {
                    SetDrop_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnFloor_NF:
                {
                    SetDamageBuffOnFloor_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                {
                    SetDamageBuffOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                {
                    SetDamageBuffOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    SetPosHPOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    SetNegHPOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetPosAttackSpeedOnPlayer_NF:
                {
                    SetNegDamageOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetNegAttackSpeedOnPlayer_NF:
                {
                    SetPosDamageOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    SetPosDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    SetNegDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetSpecialMonster_NF:
                {
                    SetSpecialMonster_NF(keyValue);
                }
                break;
        }
    }
    private void SetMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[0] = keyValue;
    }
    private void SetDrop_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[1] = keyValue;
    }
    private void SetSpecialMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[2] = keyValue;
    }
    private void SetDamageBuffOnFloor_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[3] = keyValue;
    }
    private void SetDamageBuffOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[4] = keyValue;
    }
    private void SetDamageBuffOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[5] = keyValue;
    }
    private void SetPosHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[6] = keyValue;
    }
    private void SetNegHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[7] = keyValue;
    }
    private void SetPosDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[8] = keyValue;
    }
    private void SetNegDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[9] = keyValue;
    }
    private void SetPosDamageOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[10] = keyValue;
    }
    private void SetNegDamageOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[11] = keyValue;
    }
}
public class MarkerVariable
{
    public int[] markerVariable = new int[12];
    public int[] markerPreVariable = new int[12];
    
    public void Reset() // 던전 초기화 시 실행필수
    {
        markerVariable[(int)Markers.SetMonster_NF] = 0;
        markerVariable[(int)Markers.SetDrop_NF] = 0;
        markerVariable[(int)Markers.SetSpecialMonster_NF] = 0;
        markerVariable[(int)Markers.SetDamageBuffOnFloor_NF] = 1;
        markerVariable[(int)Markers.SetDamageBuffOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetDamageBuffOnPlayer_NF] = 1;

        markerVariable[(int)Markers.SetPosHPOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetNegHPOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetPosDashSpeedOnPlayer_NF] = 0;
        markerVariable[(int)Markers.SetNegDashSpeedOnPlayer_NF] = 0;
        markerVariable[(int)Markers.SetPosAttackSpeedOnPlayer_NF] = 1;
        markerVariable[(int)Markers.SetNegAttackSpeedOnPlayer_NF] = 1;
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
    int MaxDangerous;
    int MinDangerous;

    public FloorData(int _floor, int _MaxDangerous, int _MinDangerous)
    {
        floor = _floor;
        MaxDangerous = _MaxDangerous;
        MinDangerous = _MinDangerous;

        if ((floor % 5) != 0)
        {
            spawnAmount = (floor % 5) + 5;
        }
        else
        {
            spawnAmount = 10;
        }
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
    public CameraManager mainCamera;
    private CanvasManager canvasManager;
    public GameObject playerStatView;
    public GameObject player;
    private PlayerStatus playerStatus;
    private GameObject mark;
    public Dungeon_UI dungeonUI;
    private GameObject[] teleportPoint;
    #endregion
    
    public Marker marker;
    public MarkerVariable marker_Variable;   // Marker로부터 전달받는 값 저장공간
    public FloorData[] FloorDatas;
    public int markerRandom;
    private Sprite[] markSprite;

    private Vector2 entrance;            // 텔레포트 위치
    public int useTeleportSystem;       // 텔레포트 사용 방법 0~4 입구, 10 사용 안함
    public int currentDate;

    private bool isTraingPossible;
    public bool isDead;
    public bool freePassFloor;
    public bool freePassThisFloor;
    public int bossClear;
    public bool inDungeon;
    public bool[] eventFlag;
    
    #region 던전 생성 관련
    private GameObject[] mapList;
    public GameObject[] monsterPreFabsList;
    public GameObject[] bossMonsterPreFabsList;
    public GameObject[] currentStageMonsterList;
    public GameObject[] spawner;
    public GameObject dropItemPool;

    private int selectedMapNum = 0;
    private int spawnerCount;
    private int spawn;
    private float randomX;
    
    private bool usedKey;
    private bool bossSetting;
    public int currentStage;
    public int bossStageCount;
    public bool floorRepeat;

    public bool isSceneLoading;
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
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerStatus = player.GetComponent<PlayerStatus>();
        canvasManager = GameObject.Find("UI").GetComponent<CanvasManager>();
        markSprite = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");

        monsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/Monster");
        bossMonsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/BossMonster");

        marker = new Marker();
        marker_Variable = new MarkerVariable();
        marker_Variable.Reset();

        FloorDatas = new FloorData[70];
        FloorDangerousSetting(0);
    }
    private void FloorDangerousSetting(int plusDangerous)
    {
        for (int floor = 1; floor < 71; ++floor)
        {
            FloorDatas[floor - 1] = new FloorData(floor, bossClear, floor * 2);
        }
    }
    private void Init()
    {
        currentStage = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        bossStageCount = 0;
        useTeleportSystem = 10;
        bossClear = 0;
        inDungeon = false;
        isSceneLoading = false;
        bossSetting = false;
        isTraingPossible = true;
        dungeonClear = false;
        floorRepeat = false;
        phaseClear = false;
        freePassFloor = false;
    }
    public void DungeonInit()
    {
        isTraingPossible = true;
        bossSetting = false;
        dungeonClear = false;
        floorRepeat = false;

        ++currentDate;
        currentStage = 0;
        bossStageCount = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
    }

    public void Update()
    {
        if (canvasManager.GameMenuOnCheck()) return;
        if (useTeleportSystem == 10) return;
        if (isSceneLoading) return;

        if (Input.GetButtonDown("Fire1"))           // 공격키를 눌렀을 때
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    if (useTeleportSystem != 9)
                    {
                        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                        isSceneLoading = true;
                        canvasManager.FadeOutStart(true);
                    }
                    break;
                case 1:
                    if (useTeleportSystem != 10)
                    {
                        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                        isSceneLoading = true;
                        canvasManager.FadeOutStart(true);
                    }
                    break;
                case 2:
                case 3:
                    if (useTeleportSystem == 8)         // 던전 포탈 앞에 서있을 경우 다음던전 또는 집으로 이동한다.
                    {
                        if (!dungeonClear) return;
                        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;

                        if (!phaseClear)
                        {
                            if (usedKey)            // 키를 쓴경우
                            {
                                isSceneLoading = true;
                                canvasManager.FadeOutStart(false);
                            }
                            else                    // 키를 안쓴경우 인벤토리를 연다.
                            {
                                canvasManager.OpenInGameMenu(true);
                            }
                        }
                        else
                        {
                            if (usedKey)            // 키를 쓴경우
                            {
                                isSceneLoading = true;
                                canvasManager.FadeOutStart(true);
                            }
                            else                    // 키를 안쓴경우 인벤토리를 연다.
                            {
                                canvasManager.OpenInGameMenu(true);
                            }
                        }
                    }
                    break;
            }
        }
    }

    public void UseItemInDungeon(Item _item)
    {
        switch (_item.usingType)
        {
            case ItemUsingType.ReturnTown:
                // 마을로 돌아간다. 클리어 정보창 표시
                isSceneLoading = true;
                canvasManager.CircleFadeOutStart(true);
                break;
        }
    }

    public bool UseKeyInDungeon(Item _Item)
    {
        if (usedKey || phaseClear) return false;

        usedKey = true;
        // 키가 가진 것들을 가지고 체크
        switch (_Item.itemType)
        {
            case ItemType.Number:
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassNextFloor:
                freePassFloor = true;
                break;
            case ItemType.FreePassThisFloor:
                freePassThisFloor = true;
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

    public void SceneLoad()
    {
        if (!isDead)
        {
            switch (useTeleportSystem)
            {
                case 0:     // 집 현관 문
                    mainCamera.SetHeiWid(640, 360);
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 1:     // 마을로 향하는 문
                    mainCamera.SetHeiWid(1280, 720);
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 2:     // 탑으로 향하는 길
                    canvasManager.Menus[0].GetComponent<Menu_Inventory>().DeleteStorageItem();
                    SceneManager.LoadScene("Tower_First_Floor");
                    break;
                case 8:
                    if (phaseClear)
                    {
                        phaseClear = false;
                        SceneManager.LoadScene("Tower_First_Floor");
                    }
                    else
                    {
                        ReturnToTown();
                    }
                    break;
                case 9:
                    break;
            }
        }
        else
        {
            isDead = false;
            ReturnToTown();
        }
    }
    public void PlayerIsDead()
    {
        isDead = true;
        /*
         *  플레이어 죽었을 때 게임 오버 창 표시
         */
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(true);
        isSceneLoading = true;
        canvasManager.CircleFadeOutStart(true);
    }
    public void ReturnToTown()
    {
        /*
         *  정상적으로 복귀 시 게임 오버 창 표시
         */
        DungeonInit();
        playerStatus.ReturnToTown();
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(false);
        mainCamera.SetHeiWid(640, 360);
        SceneManager.LoadScene(0);
    }
    public bool NewDayCheck()
    {
        if (isTraingPossible)
        {
            isTraingPossible = false;
            return true;
        }
        return false;
    }

    // 층 이동 시 나타날 층 세팅
    public void FloorSetting()
    {
        phaseClear = false;
        dungeonClear = true;    //false 로 변경
        usedKey = false;
        freePassFloor = false;
        spawnerCount = 0;

        int dropItemPoolCount = dropItemPool.transform.childCount;

        for(int i = 0; i < dropItemPoolCount; ++i)
        {
            Destroy(dropItemPool.transform.GetChild(i).gameObject);
        }

        mapList = GameObject.FindGameObjectsWithTag("BaseMap");
        selectedMapNum = Random.Range(0, mapList.Length);

        for (int i = 0; i < 2; ++i)
        {
            if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
        }

        ++currentStage;
        // 다음층이 보스층인걸 미리 알면 순서 변경
        ++bossStageCount;
        
        // 다음 층 스킵
        if (freePassFloor)
        {
            FloorReset();

            ++currentStage;
            ++bossStageCount;
        }
        if (freePassThisFloor)
        {
            FloorReset();
        }

        // 보스 스테이지 설정
        if (!bossSetting)
        {
            // 이벤트 플래그로 구간별 보스 등장
            if ((bossStageCount - (5 * bossClear) - 2) > 0)  // 보스스테이지 설정
            {
                if (bossStageCount * 20 > Random.Range(50, 90))
                    bossSetting = true;
            }
        }

        if (bossSetting)    // 보스 층 일때
        {
            bossSetting = false;
            bossStageCount = 0;

            FloorReset();
            
            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            int randomBoss = Random.Range(0, bossMonsterPreFabsList.Length);

            monsterCount = 1;
            currentMonsterCount = monsterCount;
            currentStageMonsterList = new GameObject[currentMonsterCount];
            currentStageMonsterList[0] = Instantiate(bossMonsterPreFabsList[randomBoss], new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
        }
        else if (floorRepeat && !phaseClear)    // 맵 반복시
        {
            floorRepeat = false;
            currentMonsterCount = monsterCount;
            if(monsterCount > 0)
            {
                for (int i = 0; i < monsterCount; ++i)
                {
                    randomX = Random.Range(-1, 2);
                    currentStageMonsterList[i].GetComponent<NormalMonsterControl>().MonsterInit();
                    currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                             , spawner[Random.Range(0, spawnerCount)].transform.position.y);
                }
            }
        }
        else            // 일반 맵일경우
        {
            FloorReset();

            // 초기 맵 랜덤 선택
            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            monsterCount = FloorDatas[currentStage].SpawnAmount + marker_Variable.markerVariable[0];
            currentMonsterCount = monsterCount;

            currentStageMonsterList = new GameObject[monsterCount];

            int monsterPrefabListCount = monsterPreFabsList.Length;
            int randomSpawner = Random.Range(0, spawnerCount);
            // 몬스터 스폰
            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterPreFabsList[Random.Range(0, monsterPrefabListCount)]
                    , new Vector2(
                        spawner[randomSpawner].transform.position.x + randomX
                        , spawner[randomSpawner].transform.position.y)
                        , Quaternion.identity);
            }
        }

        if(currentStage < 2)
        {
            dungeonUI.SetDungeonFloor(currentStage, "");
        }
        else
        {
            dungeonUI.SetDungeonFloor(currentStage, SetFloorStatus());
        }

        markerRandom = Random.Range(0, 12);
        marker.thisMarker = (Markers)markerRandom;
        mark = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().teleporter.transform.GetChild(0).gameObject;
        //mark.GetComponent<SpriteRenderer>().sprite = markSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

        marker_Variable.markerPreVariable = marker_Variable.markerVariable;
        marker_Variable.Reset();

        player.transform.position = entrance;
        mapList[selectedMapNum].GetComponent<BackgroundScrolling>().SetBackGroundPosition(currentStage);
        
    }
    public void FloorReset()
    {
        for(int i = 0; i < monsterCount; ++i)
        {
            if(currentStageMonsterList[i] != null)
            {
                Destroy(currentStageMonsterList[i].gameObject);
            }
        }
        // 구조물 위치 초기화
    }

    // 아이템이 사용된 층에 효과를 적용
    public string SetFloorStatus()
    {
        playerStatus.PlayerStatusInit();

        string stageStatText = "";
        int monsterListCount = currentStageMonsterList.Length;
        int markerNumber = (int)marker.thisMarker;

        switch (marker.thisMarker)
        {
            case Markers.SetDamageBuffOnFloor_NF:
                playerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], true);
                MonsterAttackSetting(monsterListCount, marker_Variable.markerVariable[markerNumber]);
                stageStatText = "전체 공격력 증가";
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                MonsterAttackSetting(monsterListCount, marker_Variable.markerVariable[markerNumber]);
                stageStatText = "몬스터 공격력 증가";
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                playerStatus.SetAttackAdd_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "자신의 공격력 증가";
                break;
            case Markers.SetPosHPOnMonster_NF:
                MonsterHPSetting(monsterListCount, marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "몬스터 체력 증가";
                break;
            case Markers.SetNegHPOnMonster_NF:
                MonsterHPSetting(monsterListCount, marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "몬스터 체력 감소";
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "대시 거리 증가";
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "대시 거리 감소";
                break;
            case Markers.SetPosAttackSpeedOnPlayer_NF:
                playerStatus.SetAttackSpeedAdd_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "공격 속도 증가";
                break;
            case Markers.SetNegAttackSpeedOnPlayer_NF:
                playerStatus.SetAttackSpeedAdd_Result(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "공격 속도 감소";
                break;
            case Markers.SetMonster_NF:
                stageStatText = "몬스터 " + marker_Variable.markerVariable[markerNumber] + " 마리 추가";
                break;
            case Markers.SetDrop_NF:
                stageStatText = "드랍률 " + marker_Variable.markerVariable[markerNumber] + "% 증가";
                break;
            case Markers.SetSpecialMonster_NF:
                stageStatText = "엘리트 몬스터 " + marker_Variable.markerVariable[markerNumber] + " 마리 출현";
                break;
            default:
                stageStatText = "";
                break;
        }
        return stageStatText;
    }

    public void MonsterAttackSetting(int _monsterListCount, int _value)
    {
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_attack(_value);
        }
    }
    public void MonsterHPSetting(int _monsterListCount, int _value, bool upgrade)
    {
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_hp(_value, upgrade);
        }
    }

    public void FloorBossKill()
    {
        ++bossClear;
        phaseClear = true;
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
        mainCamera = CameraManager.instance;

        useTeleportSystem = 10;
        
        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");
        int teleportCount = teleportPoint.Length;

        if (SceneManager.GetActiveScene().buildIndex == 0)      // 메인 메뉴 씬 일 때
        {
            if (!GameManager.instance.gameStart)
            {
                Init();
                for (int i = 0; i < teleportCount; ++i)
                {
                    if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                        entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                }
            }
            else
            {
                for (int i = 0; i < teleportCount; ++i)
                {
                    if (teleportPoint[i].GetComponent<Teleport>().useSystem == 1)
                        entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                }
            }
            player.transform.position = entrance;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)  // 마을 화면 일 때
        {
            mapList = GameObject.FindGameObjectsWithTag("BaseMap");
            canvasManager.SetTownUI();
            for (int i = 0; i < teleportCount; ++i)
            {
                if (teleportPoint[i].GetComponent<Teleport>().useSystem == 0)
                    entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
            }
            player.transform.position = entrance;
            mapList[0].GetComponent<BackgroundScrolling>().SetBackGroundPosition(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)  // 던전 화면 일 때
        {
            inDungeon = true;
            dungeonUI = GameObject.Find("DungeonUI").GetComponent<Dungeon_UI>();
            dropItemPool = GameObject.Find("DropItemPool");
            FloorSetting();
        }

        mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());

        canvasManager.FadeInStart();
    }

    public int GetCurrentDate()
    {
        return currentDate;
    }
    public bool GetTrainigPossible()
    {
        return isTraingPossible;
    }
    public bool[] GetEventFlag()
    {
        return eventFlag;
    }

    public void LoadGamePlayDate(int _currentDate, bool _isTrainingPossible, bool[] _eventFlag)
    {
        currentDate = _currentDate;
        isTraingPossible = _isTrainingPossible;
        eventFlag = _eventFlag;
    }
}