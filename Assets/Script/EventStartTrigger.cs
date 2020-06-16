using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStartTrigger : MonoBehaviour
{
    public GameObject[] activeObject;
    public int monsterCount;
    public GameObject spawner;

    private void OnEnable()
    {
        if(activeObject.Length > 0)
        {
            DungeonManager.instance.ScenarioMonsterPop(activeObject, spawner, monsterCount);
        }
    }
}