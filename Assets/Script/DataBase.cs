using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;

    private Key[] storageKeyList;
    private int availableStorageSlot;

    private int takeKeySlot;
    private int availableInventorySlot;

    private int currentDate;
    private bool[] eventCheck;

    public DataBase()
    {
        playerData = new PlayerData();
        storageKeyList = new Key[72];
        availableStorageSlot = 36;
        takeKeySlot = 3;
        availableInventorySlot = 6;
        currentDate = 0;
    }

    public Key[] GetStorageKeyList()
    {
        return storageKeyList;
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
    public int GetcurrentDate()
    {
        return currentDate;
    }

    public void SaveCurrentDate(int _currentDate)
    {
        currentDate = _currentDate;
    }
    public void SaveStorageData(Key[] _storageKeyList, int _availableStorageSLot)
    {
        storageKeyList = _storageKeyList;
        availableStorageSlot = _availableStorageSLot;
    }
    public void SaveInventoryData(int _takeKeySlot, int _availableInventorySlot)
    {
        takeKeySlot = _takeKeySlot;
        availableInventorySlot = _availableInventorySlot;
    }
}