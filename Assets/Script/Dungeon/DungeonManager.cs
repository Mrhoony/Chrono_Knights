using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    #region 오브젝트 등록
    public static DungeonManager instance;

    public ScenarioManager scenarioManager;
    public DungeonMaker dungeonMaker;
    public CameraManager mainCamera;
    public CanvasManager canvasManager;
    public GameObject player;
    public PlayerStatus playerStatus;

    public GameObject backgroundSet;
    private GameObject[] mapList;
    #endregion

    public bool isDead;                     // 플레이어 죽었는지
    private Vector2 entrance;               // 텔레포트 위치
    public int useObjectNumber;           // 텔레포트 사용 방법 0~4 입구, 10 사용 안함
    public int currentDate;                 // 현재 날짜
    public bool isSceneLoading;         // 페이드 아웃 중 입력 제한
    public bool inDungeon;                  // 플레이어가 던전 입장 시 true 나갈 시 false
    public bool usedKey;                    // 키를 사용했는지
    public bool isReturn;                   // 죽던 클리어 하던 나갈 시 true 입장 시 false
    public bool dungeonClear;           // 던전 클리어시
    public bool phaseClear;             // 페이즈 클리어시

    public bool sceneMove;
    public Teleport teleportDestination;

    private bool isTraingPossible;          // 트레이닝 가능 여부
    private bool isShopRefill;              // 샵 리필 여부
    
    private Item[] shopItemList;
    private int[] itemCostList;

    public float dungeonPlayTimer;

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
    }

    public void DungeonManagerInit()
    {
        dungeonMaker.DungeonMakerInit();
        useObjectNumber = 99;

        isSceneLoading = false;

        EventFlagReset();
        shopItemList = new Item[8];
        DungeonEscape();
        isReturn = false;
    }
    public void EventFlagReset()
    {
        scenarioManager.EventReset();
    }
    public void DungeonEscape()
    {
        isDead = false;
        isReturn = false;
        useObjectNumber = 99;
        inDungeon = false;
        DungeonFlagReset();

        ++currentDate;

        for (int i = 0; i < 8; ++i)
        {
            shopItemList[i] = null;
        }
        isShopRefill = true;
        isTraingPossible = true;
    }
    public void DungeonFlagReset()
    {
        dungeonClear = false;
        usedKey = false;
    }

    public void Update()
    {
        if (isSceneLoading) return;
        if (inDungeon) dungeonPlayTimer += Time.deltaTime;
    }

    public void UseItemInDungeon(Item _Item)
    {
        switch (_Item.usingType)
        {
            case ItemUsingType.FreePassThisFloor:                // 사용된 키가 이번 층 스킵일 때
                dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
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
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassNextFloor:                // 사용된 키가 다음 층 스킵일 때
                dungeonMaker.freePassNextFloor = true;
                dungeonMaker.MarkerReset();
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.SetBossFloor:                     // 사용된 키가 보스 소환일 때
                dungeonMaker.bossSetting = true;
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.ReturnPreFloor:                   // 사용된 키가 이전 층 효과일 때
                if (dungeonMaker.currentStage > 1)
                {
                    dungeonMaker.marker_Variable.markerVariable = dungeonMaker.marker_Variable.markerPreVariable;
                }                    // 2층 이상일때만
                else
                {
                    dungeonMaker.marker_Variable.Reset();
                }                                     // 1층일 땐 초기화
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.RepeatThisFloor:                   // 사용된 키가 층 반복일 때
                dungeonMaker.floorRepeat = true;
                break;
            case ItemType.ReturnTown:                        // 사용된 키가 귀환일 때
                OutDungeon();
                break;
            default:
                dungeonMaker.marker.ExecuteMarker(0);
                break;
        }
        return true;
    }
    
    public void EnterNextFloor()
    {
        DungeonFlagReset();
        dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
    }
    public void PlayerIsDead()
    {
        if (isDead) return;
        isDead = true;

        Time.timeScale = 0.5f;
        mainCamera.CameraSizeSetting(1);
        mainCamera.target.transform.position = player.transform.position;
        OutDungeon();
    }
    public void OutDungeon()
    {
        isReturn = true;
        isSceneLoading = true;
        Invoke("CircleFadeOutStart", 0.5f);
    }
    public void CircleFadeOutStart()
    {
        canvasManager.CircleFadeOutStart();
    }
    public void OpenGameOverResult()
    {
        canvasManager.OpenGameOverMenu(dungeonPlayTimer);
    }

    public void ActiveInteractiveTeleport(int _DestinationSceneNumber) // 텔레포트에 따른 씬이동 및 지역이동
    {
        useObjectNumber = _DestinationSceneNumber;
        sceneMove = true;

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                if (useObjectNumber == 99)
                {
                    GameManager.instance.SleepGame();
                }
                else
                {
                    isSceneLoading = true;
                    canvasManager.fadeInStartMethod += SceneLoad;
                    canvasManager.FadeOutStart();
                }
                break;
            case 1:
                isSceneLoading = true;
                canvasManager.fadeInStartMethod += SceneLoad;
                canvasManager.FadeOutStart();
                break;
            case 2:
            case 3:
                if (!dungeonClear || isReturn) return;

                if (usedKey)
                {
                    usedKey = false;
                    isSceneLoading = true;
                    if (phaseClear) canvasManager.fadeInStartMethod += SceneLoad;
                    else canvasManager.fadeInStartMethod += TeleportTransfer;
                    canvasManager.FadeOutStart();
                    break;
                }

                // 키를 안쓴경우 인벤토리를 연다.
                canvasManager.OpenInGameMenu(true);
                break;
        }
    }
    public void ActiveInteractiveTeleport(int _ObjectNumber, Teleport _Teleport) // 텔레포트에 따른 씬이동 및 지역이동
    {
        useObjectNumber = _ObjectNumber;
        sceneMove = false;

        isSceneLoading = true;
        teleportDestination = _Teleport;
        canvasManager.fadeInStartMethod += TeleportTransfer;
        canvasManager.FadeOutStart();
    }
    public void ActiveInteractiveObject(InteractiveObjectType _ObjectType, int _ObjectNumber) // 오브젝트에 따라 대사 찾기
    {
    }
    public void ActiveInteractiveNPC(NPC_Control _NPC)  // NPC 코드에 따라 대화 찾기, 창열기
    {
        if (PlayerControl.instance.actionState != ActionState.Idle) return;

        switch (_NPC.objectNumber)
        {
            case 101:
            case 102:
            case 103:
                scenarioManager.ScenarioRepeatCheck(_NPC);
                break;
            default:
                _NPC.OpenNPCUI();
                break;
        }
    }
    public void SceneLoad()
    {
        if (!isDead)
        {
            switch (useObjectNumber)
            {
                case 0:     // 집 현관 문
                    mainCamera.CameraSizeSetting(1);
                    ReturnHome();
                    break;
                case 1:     // 마을로 향하는 문
                    mainCamera.CameraSizeSetting(2);
                    SceneManager.LoadScene(1);
                    break;
                case 2:     // 탑으로 향하는 길
                    SceneManager.LoadScene(2);
                    break;
                case 999:   // 탑 내부 텔레포트
                    if (isReturn)
                    {
                        ReturnToTown();
                        break;
                    }

                    SceneManager.LoadScene(2);
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
    public void TeleportTransfer()
    {
        switch (useObjectNumber)
        {
            case 999:
                dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
                break;
            default:
                if (sceneMove)
                {
                    dungeonMaker.EnterTheDungeon();
                    dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
                }
                else
                {
                    mainCamera.SetCameraBound(teleportDestination.currentMap);
                    player.transform.position = teleportDestination.gameObject.transform.position;
                }
                break;
        }

        useObjectNumber = 99;
        StartCoroutine(MapMoveDelay());
    }
    public void ReturnToTown()
    {
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(isDead);          // 집으로 돌아갈 때 창고에 키 넣기
        DungeonEscape();
        dungeonMaker.FloorReset();
        playerStatus.ReturnToTown();
        ReturnHome();
    }
    public void ReturnHome()
    {
        mainCamera.CameraSizeSetting(1);
        SceneManager.LoadScene(0);
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
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)                  // 씬 로드시 초기화
    {
        mapList = GameObject.FindGameObjectsWithTag("BaseMap");
        backgroundSet = GameObject.Find("BackGroundSet");

        GameObject[] teleportPoint;
        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                backgroundSet = GameObject.Find("Base");
                mainCamera.SetCameraBound(backgroundSet);
                MapEntranceFind(teleportPoint, useObjectNumber);
                break;
            case 1:
                backgroundSet.GetComponent<BackgroundScrolling>().SetBackGroundPosition(entrance, -1);

                backgroundSet = GameObject.Find("Town");
                mainCamera.SetCameraBound(backgroundSet);
                MapEntranceFind(teleportPoint, useObjectNumber);

                canvasManager.SetTownUI();

                StartCoroutine(MapMoveDialogDelay("Tutorial"));
                break;
            case 2:
            case 3:
                if (!inDungeon)
                {
                    inDungeon = true;
                    dungeonPlayTimer = 0f;
                    canvasManager.SetDungeonUI();
                    TeleportTransfer();
                    StartCoroutine(MapMoveDialogDelay("FirstContact"));
                    break;
                }

                if (phaseClear)
                {
                    phaseClear = false;
                    dungeonMaker.PhaseClear();
                    canvasManager.SetDungeonUI();
                    TeleportTransfer();
                }
                break;
        }

        useObjectNumber = 99;
        StartCoroutine(MapMoveDelay());
    }
    public void MapEntranceFind(GameObject[] _TeleportPoint, int _useSystem)
    {
        int _TeleportCount = _TeleportPoint.Length;
        for (int i = 0; i < _TeleportCount; ++i)
        {
            if (_TeleportPoint[i].GetComponent<Teleport>().objectNumber == _useSystem)
                entrance = _TeleportPoint[i].GetComponent<Teleport>().transform.position;
        }
        player.transform.position = entrance;
    }

    public IEnumerator MapMoveDialogDelay(string _EventName)
    {
        yield return new WaitForSeconds(1f);
        scenarioManager.ScenarioCheck(_EventName);
    }
    public IEnumerator MapMoveDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canvasManager.FadeInStart();        // 씬 로드 종료 후 페이드 인
    }

    #region save, load

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

    public int GetCurrentDate()
    {
        return currentDate;
    }
    public int GetStoryProgress()
    {
        return scenarioManager.GetStoryProgress();
    }
    public bool[] GetEventFlag()
    {
        return scenarioManager.GetEventFlag();
    }
    public void LoadGamePlayDate(int _currentDate, bool _isTrainingPossible, bool[] _EventFlag, int _StoryProgress)
    {
        currentDate = _currentDate;
        isTraingPossible = _isTrainingPossible;
        scenarioManager.LoadGamePlayData(_StoryProgress, _EventFlag);
    }

    #endregion
}