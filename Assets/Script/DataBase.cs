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
    public bool[] eventCheck;

    public void Init()
    {
        playerData = new PlayerData();
        storageItemCodeList = new int[72];
        availableStorageSlot = 36;
        takeKeySlot = 3;
        availableInventorySlot = 6;
        currentDate = 0;
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
    public int GetcurrentDate()
    {
        return currentDate;
    }

    public void SaveCurrentDate(int _currentDate)
    {
        currentDate = _currentDate;
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