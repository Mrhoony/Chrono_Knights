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
    public int useTeleportSystem;           // 텔레포트 사용 방법 0~4 입구, 10 사용 안함
    public int currentDate;                 // 현재 날짜
    public bool isSceneLoading;         // 페이드 아웃 중 입력 제한
    public bool inDungeon;                  // 플레이어가 던전 입장 시 true 나갈 시 false
    public bool usedKey;                    // 키를 사용했는지
    public bool isReturn;                   // 죽던 클리어 하던 나갈 시 true 입장 시 false
    public bool dungeonClear;           // 던전 클리어시
    public bool phaseClear;             // 페이즈 클리어시

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
        
        dungeonMaker.DungeonMakerInit();
        useTeleportSystem = 10;

        isSceneLoading = false;

        EventFlagReset();
        shopItemList = new Item[8];
        DungeonEscape();
    }
    public void EventFlagReset()
    {
        scenarioManager.EventReset();
    }
    private void DungeonEscape()
    {
        isDead = false;
        inDungeon = false;
        dungeonClear = false;
        phaseClear = false;
        usedKey = false;

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
        if (isSceneLoading) return;

        if (inDungeon) dungeonPlayTimer += Time.deltaTime;
        
        /*
        if (Input.GetButtonDown("Fire1"))           // 공격키를 눌렀을 때
        {
        }
        */
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
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassNextFloor:                // 사용된 키가 다음 층 스킵일 때
                dungeonMaker.freePassNextFloor = true;
                dungeonMaker.marker.ExecuteMarker(_Item.value);
                break;
            case ItemType.FreePassThisFloor:                // 사용된 키가 이번 층 스킵일 때
                dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
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

    public void EnterTheDungeon()
    {
        dungeonMaker.EnterTheDungeon();
        dungeonMaker.FloorSetting(mapList, player, mainCamera, backgroundSet);
    }
    public void PlayerIsDead()
    {
        if (isDead) return;
        isDead = true;

        Time.timeScale = 0.5f;
        mainCamera.SetHeiWid(640, 360);
        mainCamera.target.transform.position = player.transform.position;
        OutDungeon();
    }
    public void OutDungeon()
    {
        isReturn = true;
        isSceneLoading = true;
        canvasManager.CircleFadeOutStart();
    }
    public void OpenGameOverResult()
    {
        canvasManager.OpenGameOverMenu(dungeonPlayTimer);
    }
    
    public void ActiveInteractiveObject(bool _IsNPCCheck, int _ObjectNumber)
    {
        if (_IsNPCCheck)
        {
            switch (_ObjectNumber)
            {
                case 101:
                    break;
                case 102:
                    break;
                case 103:
                    break;
                default:
                    break;
            }
        }
        else
        {
            useTeleportSystem = _ObjectNumber;
            if (useTeleportSystem == 10) return;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    if (useTeleportSystem != 9)
                    {
                        isSceneLoading = true;
                        canvasManager.FadeOutStart(true);
                    }
                    else
                    {
                        GameManager.instance.GameStart();
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
                        if (!dungeonClear || isReturn) return;

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
                    SceneManager.LoadScene(1);
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
                    else if (isReturn)
                    {
                        ReturnToTown();
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
        canvasManager.Menus[0].GetComponent<Menu_Inventory>().PutInBox(isDead);          // 집으로 돌아갈 때 창고에 키 넣기
        DungeonEscape();
        dungeonMaker.FloorReset();
        playerStatus.ReturnToTown();
        ReturnHome();
    }
    public void ReturnHome()
    {
        mainCamera.SetHeiWid(640, 360);
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
        useTeleportSystem = 10;
        mapList = GameObject.FindGameObjectsWithTag("BaseMap");
        
        GameObject[] teleportPoint;
        teleportPoint = GameObject.FindGameObjectsWithTag("Portal");

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
                if (GameManager.instance.gameStart && !isReturn)
                {
                    isReturn = false;
                    MapEntranceFind(teleportPoint, 1);
                }
                else
                {
                    MapEntranceFind(teleportPoint, 9);
                }
                scenarioManager.ScenarioCheck("Tutorial");
                break;
            case 1:
                mainCamera.SetCameraBound(GameObject.Find("BackGround").GetComponent<BoxCollider2D>());
                canvasManager.SetTownUI();

                MapEntranceFind(teleportPoint, 0);

                mapList[0].GetComponent<BackgroundScrolling>().SetBackGroundPosition(entrance, -1);
                scenarioManager.ScenarioCheck("FirstContact");
                break;
            case 2:
            case 3:
                inDungeon = true;
                dungeonPlayTimer = 0f;
                canvasManager.SetDungeonUI();
                backgroundSet = GameObject.Find("BackGroundSet");
                EnterTheDungeon();
                break;
        }


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
    public IEnumerator MapMoveDelay()
    {
        yield return new WaitForSeconds(0.7f);
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