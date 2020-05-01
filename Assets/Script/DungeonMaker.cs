using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class DungeonMaker : MonoBehaviour
{
    #region dungeon
    public Marker marker;
    public MarkerVariable marker_Variable;   // Marker로부터 전달받는 값 저장공간
    public FloorData[] FloorDatas;
    public DungeonTrialStack dungeonTrialStack;
    #endregion

    #region 던전 생성 관련 변수
    private GameObject[] monsterPreFabsList;
    private GameObject[] bossMonsterPreFabsList;
    private GameObject[] currentStageMonsterList;
    private GameObject[] spawner;
    private GameObject dropItemPool;
    private Vector2 entrance;               // 텔레포트 위치

    private int spawnerCount;
    private int spawn;

    public int currentStage;
    public int bossStageCount;
    public bool freePassNextFloor;          // 다음층 스킵 체크
    public bool bossSetting;                // 보스층 등장 체크
    public bool floorRepeat;                // 층 반복 체크
    public int bossClearCount;

    private int monsterCount;           // 최대 몬스터 수
    private int eliteMonsterCount;
    private int currentMonsterCount;    // 현재 몬스터 수
    private int allKillCount;           // 총 몬스터 킬 수
    private int monsterKillCount;
    private int eliteMonsterKillCount;
    private int bossMonsterKillCount;
    #endregion

    public void DungeonMakerInit()
    {
        monsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/Monster");
        bossMonsterPreFabsList = Resources.LoadAll<GameObject>("Prefabs/Unit/Mob/BossMonster");

        marker = new Marker();
        marker_Variable = new MarkerVariable();
        dungeonTrialStack = new DungeonTrialStack();

        FloorDatas = new FloorData[70];

        FloorDangerousSetting(0);
        marker_Variable.Reset();
    }
    public void DungeonReset()
    {
        bossSetting = false;
        floorRepeat = false;
        freePassNextFloor = false;

        currentStage = 0;
        bossStageCount = 0;
        bossClearCount = 0;
        monsterCount = 0;
        currentMonsterCount = 0;
    }
    public void EnterTheDungeon()
    {
        dungeonTrialStack.Init();
        dropItemPool = GameObject.Find("DropItemPool");
    }

    public void MonsterClear()
    {
        for (int i = 0; i < monsterCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterStop();
        }
    }
    private void FloorDangerousSetting(int plusDangerous)
    {
        for (int floor = 1; floor < 71; ++floor)
        {
            FloorDatas[floor - 1] = new FloorData(floor, bossClearCount, floor * 2);
        }
    }
    public void SetTrialStack(int _DungeonStatusNum, int _StatusValue)
    {
        dungeonTrialStack.SetTrialStatus(_DungeonStatusNum, _StatusValue);
    }
    public void SetTrialStatus()
    {
        //보스 층 클리어 한 양만큼 방어력 / 공격력 증가
        MonsterAttackSetting(dungeonTrialStack.currentDungeonStatus[0]);
        MonsterDefSetting(dungeonTrialStack.currentDungeonStatus[1]);
        MonsterHPSetting(dungeonTrialStack.currentDungeonStatus[2], true);
    }
    
    #region dungeon 관련
    // 층 이동 시 나타날 층 세팅
    public void FloorSetting(GameObject[] _MapList, GameObject _Player, CameraManager _MainCamera, GameObject _BackGroundSet)
    {
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

        int selectedMapNum = Random.Range(0, _MapList.Length);
        entrance = _MapList[selectedMapNum].GetComponent<Map_ObjectSetting>().entrance.transform.position;
        spawner = _MapList[selectedMapNum].GetComponent<Map_ObjectSetting>().spawner;
        spawnerCount = spawner.Length;

        _Player.transform.position = entrance;
        _MainCamera.SetCameraBound(_MapList[selectedMapNum].GetComponent<BoxCollider2D>());
        _MainCamera.transform.position = entrance;
        _BackGroundSet.GetComponent<BackgroundScrolling>().SetBackGroundPosition(entrance, currentStage);

        if (!bossSetting)
        {
            if ((bossStageCount - (5 * bossClearCount) - 2) > 0)  // 보스스테이지 설정
            {
                if (bossStageCount * 20 > Random.Range(50, 90))
                    bossSetting = true;
            }            // 이벤트 플래그로 구간별 보스 등장
        }           // 보스 스테이지 설정

        float randomX;
        if (bossSetting)                       // 보스층 일 때
        {
            bossSetting = false;
            bossStageCount = 0;

            int randomBoss = Random.Range(0, bossMonsterPreFabsList.Length);

            monsterCount = 1;
            currentMonsterCount = monsterCount;
            currentStageMonsterList = new GameObject[currentMonsterCount];
            currentStageMonsterList[0] = Instantiate(bossMonsterPreFabsList[randomBoss],
                new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x,
                spawner[Random.Range(0, spawnerCount)].transform.position.y),
                Quaternion.identity);
            currentStageMonsterList[0].GetComponent<BossMonster_Control>().monsterDeadCount = FloorBossKill;
        }
        else if (floorRepeat)                    // 맵 반복시
        {
            Debug.Log("WHERE ::: " + monsterCount + " ABS : " + currentStageMonsterList.Length);
            floorRepeat = false;
            currentMonsterCount = monsterCount;

            if (monsterCount > 0)
            {
                for (int i = 0; i < monsterCount; ++i)
                {
                    randomX = Random.Range(-1, 2);
                    currentStageMonsterList[i].GetComponent<Monster_Control>().MonsterInit();
                    currentStageMonsterList[i].transform.position = new Vector2(spawner[Random.Range(0, spawnerCount)].transform.position.x + randomX
                                                             , spawner[Random.Range(0, spawnerCount)].transform.position.y);
                }
            }
        }
        else                                   // 일반 맵일경우
        {
            eliteMonsterCount = marker_Variable.markerVariable[(int)Markers.SetSpecialMonster_NF];
            monsterCount = FloorDatas[currentStage].SpawnAmount + marker_Variable.markerVariable[(int)Markers.SetMonster_NF] + eliteMonsterCount;
            currentMonsterCount = monsterCount;

            currentStageMonsterList = new GameObject[monsterCount];

            int monsterPrefabListCount = monsterPreFabsList.Length;
            int randomSpawner = Random.Range(0, spawnerCount);

            // 몬스터 스폰
            for (int i = 0; i < currentMonsterCount - eliteMonsterCount; ++i)
            {
                randomX = Random.Range(-1, 2);
                currentStageMonsterList[i] = Instantiate(monsterPreFabsList[Random.Range(0, monsterPrefabListCount)]
                    , new Vector2(
                        spawner[randomSpawner].transform.position.x + randomX,
                        spawner[randomSpawner].transform.position.y),
                        Quaternion.identity);
                currentStageMonsterList[i].GetComponent<NormalMonsterControl>().monsterDeadCount = FloorMonsterKill;
            }
            for (int j = currentMonsterCount - eliteMonsterCount; j < currentMonsterCount; ++j)
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
        }

        if (currentStage < 2) CanvasManager.instance.dungeonUI.SetDungeonFloor(currentStage, "");
        else
        {
            // 선택된 시련 만큼 몬스터 스테이터스 증가
            SetTrialStatus();
            //보스 층 클리어 한 양만큼 방어력 / 공격력 증가
            MonsterAttackSetting(bossClearCount * 1);
            MonsterDefSetting(bossClearCount * 1);

            CanvasManager.instance.dungeonUI.SetDungeonFloor(currentStage, SetFloorStatus(_Player.GetComponent<PlayerStatus>()));
        }

        MarkerSetting(_MapList[selectedMapNum]);
    }
    public void FloorReset()
    {
        DungeonPoolManager.instance.bossMonsterCountReset();

        if (floorRepeat) return;

        for (int i = 0; i < monsterCount; ++i)
        {
            if (currentStageMonsterList[i] != null)
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
            DungeonManager.instance.dungeonClear = true;
            DungeonManager.instance.phaseClear = true;
            ++bossClearCount;
        }
        StartCoroutine(BossKillSlowMotion());
    }
    public void FloorMonsterKill()
    {
        --currentMonsterCount;
        ++allKillCount;
        ++monsterKillCount;
        if (currentMonsterCount < 1)
        {
            DungeonManager.instance.dungeonClear = true;
        }
    }
    public void FloorEliteMonsterKill()
    {
        --currentMonsterCount;
        ++allKillCount;
        ++eliteMonsterKillCount;
        if (currentMonsterCount < 1)
        {
            DungeonManager.instance.dungeonClear = true;
        }
    }
    IEnumerator BossKillSlowMotion()
    {
        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(2f);

        Time.timeScale = 1f;

        yield return new WaitForSeconds(1f);

        CanvasManager.instance.OpenTrialCardSelectMenu();
    }

    public void MarkerSetting(GameObject _SelectedMap)
    {
        int markerRandom = Random.Range(0, 12);
        marker.thisMarker = (Markers)markerRandom;
        GameObject mark = _SelectedMap.GetComponent<Map_ObjectSetting>().teleporter.transform.GetChild(0).gameObject;
        mark.GetComponent<DungeonMarker>().SetMarker((Markers)markerRandom);

        marker_Variable.markerPreVariable = marker_Variable.markerVariable;
        marker_Variable.Reset();
    }
    public string SetFloorStatus(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.PlayerStatusResultInit();          // 기본 스탯 + 장비 스탯 + 트레이닝 스탯으로 초기화

        string stageStatText = "";
        int markerNumber = (int)marker.thisMarker;

        switch (marker.thisMarker)
        {
            case Markers.SetDamageBuffOnFloor_NF:
                _PlayerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], true);
                MonsterAttackSetting(marker_Variable.markerVariable[markerNumber]);
                stageStatText = "전체 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                MonsterAttackSetting(marker_Variable.markerVariable[markerNumber]);
                stageStatText = "몬스터 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                _PlayerStatus.SetAttackAdd_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "자신의 공격력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetPosHPOnMonster_NF:
                MonsterHPSetting(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "몬스터 체력 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetNegHPOnMonster_NF:
                MonsterHPSetting(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "몬스터 체력 " + marker_Variable.markerVariable[markerNumber] + " 감소";
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                _PlayerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "대시 거리 " + marker_Variable.markerVariable[markerNumber] + " 증가";
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                _PlayerStatus.SetDashDistance_Result(marker_Variable.markerVariable[markerNumber], false);
                stageStatText = "대시 거리 " + marker_Variable.markerVariable[markerNumber] + " 감소";
                break;
            case Markers.SetPosAttackMulty_NF:
                _PlayerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], true);
                stageStatText = "공격력 " + marker_Variable.markerVariable[markerNumber] + "배 증가";
                break;
            case Markers.SetNegAttackMulty_NF:
                _PlayerStatus.SetAttackMulty_Result(marker_Variable.markerVariable[markerNumber], false);
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
    }    // 아이템이 사용된 층에 효과를 적용
    #endregion

    public void MonsterAttackSetting(int _value)
    {
        if (_value == 0) return;
        int _monsterListCount = currentStageMonsterList.Length;
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_Attack(_value);
        }
    }
    public void MonsterHPSetting(int _value, bool upgrade)
    {
        if (_value == 0) return;
        int _monsterListCount = currentStageMonsterList.Length;
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_Hp(_value, upgrade);
        }
    }
    public void MonsterDefSetting(int _value)
    {
        if (_value == 0) return;
        int _monsterListCount = currentStageMonsterList.Length;
        for (int i = 0; i < _monsterListCount; ++i)
        {
            currentStageMonsterList[i].GetComponent<EnemyStatus>().Set_Defense(_value);
        }
    }
}
