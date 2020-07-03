using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStartTrigger : MonoBehaviour
{
    public bool bossStage;

    public GameObject[] activeObject;
    public int monsterCount;
    public GameObject spawner;

    private void OnEnable()
    {
        PlayerControl.instance.gameObject.SetActive(false);
        if (activeObject.Length > 0)
        {
            if (bossStage)
                DungeonManager.instance.ScenarioBossMonsterPop(activeObject);
            else
                DungeonManager.instance.ScenarioMonsterPop(activeObject, spawner, monsterCount);
        }
    }
}