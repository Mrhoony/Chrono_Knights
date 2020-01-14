using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;
    public int[] storageItemCodeList;
    public int availableStorageSlot;
    public int takeKeySlot;
    public int availableInventorySlot;
    public int currentDate;
    public bool[] eventFlag;

    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        storageItemCodeList = new int[72];
        availableStorageSlot = 36;
        takeKeySlot = 3;
        availableInventorySlot = 6;
        currentDate = 0;
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
    public bool GetEventFlag(int flagNum)
    {
        return eventFlag[flagNum];
    }

    public void SaveGameData(int _currentDate, bool[] _eventFlag)
    {
        currentDate = _currentDate;
        eventFlag = _eventFlag;
    }
    public void SaveStorageData(int[] _storageKeyList, int _availableStorageSLot)
    {
        storageItemCodeList = _storageKeyList;
        availableStorageSlot = _availableStorageSLot;
    }
    public void SaveInventoryData(int _takeKeySlot, int _availableInventorySlot)
    {
        takeKeySlot = _takeKeySlot;
        availableInventorySlot = _availableInventorySlot;
    }
}