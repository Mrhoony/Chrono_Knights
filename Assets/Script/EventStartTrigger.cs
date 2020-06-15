using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStartTrigger : MonoBehaviour
{
    public GameObject[] activeObject;
    public int monsterCount;

    private void OnEnable()
    {
        if(activeObject.Length > 0)
        {
            DungeonManager.instance.mainQuest = true;
            monsterCount = activeObject.Length;

            for (int i = 0; i < activeObject.Length; ++i)
            {
                activeObject[i].SetActive(true);
                activeObject[i].GetComponent<NormalMonsterControl>().monsterDeadCount = monsterKill;
            }
        }
    }

    public void monsterKill()
    {
        --monsterCount;
        if(monsterCount < 1)
        {
            DungeonManager.instance.MainEventQuestClear();
        }
    }
}