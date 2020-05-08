using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public CanvasManager canvasManager;
    public bool[] eventFlag;
    public int storyProgress;
    Dictionary<string, int> eventList = new Dictionary<string, int>();
    Dictionary<int, List<EventDialog>> eventContent = new Dictionary<int, List<EventDialog>>();

    public void EventReset()
    {
        storyProgress = 0;
        eventFlag = new bool[36];
        eventFlag[0] = true;
        for (int i = 1; i < 36; ++i)
        {
            eventFlag[i] = false;
        }
    }
    public void SetEventList(Dictionary<string, int> _EventList, Dictionary<int, List<EventDialog>> _EventContent)
    {
        eventList = _EventList;
        eventContent = _EventContent;
    }

    public void ScenarioCheck(string _CheckCurrentProgress)
    {
        if (!eventFlag[eventList[_CheckCurrentProgress]])
        {
            eventFlag[eventList[_CheckCurrentProgress]] = true;
            ++storyProgress;
            canvasManager.SetDialogText(eventContent[eventList[_CheckCurrentProgress]]);
        }
    }
    
    public int GetStoryProgress()
    {
        return storyProgress;
    }
    public bool[] GetEventFlag()
    {
        return eventFlag;
    }
    public void LoadGamePlayDate(int _StoryProgress, bool[] _eventFlag)
    {
        storyProgress = _StoryProgress;
        eventFlag = _eventFlag;
    }

}
