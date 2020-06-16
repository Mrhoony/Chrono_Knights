using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventCheckType
{
    TriggerEnter, InputKey
}

public class EventTrigger : MonoBehaviour
{
    public string[] eventName;
    public EventEndTrigger eventEndTrigger;

    public bool EventStart()
    {
        int count = eventName.Length;
        for(int i = 0; i < count; ++i)
        {
            if (DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[i])) return true;
        }
        return false;
    }
}
