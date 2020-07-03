using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;
    public int[] storageItemCodeList;

    public int currentMoney;
    public int currentDate;

    public bool isTrainigPossible;
    public Dictionary<string, bool> eventFlag;
    public Dictionary<string, int> butterFlyEffectFlag;
    public int storyProgress;

    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        storageItemCodeList = new int[72];
        currentMoney = 0;
        currentDate = 0;
        isTrainigPossible = false;
        EventFlagInit();
        
        CanvasManager.instance.DebugText("database init");
    }
    public void EventFlagInit()
    {
        eventFlag = new Dictionary<string, bool>();
        butterFlyEffectFlag = new Dictionary<string, int>();
        List<string> scenarioData = Database_Game.instance.mainScenarioName;

        butterFlyEffectFlag = DungeonManager.instance.scenarioManager.SetButterFlyEffectFlagInit();

        for (int i = 0; i < scenarioData.Count; ++i)
        {
            eventFlag.Add(scenarioData[i], false);
        }
        DungeonManager.instance.scenarioManager.eventFlag = eventFlag;
    }

    public int[] GetStorageItemCodeList()
    {
        return storageItemCodeList;
    }
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
    public int GetCurrentDate()
    {
        return currentDate;
    }
    public Dictionary<string, bool> GetEventFlag()
    {
        List<string> scenarioData = Database_Game.instance.mainScenarioName;

        if (eventFlag.Count < 1) eventFlag = new Dictionary<string, bool>();

        for (int i = 0; i < scenarioData.Count; ++i)
        {
            if (!eventFlag.ContainsKey(scenarioData[i]))
                eventFlag.Add(scenarioData[i], false);
        }

        return eventFlag;
    }
    public Dictionary<string, int> GetButterFlyEffectFlag()
    {
        if(butterFlyEffectFlag == null)
        {
            butterFlyEffectFlag = new Dictionary<string, int>();
        }
        // 저장된 파일에 데이터 가 없을 경우
        if(butterFlyEffectFlag.Count < 1)
        {
            butterFlyEffectFlag = DungeonManager.instance.scenarioManager.SetButterFlyEffectFlagInit();
        }
        else
        {
            Dictionary<string, int> temp = DungeonManager.instance.scenarioManager.SetButterFlyEffectFlagInit();

            // 추가된 시나리오가 더 있을 경우
            if(butterFlyEffectFlag.Count < temp.Count)
            {
                // 데이터베이스의 딕셔너리를 순회하면서 시나리오가 없으면 추가하고 해당 시나리오를 초기화
                foreach(KeyValuePair<string, int> item in temp)
                {
                    if (butterFlyEffectFlag.ContainsKey(item.Key))
                    {
                        break;
                    }
                    butterFlyEffectFlag.Add(item.Key, 0);
                }
            }
        }

        return butterFlyEffectFlag;
    }
    public int GetStoryProgress()
    {
        return storyProgress;
    }
    public bool GetTrainingPossible()
    {
        return isTrainigPossible;
    }

    public void SaveGameData(int _currentDate, bool _isTrainigPossible, int _StoryProgress, Dictionary<string, bool> _eventFlag, Dictionary<string, int> _ButterflyEffectFlag)
    {
        currentDate = _currentDate;
        isTrainigPossible = _isTrainigPossible;
        storyProgress = _StoryProgress;
        eventFlag = _eventFlag;
        butterFlyEffectFlag = _ButterflyEffectFlag;
    }
    public void SaveStorageData(int[] _storageItemList)
    {
        storageItemCodeList = _storageItemList;
    }
    public void SaveInventoryData(int _currentMoney)
    {
        currentMoney = _currentMoney;
    }
}