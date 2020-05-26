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
    Dictionary<int, List<RepeatEventDialog>> repeatEventList = new Dictionary<int, List<RepeatEventDialog>>();

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
    public void SetEventList(Dictionary<string, int> _EventList, Dictionary<int, List<EventDialog>> _EventContent, Dictionary<int, List<RepeatEventDialog>> _RepeatEventList)
    {
        eventList = _EventList;
        eventContent = _EventContent;
        repeatEventList = _RepeatEventList;
    }
    public void SetEventTalkBoxList(Dictionary<int, List<EventTalkBox>> _EventTalkBox)
    {
        eventTalkBox = _EventTalkBox;
    }
    
    public bool ScenarioRepeatCheck(NPC_Control _NPC)
    {
        if (repeatEventList.ContainsKey(_NPC.objectNumber))
        {
            List<RepeatEventDialog> _TempRepeatEventList = new List<RepeatEventDialog>();
            for(int i = 0; i < repeatEventList[_NPC.objectNumber].Count; ++i)
            {
                if (repeatEventList[_NPC.objectNumber][i].eventNumber < storyProgress)
                {
                    _TempRepeatEventList = repeatEventList[_NPC.objectNumber];
                }
                else
                {
                    break;
                }
            }
            canvasManager.SetDialogText(_TempRepeatEventList, _NPC);
            return true;
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }
    public bool ScenarioCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            if (!eventFlag[eventList[_CheckCurrentProgress]])
            {
                eventFlag[eventList[_CheckCurrentProgress]] = true;
                ++storyProgress;
                canvasManager.SetDialogText(eventContent[eventList[_CheckCurrentProgress]]);
                return true;
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
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
