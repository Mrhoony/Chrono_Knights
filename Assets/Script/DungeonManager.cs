using System.Collections;
using System.Collections.Generic;
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
    SetPosDamageOnPlayer_NF,
    SetNegDamageOnPlayer_NF
}
public enum MarkerVariableNumber
{
    MonsterModifier = 0,
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
            case Markers.SetDrop_NF:
                {
                    SetDrop_NF(keyValue);
                }
                break;
            case Markers.SetMonster_NF:
                {
                    SetMonster_NF(keyValue);
                }
                break;
            case Markers.SetNegDamageOnPlayer_NF:
                {
                    SetNegDamageOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    SetNegDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    SetNegHPOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetPosDamageOnPlayer_NF:
                {
                    SetPosDamageOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    SetPosDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    SetPosHPOnMonster_NF(keyValue);
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
    private CanvasManager menu;
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

    private bool newDay;
    public bool isDead;
    public bool freePassFloor;
    public int bossClear;
    public bool inDungeon;
    public bool[] eventFlag;
    
    #region 던전 생성 관련
    private GameObject[] mapList;
    public GameObject[] monsterList;
    public GameObject[] bossMonsterList;
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
        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
        markSprite = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");

        marker = new Marker();
        marker_Variable = new MarkerVariable();
        marker_Variable.Reset();

        FloorDatas = new FloorData[70];
        FloorDangerousSetting(0);
    }
    private void Init()
    {
        currentStage = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        bossStageCount = 0;
        useTeleportSystem = 10;
        bossClear = 0;
        isSceneLoading = false;
        bossSetting = false;
        newDay = false;
        dungeonClear = false;
        floorRepeat = false;
        phaseClear = false;
        freePassFloor = false;
    }
    private void FloorDangerousSetting(int plusDangerous)
    {
        for (int floor = 1; floor < 71; ++floor)
        {
            FloorDatas[floor - 1] = new FloorData(floor, bossClear, floor * 2);
        }
    }
    public void DungeonInit()
    {
        newDay = true;
        bossSetting = false;
        dungeonClear = false;
        floorRepeat = false;

        currentStage = 0;
        bossStageCount = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
    }

    public void Update()
    {
        if (menu.GameMenuOnCheck()) return;
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
                        menu.FadeOutStart(true);
                    }
                    break;
                case 1:
                    if (useTeleportSystem != 10)
                    {
                        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                        isSceneLoading = true;
                        menu.FadeOutStart(true);
                    }
                    break;
                case 2:
                case 3:
                    if (useTeleportSystem == 8)         // 던전 포탈 앞에 서있을 경우 다음던전 또는 집으로 이동한다.
                    {
                        if (!dungeonClear) return;
                        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;

                        if (usedKey)            // 키를 쓴경우
                        {
                            Debug.Log("맵이동");
                            isSceneLoading = true;
                            menu.FadeOutStart(false);
                        }
                        else                    // 키를 안쓴경우 인벤토리를 연다.
                        {
                            Debug.Log("아이템 사용");
                            menu.OpenInGameMenu(true);
                        }
                    }
                    break;
            }
        }
    }
    
    public bool useKeyInDungeon(Item _Item)
    {
        if (usedKey) return false;
        usedKey = true;
        // 키가 가진 것들을 가지고 체크
        switch (_Item.itemType)
        {
            case ItemType.Number:
                marker.ExecuteMarker(_Item.Value);
                break;
            case ItemType.ReturnTown:
                // 마을로 돌아간다. 클리어 정보창 표시
                isSceneLoading = true;
                menu.FadeOutStart(true);
                break;
            case ItemType.FreePassNextFloor:
                freePassFloor = true;
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

    public void SceneLoad()
    {
        if (!isDead)
        {
            switch (useTeleportSystem)
            {
                case 0:
                    mainCamera.SetHeiWid(640, 360);
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 1:
                    mainCamera.SetHeiWid(1280, 720);
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 2:
                    menu.Menus[0].GetComponent<Menu_Inventory>().DeleteStorageItem();
                    SceneManager.LoadScene("Tower_First_Floor");
                    break;
                case 3:
                    //SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 4:
                    //SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 8:
                    if (phaseClear)
                    {
                        phaseClear = false;
                        SceneManager.LoadScene("Tower_Second_Floor");
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
        menu.Menus[0].GetComponent<Menu_Inventory>().PutInBox(true);
        isSceneLoading = true;
        menu.FadeOutStart(true);
    }
    public void ReturnToTown()
    {
        DungeonInit();
        NewDayCheck();
        ++currentDate;
        playerStatus.ReturnToTown();
        menu.Menus[0].GetComponent<Menu_Inventory>().PutInBox(true);
        mainCamera.SetHeiWid(640, 360);
        SceneManager.LoadScene(0);
    }
    public bool NewDayCheck()
    {
        if (newDay)
        {
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

            int randomBoss = Random.Range(0, bossMonsterList.Length);

            currentStageMonsterList = new GameObject[1];
            currentStageMonsterList[0] = Instantiate(bossMonsterList[randomBoss], new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
        }
        else if (floorRepeat)    // 맵 반복시
        {
            floorRepeat = false;
            currentMonsterCount = monsterCount;

            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i].SetActive(true);
                currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y);
            }
        }
        else            // 일반 맵일경우
        {
            FloorReset();
            // 초기 맵 랜덤 선택
            /* (임시)하나만 클리어 해도 마을로
            if (currentStage > 0)
                phaseClear = true;
            */
            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            monsterCount = FloorDatas[currentStage].SpawnAmount * marker_Variable.markerVariable[0];
            currentMonsterCount = monsterCount;

            currentStageMonsterList = new GameObject[monsterCount];

            // 몬스터 스폰
            for (int i = 0; i < monsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterList[Random.Range(0, monsterList.Length)], new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
            }
        }
        SetFloorStatus();

        markerRandom = Random.Range(0, 12);
        marker.thisMarker = (Markers)markerRandom;
        mark = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().teleporter.transform.GetChild(0).gameObject;
        mark.GetComponent<SpriteRenderer>().sprite = markSprite[Random.Range(3, 5)]; // 텔레포터 마크를 바꿈

        marker_Variable.markerPreVariable = marker_Variable.markerVariable;
        marker_Variable.Reset();

        player.transform.position = entrance;
        mapList[selectedMapNum].GetComponent<BackgroundScrolling>().SetBackGroundPosition(currentStage);
        
        dungeonUI.SetDungeonFloor(currentStage);
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
    public void SetFloorStatus()
    {
        playerStatus.PlayerStatusUpdate();

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
            inDungeon = false;
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
            for (int i = 0; i < teleportCount; ++i)
            {
                if (teleportPoint[i].GetComponent<Teleport>().useSystem == 0)
                    entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
            }
            player.transform.position = entrance;
            mapList[0].GetComponent<BackgroundScrolling>().SetBackGroundPosition(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)  // 마을 - 숲 화면 일 때
        {
            inDungeon = true;
            dungeonUI = GameObject.Find("DungeonUI").GetComponent<Dungeon_UI>();
            FloorSetting();
        }

        mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());

        menu.FadeInStart();
    }

    public GameObject[] GetCurrentMonsterList()
    {
        return currentStageMonsterList;
    }
    public int GetCurrentDate()
    {
        return currentDate;
    }
    public bool[] GetEventFlag()
    {
        return eventFlag;
    }

    public void LoadGamePlayDate(int _currentDate, bool[] _eventFlag)
    {
        currentDate = _currentDate;
        eventFlag = _eventFlag;
    }
}