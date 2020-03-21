using System.Collections;
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
    public GameObject player;
    public CanvasManager canvasManager;
    private PlayerStatus playerStatus;
    private GameObject[] teleportPoint;
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

    #region 던전 생성 관련
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
    private int currentMonsterCount;    // 현재 몬스터 수
    private int allKillCount;           // 총 몬스터 킬 수
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
            FloorDatas[floor - 1] = new FloorData(floor, bossClearCount, floor * 2);
        }
    }
    private void Init()
    {
        shopItemList = new Item[8];
        for (int i = 0; i < 8; ++i)
        {
            shopItemList[i] = null;
        }
        useTeleportSystem = 10;

        currentStage = 0;
        bossStageCount = 0;
        bossClearCount = 0;
        monsterCount = 0;
        currentMonsterCount = 0;

        isSceneLoading = false;
        inDungeon = false;
        dungeonClear = false;
        bossSetting = false;
        floorRepeat = false;
        freePassNextFloor = false;
        phaseClear = false;

        isTraingPossible = true;
        isShopRefill = true;
    }
    private void DungeonInit()
    {
        isDead = false;
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
        isTraingPossible = true;
        isShopRefill = true;
    }

    public void Update()
    {
        if (isSceneLoading) return;
        if (canvasManager.GameMenuOnCheck()) return;
        if (useTeleportSystem == 10) return;

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
                canvasManager.CircleFadeOutStart();
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
                break;
            case ItemType.FreePassThisFloor:                // 사용된 키가 이번 층 스킵일 때
                FloorSetting();
                break;
            case ItemType.SetBossFloor:                     // 사용된 키가 보스 소환일 때
                bossSetting = true;
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
                break;
            case ItemType.RepeatThisFloor:                   // 사용된 키가 층 반복일 때
                floorRepeat = true;
                break;
            case ItemType.ReturnTown:                        // 사용된 키가 귀환일 때
                isSceneLoading = true;
                canvasManager.CircleFadeOutStart();
                break;
            default:
                marker.ExecuteMarker(0);
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
                    canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(false);          // 집으로 돌아갈 때 창고에 키 넣기
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 1:     // 마을로 향하는 문
                    mainCamera.SetHeiWid(1280, 720);
                    canvasManager.Menus[0].GetComponent<Menu_Inventory>().DeleteStorageItem();      // 집에서 나올 때 창고에 선택된 키 삭제
                    SceneManager.LoadScene(useTeleportSystem);
                    break;
                case 2:     // 탑으로 향하는 길
                    SceneManager.LoadScene("Tower_First_Floor");
                    break;
                case 8:
                    if (phaseClear)
                    {
                        phaseClear = false;
                        SceneManager.LoadScene("Tower_First_Floor");
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
    public void PlayerIsDead()
    {
        isDead = true;
        Time.timeScale = 0.5f;
        mainCamera.SetHeiWid(640, 360);
        mainCamera.target.transform.position = player.transform.position;
        isSceneLoading = true;
        for (int i = 0; i < monsterCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterStop();
        }
        canvasManager.CircleFadeOutStart();
    }
    public void ReturnToTown()
    {
        for(int i = 0; i < 8; ++i)
        {
            shopItemList[i] = null;
        }
        FloorReset();
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(isDead);
        playerStatus.ReturnToTown();
        DungeonInit();
        mainCamera.SetHeiWid(640, 360);
        SceneManager.LoadScene(0);
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

    #region dungeon 관련
    // 층 이동 시 나타날 층 세팅
    public void FloorSetting()
    {
        float randomX;

        dungeonClear = false;
        usedKey = false;

        int dropItemPoolCount = dropItemPool.transform.childCount;
        for (int i = 0; i < dropItemPoolCount; ++i)
        {
            Destroy(dropItemPool.transform.GetChild(i).gameObject);
        }

        if (!floorRepeat)
            FloorReset();

        DungeonPoolManager.instance.bossMonsterCountReset();
        
        mapList = GameObject.FindGameObjectsWithTag("BaseMap");
        selectedMapNum = Random.Range(0, mapList.Length);

        for (int i = 0; i < 2; ++i)
        {
            if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
        }
        player.transform.position = entrance;
        mapList[selectedMapNum].GetComponent<BackgroundScrolling>().SetBackGroundPosition(currentStage);

        ++currentStage;
        ++bossStageCount;
        
        // 다음 층 스킵
        if (freePassNextFloor)
        {
            freePassNextFloor = false;
            ++currentStage;
            ++bossStageCount;
        }
        if (!bossSetting)
        {
            if ((bossStageCount - (5 * bossClearCount) - 2) > 0)  // 보스스테이지 설정
            {
                if (bossStageCount * 20 > Random.Range(50, 90))
                    bossSetting = true;
            }            // 이벤트 플래그로 구간별 보스 등장
        }           // 보스 스테이지 설정
        if (bossSetting)
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
        }            // 보스 층 일때
        else if (floorRepeat)
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
        }       // 맵 반복시
        else
        {
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
        }                        // 일반 맵일경우

        if (currentStage < 2)            canvasManager.dungeonUI.SetDungeonFloor(currentStage, "");
        else                             canvasManager.dungeonUI.SetDungeonFloor(currentStage, SetFloorStatus());
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
    public void FloorBossKill()
    {
        --currentMonsterCount;
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
        if(currentMonsterCount < 1)
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
                    Init();
                    for (int i = 0; i < teleportCount; ++i)
                    {
                        if (teleportPoint[i].GetComponent<Teleport>().useSystem == 9)
                            entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                    }
                }
                player.transform.position = entrance;
                break;
            case 1:
                mapList = GameObject.FindGameObjectsWithTag("BaseMap");
                canvasManager.SetTownUI();

                for (int i = 0; i < teleportCount; ++i)
                {
                    if (teleportPoint[i].GetComponent<Teleport>().useSystem == 0)
                        entrance = teleportPoint[i].GetComponent<Teleport>().transform.position;
                }
                mapList[0].GetComponent<BackgroundScrolling>().SetBackGroundPosition(0);
                player.transform.position = entrance;
                break;
            case 2:
                inDungeon = true;
                canvasManager.SetDungeonUI();
                dropItemPool = GameObject.Find("DropItemPool");
                FloorSetting();
                break;
            case 3:
                break;
        }
        mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
        StartCoroutine(MapMoveDelay());
    }
    public IEnumerator MapMoveDelay()
    {
        yield return new WaitForSeconds(0.4f);
        canvasManager.FadeInStart();        // 씬 로드 종료 후 페이드 인
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
    public bool GetShopRefill()
    {
        return isShopRefill;
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