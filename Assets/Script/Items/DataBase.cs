using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase
{
    public PlayerData playerData;
    public List<Key> storageKeyList;
    public int currentDate;
    public bool[] eventCheck;

    public void Init()
    {
        playerData = new PlayerData();
        storageKeyList = new List<Key>();
        currentDate = 0;
    }

    public void AddKeyList(Key key)
    {

    }
}
