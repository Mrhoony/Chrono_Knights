using System;
using System.Collections.Generic;

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

        CanvasManager.instance.DebugText("database init");
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