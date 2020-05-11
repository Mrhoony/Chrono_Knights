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

    Dictionary<int, List<EventTalkBox>> eventTalkBox = new Dictionary<int, List<EventTalkBox>>();

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
    public void SetEventTalkBoxList(Dictionary<int, List<EventTalkBox>> _EventTalkBox)
    {
        eventTalkBox = _EventTalkBox;
    }

    public void ScenarioCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            if (!eventFlag[eventList[_CheckCurrentProgress]])
            {
                eventFlag[eventList[_CheckCurrentProgress]] = true;
                ++storyProgress;
                canvasManager.SetDialogText(eventContent[eventList[_CheckCurrentProgress]]);
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
    }

    public void ScenarioCheckTalkBox(GameObject NPC, int _NPCCode)
    {
        if (eventTalkBox.ContainsKey(_NPCCode))
        {
            int temp = 0;
            int count = eventTalkBox[_NPCCode].Count;
            for(int i = 0; i < count; ++i)
            {
                if (eventTalkBox[_NPCCode][i].scenarioNumber <= storyProgress)
                {
                    temp = i;
                }
                else
                {
                    break;
                }
            }
            canvasManager.SetTalkBoxText(NPC, eventTalkBox[_NPCCode][temp].content);
        }
        else
        {
            Debug.Log("Not Found");
        }
    }
    public void TalkBoxDisActive()
    {
        canvasManager.CloseTalkBox();
    }
    
    public int GetStoryProgress()
    {
        return storyProgress;
    }
    public bool[] GetEventFlag()
    {
        return eventFlag;
    }
    public void LoadGamePlayData(int _StoryProgress, bool[] _eventFlag)
    {
        storyProgress = _StoryProgress;
        eventFlag = _eventFlag;
    }
}
