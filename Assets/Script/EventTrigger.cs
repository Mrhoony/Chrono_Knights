using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventCheckType
{
    TriggerEnter, InputKey
}

public class EventTriggerSet
{
    public string[] eventName;
    public string eventSelectName;

    public EventTriggerSet(string _EventName)
    {
        string[] splitEventName;
        int splitEventNameCount;

        splitEventName = _EventName.Split(':');
        splitEventNameCount = splitEventName.Length;

        if (splitEventNameCount == 1)
        {
            eventName = splitEventName;
            eventSelectName = "";
        }
        else if (splitEventNameCount > 1)
        {
            eventName = new string[splitEventNameCount - 1];

            for(int i = 0; i < splitEventNameCount - 1; ++i)
            {
                eventName[i] = splitEventName[i];
            }
            eventSelectName = splitEventName[splitEventNameCount - 1];
        }
    }

    public bool EventStart()
    {
        if (eventName.Length == 1)
        {
            return DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[0]);
        }
        else if (eventName.Length > 1)
        {
            return SelectedEventStart();
        }
        return false;
    }
    public bool SelectedEventStart()
    {
        Debug.Log(eventSelectName);
        switch (DungeonManager.instance.scenarioManager.GetSelectedEventOn(eventSelectName))
        {
            case 1:
                {
                    if (DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[0]))
                    {
                        DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[1]);
                        if (eventName.Length > 2)
                        {
                            DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[2]);
                        }
                        return true;
                    }
                    break;
                }
            case 2:
                {
                    Debug.Log(eventName[1]);
                    if (DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[1]))
                    {
                        DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[0]);
                        if (eventName.Length > 2)
                        {
                            DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[2]);
                        }
                        return true;
                    }
                    break;
                }
            case 3:
                {
                    if (DungeonManager.instance.scenarioManager.ScenarioCheck(eventName[2]))
                    {
                        DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[0]);
                        DungeonManager.instance.scenarioManager.ScenarioFlagCheck(eventName[1]);

                        return true;
                    }
                    break;
                }
        }
        Debug.Log("Has Not Event");
        return false;
    }
}

public class EventTrigger : MonoBehaviour
{
    public string[] eventName;

    public List<EventTriggerSet> eventTriggerSet;
    public EventEndTrigger eventEndTrigger;

    private void OnEnable()
    {
        if (eventName.Length < 1) return;

        eventTriggerSet = new List<EventTriggerSet>();

        for (int i = 0; i < eventName.Length; ++i)
        {
            eventTriggerSet.Add(new EventTriggerSet(eventName[i]));
        }
    }

    public bool EventStart()
    {
        int count = eventTriggerSet.Count;

        for(int i = 0; i < count; ++i)
        {
            if (eventTriggerSet[i].EventStart()) return true;
        }
        return false;
    }
}