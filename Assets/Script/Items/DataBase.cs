using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;

    public Key[] storageKeyList;
    public int takeKeySlot;
    public int availableInventorySlot;
    public int availableStorageSlot;

    public int currentDate;
    public bool[] eventCheck;

    public void Init()
    {
        playerData = new PlayerData();
        storageKeyList = new Key[72];
        availableStorageSlot = 36;
        takeKeySlot = 3;
        availableInventorySlot = 6;
        currentDate = 0;
    }

    public void AddKey(Key key)
    {

    }
}
