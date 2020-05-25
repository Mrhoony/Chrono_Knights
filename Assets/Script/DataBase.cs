using System;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;
    public int[] storageItemCodeList;

    public int currentMoney;
    public int currentDate;

    public bool isTrainigPossible;
    public bool[] eventFlag;
    public int storyProgress;

    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        storageItemCodeList = new int[72];
        currentMoney = 0;
        currentDate = 0;
        isTrainigPossible = false;
        eventFlag = new bool[36];
        storyProgress = 0;
        for (int i = 0; i < 36; ++i)
        {
            eventFlag[i] = false;
        }

        Debug.Log("database init");
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
    public bool[] GetEventFlag()
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
    public bool GetEventFlag(int flagNum)
    {
        return eventFlag[flagNum];
    }

    public void SaveGameData(int _currentDate, bool _isTrainigPossible, bool[] _eventFlag, int _StoryProgress)
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