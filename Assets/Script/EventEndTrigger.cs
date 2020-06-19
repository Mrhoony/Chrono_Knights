using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEndTrigger : MonoBehaviour
{
    public delegate void EventEndDelegate();
    public EventEndDelegate eventEndDelegate;

    private void OnEnable()
    {
        CameraManager.instance.mainScenarioOn = false;
        CanvasManager.instance.MainScenarioEnd();
        DungeonManager.instance.mainQuest = false;

        if(eventEndDelegate != null)
        {
            eventEndDelegate();
            eventEndDelegate = null;
        }
    }
}
