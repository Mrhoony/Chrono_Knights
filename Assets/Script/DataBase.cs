using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;
    public int[] storageItemCodeList;
    public int[] storageItemSkillCodeList;
    public int availableStorageSlot;
    public int takeKeySlot;
    public int availableInventorySlot;
    public int currentDate;
    public bool isTrainigPossible;
    public bool[] eventFlag;

    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        storageItemCodeList = new int[72];
        storageItemSkillCodeList = new int[72];
        availableStorageSlot = 36;
        takeKeySlot = 3;
        availableInventorySlot = 6;
        currentDate = 0;
        isTrainigPossible = false;
        eventFlag = new bool[36];
        for(int i = 0; i < 36; ++i)
        {
            eventFlag[i] = false;
        }

        Debug.Log("database init");
    }

    public int[] GetStorageItemCodeList()
    {
        return storageItemCodeList;
    }
    public int[] GetStorageItemSkillCodeList()
    {
        return storageItemSkillCodeList;
    }
    #region 저장 영역
    public int GetAvailableStorageSlot()
    {
        return availableStorageSlot;
    }
    public int GetTakeKeySlot()
    {
        return takeKeySlot;
    }

    public int GetAvailableInventorySlot()
    {
        return availableInventorySlot;
    }
    public int GetCurrentDate()
    {
        return currentDate;
    }
    public bool[] GetEventFlag()
    {
        return eventFlag;
    }
    public bool GetTrainingPossible()
    {
        return isTrainigPossible;
    }
    public bool GetEventFlag(int flagNum)
    {
        return eventFlag[flagNum];
    }

    public void SaveGameData(int _currentDate, bool _isTrainigPossible, bool[] _eventFlag)
    {
        currentDate = _currentDate;
        isTrainigPossible = _isTrainigPossible;
        eventFlag = _eventFlag;
    }
    public void SaveStorageData(int[] _storageItemList, int[] _storageItemSkillList, int _availableStorageSLot)
    {
        storageItemCodeList = _storageItemList;
        storageItemSkillCodeList = _storageItemSkillList;
        availableStorageSlot = _availableStorageSLot;
    }
    public void SaveInventoryData(int _takeKeySlot, int _availableInventorySlot)
    {
        takeKeySlot = _takeKeySlot;
        availableInventorySlot = _availableInventorySlot;
    }
    #endregion
}