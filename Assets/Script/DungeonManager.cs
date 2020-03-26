﻿using System.Collections;
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
    SetPosAttackMulty_NF,
    SetNegAttackMulty_NF,
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
            case Markers.SetPosAttackMulty_NF:
                {
                    SetPosAttackMulty_NF(keyValue);
                }
                break;
            case Markers.SetNegAttackMulty_NF:
                {
                    SetNegAttackMulty_NF(keyValue);
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
    private void SetPosAttackMulty_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[10] = keyValue;
    }
    private void SetNegAttackMulty_NF(int keyValue)
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
        markerVariable[(int)Markers.SetPosAttackMulty_NF] = 1;
        markerVariable[(int)Markers.SetNegAttackMulty_NF] = 1;
    }
}
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
    public GameObject player;
    public CanvasManager canvasManager;
    public PlayerStatus playerStatus;
    public GameObject[] teleportPoint;
    #endregion
    #region dungeon
    public Marker marker;
    public MarkerVariable marker_Variable;   // Marker로부터 전달받는 값 저장공간
    public FloorData[] FloorDatas;

    private Vector2 entrance;               // 텔레포트 위치
    public int useTeleportSystem;           // 텔레포트 사용 방법 0~4 입구, 10 사용 안함
    public int currentDate;                 // 현재 날짜
    public bool isDead;                     // 플레이어 죽었는지
    public bool usedKey;                    // 키를 사용했는지
    public bool inDungeon;                  // 플레이어가 던전 입장 시 true 나갈 시 false
    public bool isReturn;                   // 죽던 클리어 하던 나갈 시 true 입장 시 false
    #endregion
    
    private bool isTraingPossible;          // 트레이닝 가능 여부
    private bool isShopRefill;              // 샵 리필 여부

    private Item[] shopItemList;
    private int[] itemCostList;
    private bool[] eventFlag;

    public float dungeonPlayTimer;

    #region 던전 생성 관련 변수
    private GameObject[] mapList;
    private GameObject[] monsterPreFabsList;
    private GameObject[] bossMonsterPreFabsList;
    private GameObject[] currentStageMonsterList;
    private GameObject[] spawner;
    private GameObject dropItemPool;

    private int selectedMapNum = 0;
    private int spawnerCount;
    private int spawn;

    public int currentStage;
    public int bossStageCount;
    public bool freePassNextFloor;          // 다음층 스킵 체크
    public bool bossSetting;                // 보스층 등장 체크
    public bool floorRepeat;                // 층 반복 체크
    public int bossClearCount;
    public bool dungeonClear;           // 던전 클리어시
    public bool phaseClear;             // 페이즈 클리어시
    public bool isSceneLoading;         // 페이드 아웃 중 입력 제한

    private int monsterCount;           // 최대 몬스터 수
    private int eliteMonsterCount;
    private int currentMonsterCount;    // 현재 몬스터 수
    private int allKillCount;           // 총 몬스터 킬 수
    private int monsterKillCount;
    private int eliteMonsterKillCount;
    private int bossMonsterKillCount;
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

        monsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/Monster");
        bossMonsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/BossMonster");

        marker = new Marker();
        marker_Variable = new MarkerVariable();

        FloorDatas = new FloorData[70];
        shopItemList = new Item[8];

        Init();
    }
    private void Init()
    {
        marker_Variable.Reset();
        FloorDangerousSetting(0);

        useTeleportSystem = 10;
        
        isSceneLoading = false;
        inDungeon = false;

        DungeonInit();
    }
    private void DungeonInit()
    {
        isDead = false;
        isReturn = false;
        bossSetting = false;
        dungeonClear = false;
        floorRepeat = false;
        phaseClear = false;
        freePassNextFloor = false;

        currentStage = 0;
        bossStageCount = 0;
        bossClearCount = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
        ++currentDate;

        for (int i = 0; i < 8; ++i)
        {
            shopItemList[i] = null;
        }
        isShopRefill = true;
        isTraingPossible = true;
    }

    public void Update()
    {
        if (inDungeon) dungeonPlayTimer += Time.deltaTime;

        if (isSceneLoading) return;
        if (canvasManager.GameMenuOnCheck()) return;

        if (Input.GetButtonDown("Fire1"))           // 공격키를 눌렀을 때
        {
            if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
            if (useTeleportSystem == 10) return;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    if (useTeleportSystem != 9)
                    {
                        isSceneLoading = true;
                        canvasManager.FadeOutStart(true);
                    }
                    break;
                case 1:
                    isSceneLoading = true;
                    canvasManager.FadeOutStart(true);
                    break;
                case 2:
                case 3:
                    if (useTeleportSystem == 8)         // 던전 포탈 앞에 서있을 경우 다음던전 또는 집으로 이동한다.
                    {
                        if (!dungeonClear) return;

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
                OutDungeon();
                break;
        }
    }       // 아이템 사용시 아이템이 가지고있는 버프 적용
    public bool UseKeyInDungeon(Item _Item)
    {
        if (usedKey) return false;
        usedKey = true;

        // 키가 가진 것들을 가지고 체크
        switch (_Item.itemType)
        {
            case ItemType.Number:                           // 사용된 키가 숫자키일 때
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassNextFloor:                // 사용된 키가 다음 층 스킵일 때
                freePassNextFloor = true;
                MarkerSetting();
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassThisFloor:                // 사용된 키가 이번 층 스킵일 때
                FloorSetting();
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.SetBossFloor:                     // 사용된 키가 보스 소환일 때
                bossSetting = true;
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.ReturnPreFloor:                   // 사용된 키가 이전 층 효과일 때
                if (currentStage > 1)
                {
                    marker_Variable.markerVariable = marker_Variable.markerPreVariable;
                }                    // 2층 이상일때만
                else
                {
                    marker_Variable.Reset();
                }                                     // 1층일 땐 초기화
                marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.RepeatThisFloor:                   // 사용된 키가 층 반복일 때
                floorRepeat = true;
                break;
            case ItemType.ReturnTown:                        // 사용된 키가 귀환일 때
                OutDungeon();
                break;
            default:
                marker.ExecuteMarker(0);
                break;
        }
        return true;
    }
    public void PlayerIsDead()
    {
        if (isDead) return;
        isDead = true;

        Time.timeScale = 0.5f;
        mainCamera.SetHeiWid(640, 360);
        mainCamera.target.transform.position = player.transform.position;
        for (int i = 0; i < monsterCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterStop();
        }
        OutDungeon();
    }
    public void OutDungeon()
    {
        isReturn = true;
        inDungeon = false;
        isSceneLoading = true;
        canvasManager.CircleFadeOutStart();
    }
    public void OpenGameOverResult()
    {
        canvasManager.OpenGameOverMenu(dungeonPlayTimer);
    }
    public void SceneLoad()
    {
        if (!isDead)
        {
            switch (useTeleportSystem)
            {
                case 0:     // 집 현관 문
                    ReturnHome();
                    break;
                case 1:     // 마을로 향하는 문
                    mainCamera.SetHeiWid(1280, 720);
                    canvasManager.Menus[0].GetComponent<Menu_Inventory>().DeleteStorageItem();      // 집에서 나올 때 창고에 선택된 키 삭제
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 2:     // 탑으로 향하는 길
                    SceneManager.LoadScene(2);
                    break;
                case 8:
                    if (phaseClear)
                    {
                        phaseClear = false;
                        SceneManager.LoadScene(2);
                    }
                    break;
                default:
                    ReturnToTown();
                    break;
            }
        }
        else
        {
            ReturnToTown();
        }
    }
    public void ReturnToTown()
    {
        DungeonInit();
        FloorReset();
        playerStatus.ReturnToTown();
        ReturnHome();
    }
    public void ReturnHome()
    {
        mainCamera.SetHeiWid(640, 360);
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(isDead);          // 집으로 돌아갈 때 창고에 키 넣기
        SceneManager.LoadScene(0);
    }

    #region dungeon 관련
    // 층 이동 시 나타날 층 세팅
    private void FloorDangerousSetting(int plusDangerous)
    {
        for (int floor = 1; floor < 71; ++floor)
        {
            FloorDatas[floor - 1] = new FloorData(floor, bossClearCount, floor * 2);
        }
    }
    public void FloorSetting()
    {
        float randomX;

        FloorReset();

        ++currentStage;
        ++bossStageCount;
        // 다음 층 스킵
        if (freePassNextFloor)
        {
            freePassNextFloor = false;
            ++currentStage;
            ++bossStageCount;
        }

        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");
        int teleportCount = teleportPoint.Length;

        MapEntranceFind(teleportCount, 9);

        selectedMapNum = Random.Range(0, mapList.Length);
        mapList[selectedMapNum].GetComponent<BackgroundScrolling>().SetBackGroundPosition(currentStage);
        
        if (!bossSetting)
        {
            if ((bossStageCount - (5 * bossClearCount) - 2) > 0)  // 보스스테이지 설정
            {
                if (bossStageCount * 20 > Random.Range(50, 90))
                    bossSetting = true;
            }            // 이벤트 플래그로 구간별 보스 등장
        }           // 보스 스테이지 설정

        if (bossSetting)        // 보스층 일 때
        {
            bossSetting = false;
            bossStageCount = 0;
            
            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            int randomBoss = Random.Range(0, bossMonsterPreFabsList.Length);

            monsterCount = 1;
            currentMonsterCount = monsterCount;
            currentStageMonsterList = new GameObject[currentMonsterCount];
            currentStageMonsterList[0] = Instantiate(bossMonsterPreFabsList[randomBoss], new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x
                                                         , spawner[Random.Range(0, spawnerCount)].transform.position.y), Quaternion.identity);
            currentStageMonsterList[0].GetComponent<BossMonster_Control>().monsterDeadCount = FloorBossKill;
        }                       // 보스층 일 때
        else if (floorRepeat)                     // 반복 키 썼을 때
        {
            Debug.Log("WHERE ::: " + monsterCount + " ABS : " + currentStageMonsterList.Length);
            floorRepeat = false;
            currentMonsterCount = monsterCount;
            
            if(monsterCount > 0)
            {
                for (int i = 0; i < monsterCount; ++i)
                {
                    randomX = Random.Range(-1, 2);
                    currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterInit();
                    currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                             , spawner[Random.Range(0, spawnerCount)].transform.position.y);
                }
            }
        }                  // 맵 반복시
        else
        {
            spawner = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().spawner;
            spawnerCount = spawner.Length;

            eliteMonsterCount = marker_Variable.markerVariable[(int)Markers.SetSpecialMonster_NF];
            monsterCount = FloorDatas[currentStage].SpawnAmount + marker_Variable.markerVariable[(int)Markers.SetMonster_NF] + eliteMonsterCount;
            currentMonsterCount = monsterCount;

            currentStageMonsterList = new GameObject[monsterCount];

            int monsterPrefabListCount = monsterPreFabsList.Length;
            int randomSpawner = Random.Range(0, spawnerCount);

            // 몬스터 스폰
            for(int i = 0; i < currentMonsterCount - eliteMonsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterPreFabsList[Random.Range(0, monsterPrefabListCount)]
                    , new Vector2(
                        spawner[randomSpawner].transform.position.x + randomX,
                        spawner[randomSpawner].transform.position.y),
                        Quaternion.identity);
                currentStageMonsterList[i].GetComponent<NormalMonsterControl>().monsterDeadCount = FloorMonsterKill;
            }
            for(int j = currentMonsterCount - eliteMonsterCount; j < currentMonsterCount; ++j)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[j] = Instantiate(monsterPreFabsList[Random.Range(0, monsterPrefabListCount)]
                    , new Vector2(
                        spawner[randomSpawner].transform.position.x + randomX,
                        spawner[randomSpawner].transform.position.y),
                        Quaternion.identity);
                // 엘리트 몬스터 강화
                currentStageMonsterList[j].GetComponent<NormalMonsterControl>().monsterDeadCount = FloorEliteMonsterKill;
            }
        }                                   // 일반 맵일경우
        
        if (currentStage < 2) canvasManager.dungeonUI.SetDungeonFloor(currentStage, "");
        else
        {
            canvasManager.dungeonUI.SetDungeonFloor(currentStage, SetFloorStatus());
            //보스 층 클리어 한 양만큼 방어력 / 공격력 증가
            MonsterAttackSetting(currentMonsterCount, bossClearCount * 1);
            MonsterDefSetting(currentMonsterCount, bossClearCount * 1);
        }

        MarkerSetting();
    }
    public void FloorReset()
    {
        dungeonClear = false;
        usedKey = false;

        DungeonPoolManager.instance.bossMonsterCountReset();

        if (floorRepeat) return;

        for (int i = 0; i < monsterCount; ++i)
        {
            if(currentStageMonsterList[i] != null)
            {
                Destroy(currentStageMonsterList[i].gameObject);
            }
        }       // 몬스터 리스트 초기화

        int dropItemPoolCount = dropItemPool.transform.childCount;      // 드랍된 아이템 일시 제거
        for (int i = 0; i < dropItemPoolCount; ++i)
        {
            Destroy(dropItemPool.transform.GetChild(i).gameObject);
        }
        // 구조물 위치 초기화 함수 추가
    }

    public void FloorBossKill()
    {
        --currentMonsterCount;
        ++allKillCount;
        ++bossMonsterKillCount;
        if (currentMonsterCount < 1)
        {
            dungeonClear = true;
            phaseClear = true;
            ++bossClearCount;
        }
    }
    public void FloorMonsterKill()
    {
        --currentMonsterCount;
        ++allKillCount;
        ++monsterKillCount;
        if (currentMonsterCount < 1)
        {
            dungeonClear = true;
        }
        Debug.Log("monster kill");
    }
    public void FloorEliteMonsterKill()
    {
        --currentMonsterCount;
        ++allKillCount;
        ++eliteMonsterKillCount;
        if (currentMonsterCount < 1)
        {
            dungeonClear = true;
        }
    }
    
    public void MarkerSetting()
    {
        int markerRandom = Random.Range(0, 12);
        marker.thisMarker = (Markers)markerRandom;
        GameObject mark = mapList[selectedMapNum].GetComponent<BackgroundScrolling>().teleporter.transform.GetChild(0).gameObject;
        mark.GetComponent<DungeonMarker>().SetMarker((Markers)markerRandom);

        marker_Variable.markerPreVariable = marker_Variable.markerVariable;
        marker_Variable.Reset();
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
                stageStatText = "전체 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                MonsterAttackSetting(monsterListCount, marker_Variable.markerVariable[markerNumber]);
                stageStatText = "몬스터 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                playerStatus.SetAttackAdd_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "자신의 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetPosHPOnMonster_NF:
                MonsterHPSetting(monsterListCount, marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "몬스터 체력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetNegHPOnMonster_NF:
                MonsterHPSetting(monsterListCount, marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "몬스터 체력 " + marker_Variable.markerVariable[markerNumber] + " 감소";
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "대시 거리 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                playerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "대시 거리 " + marker_Variable.markerVariable[markerNumber] + " 감소";
                break;
            case Markers.SetPosAttackMulty_NF:
                playerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "공격력 " + marker_Variable.markerVariable[markerNumber] + "배 증가";
                break;
            case Markers.SetNegAttackMulty_NF:
                playerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "공격력 " + marker_Variable.markerVariable[markerNumber] + "배 감소";
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
        if (_value == 0) return;
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_attack(_value);
        }
    }
    public void MonsterHPSetting(int _monsterListCount, int _value, bool upgrade)
    {
        if (_value == 0) return;
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_hp(_value, upgrade);
        }
    }
    public void MonsterDefSetting(int _monsterListCount, int _value)
    {
        if (_value == 0) return;
        for (int i = 0; i < _monsterListCount; ++i) {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_def(_value);
        }
    }
    #endregion

    // 씬 이동 후 초기화
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)                  // 씬 로드시 초기화
    {
        useTeleportSystem = 10;

        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");
        int teleportCount = teleportPoint.Length;

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                if (GameManager.instance.gameStart || isReturn)
                {
                    isReturn = false;
                    for (int i = 0; i < teleportCount; ++i)
                    {
                        if (teleportPoint[i].GetComponent<Teleport>().useSystem == 1)
                            entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                    }
                }
                else
                {
                    for (int i = 0; i < teleportCount; ++i)
                    {
                        if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                            entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                    }
                }
                player.transform.position = entrance;
                break;
            case 1:
                canvasManager.SetTownUI();
                MapEntranceFind(teleportCount, 0);
                mapList[0].GetComponent<BackgroundScrolling>().SetBackGroundPosition(-1);
                break;
            case 2:
                inDungeon = true;
                canvasManager.SetDungeonUI();
                dungeonPlayTimer = 0;
                dropItemPool = GameObject.Find("DropItemPool");
                FloorSetting();
                break;
            case 3:
                break;
        }
        mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
        StartCoroutine(MapMoveDelay());
    }

    public void MapEntranceFind(int _teleportCount, int _useSystem)
    {
        mapList = GameObject.FindGameObjectsWithTag("BaseMap");
        for (int i = 0; i < _teleportCount; ++i)
        {
            if (teleportPoint[i].GetComponent<Teleport>().useSystem == _useSystem)
                entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
        }
        player.transform.position = entrance;
    }

    public IEnumerator MapMoveDelay()
    {
        yield return new WaitForSeconds(0.7f);
        canvasManager.FadeInStart();        // 씬 로드 종료 후 페이드 인
    }

    public void SetShopItemList(Item[] _itemList, int[] _itemCost)
    {
        shopItemList = _itemList;
        itemCostList = _itemCost;
    }
    public Item[] GetShopItemList()
    {
        return shopItemList;
    }
    public int[] GetShopitemCostList()
    {
        return itemCostList;
    }

    #region save, load
    public int GetCurrentDate()
    {
        return currentDate;
    }
    public bool GetTrainigPossible()
    {
        return isTraingPossible;
    }
    public void setTrainigPossible(bool _isTraining)
    {
        isTraingPossible = _isTraining;
    }
    public bool GetShopRefill()
    {
        return isShopRefill;
    }
    public void setShopRefill(bool _isShop)
    {
        isShopRefill = _isShop;
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
    #endregion
}