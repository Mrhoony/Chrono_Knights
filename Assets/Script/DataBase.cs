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
    public int storyProgress;

    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        storageItemCodeList = new int[72];
        currentMoney = 0;
        currentDate = 0;
        isTrainigPossible = false;
        eventFlag = new Dictionary<string, bool>();
        EventFlagInit();
        
        CanvasManager.instance.DebugText("database init");
    }
    public void EventFlagInit()
    {
        List<string> scenarioData = Database_Game.instance.mainScenarioName;

        for (int i = 0; i < scenarioData.Count; ++i)
        {
            eventFlag.Add(scenarioData[i], false);
        }
        DungeonManager.instance.scenarioManager.eventFlag = eventFlag;
    }
    public void EventFlagInit(List<string> _ScenarioData)
    {
        for (int i = eventFlag.Count; i < _ScenarioData.Count; ++i)
        {
            Debug.Log(eventFlag.Count);
            Debug.Log(_ScenarioData[i]);
            eventFlag.Add(_ScenarioData[i], false);
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

        if (eventFlag.Count < scenarioData.Count)
        {
            if(eventFlag.Count < 1) eventFlag = new Dictionary<string, bool>();
            EventFlagInit(scenarioData);
        }
        return eventFlag;
    }
    public int GetStoryProgress()
    {
        return storyProgress;
    }
    public bool GetTrainingPossible()
    {
        return isTrainigPossible;
    }

    public void SaveGameData(int _currentDate, bool _isTrainigPossible, Dictionary<string, bool> _eventFlag, int _StoryProgress)
    {
        currentDate = _currentDate;
        isTrainigPossible = _isTrainigPossible;
        eventFlag = _eventFlag;
        storyProgress = _StoryProgress;
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