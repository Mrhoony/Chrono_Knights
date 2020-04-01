using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTrialStack
{
    public int[] currentDungeonStatus = new int[3];

    public void Init()
    {
        currentDungeonStatus[0] = 0;
        currentDungeonStatus[1] = 0;
        currentDungeonStatus[2] = 0;
    }
    public void SetTrialStatus(int _dungeonTrialStatusNum, int _statusValue)
    {
        currentDungeonStatus[_dungeonTrialStatusNum] += _statusValue;
    }
}
