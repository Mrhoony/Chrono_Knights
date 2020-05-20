using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public string[] eventName;

    public void EventStart()
    {
        int count = eventName.Length;
        for(int i = 0; i < count; ++i)
        {
            if (DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[i])) return;
        }
    }
}
