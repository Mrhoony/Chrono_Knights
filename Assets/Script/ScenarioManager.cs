using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenarioManager : MonoBehaviour
{
    public CanvasManager canvasManager;
    public bool[] eventFlag;
    public int storyProgress;

    public PlayableDirectorScript playableDirector;

    Dictionary<string, int> eventList = new Dictionary<string, int>();
    Dictionary<int, List<EventDialog>> eventContent = new Dictionary<int, List<EventDialog>>();

    Dictionary<int, List<RepeatEventDialog>> repeatEventList = new Dictionary<int, List<RepeatEventDialog>>();
    Dictionary<int, List<EventTalkBox>> eventTalkBox = new Dictionary<int, List<EventTalkBox>>();

    NPC_Control npc;

    public bool isDialogOn;
    public bool isRepeatDialogOn;
    public int index;
    
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
    public void SetEventList(Dictionary<int, List<RepeatEventDialog>> _RepeatEventList)
    {
        repeatEventList = _RepeatEventList;
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
    
    // timeline event
    public bool ScenarioCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            Debug.Log("has key");

            if (!eventFlag[eventList[_CheckCurrentProgress]])
            {
                Debug.Log("scenario check " + storyProgress);

                eventFlag[eventList[_CheckCurrentProgress]] = true;
                storyProgress = eventList[_CheckCurrentProgress];
                CanvasManager.instance.talkBox.gameObject.SetActive(false);
                GameObject.Find("EventList").transform.Find(_CheckCurrentProgress).gameObject.SetActive(true);

                CameraManager.instance.MainScenarioStart();
                canvasManager.isMainScenarioOn = true;
                return true;
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }

    // talk NPC
    public bool ScenarioRepeatCheck(NPC_Control _NPC)
    {
        List<RepeatDialog> _TempRepeatEventList = null;

        if (repeatEventList.ContainsKey(_NPC.objectNumber))
        {
            npc = _NPC;
            for (int i = 0; i < repeatEventList[npc.objectNumber].Count; ++i)
            {
                if (repeatEventList[npc.objectNumber][i].eventNumber <= storyProgress)
                {
                    _TempRepeatEventList = repeatEventList[npc.objectNumber][i].eventDialog;
                }
                else
                {
                    break;
                }
            }

            if(_TempRepeatEventList != null)
            {
                isRepeatDialogOn = true;
                canvasManager.SetDialogText(_TempRepeatEventList, npc);
                return true;
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }

    // npc Contact
    public void ScenarioCheckTalkBox(GameObject _Object, int _NPCCode)
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
            canvasManager.SetTalkBoxText(_Object, eventTalkBox[_NPCCode][temp].content);
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

    public List<EventDialog> GetEventTextlist(string _EventName)
    {
        List<EventDialog> returnEventList;

        if (eventList.ContainsKey(_EventName))
        {
            returnEventList = eventContent[eventList[_EventName]];
            return returnEventList;
        }
        else
        {
            Debug.Log("Has Not Key");
            return null;
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
    public void LoadGamePlayData(int _StoryProgress, bool[] _eventFlag)
    {
        storyProgress = _StoryProgress;
        eventFlag = _eventFlag;
    }
}
