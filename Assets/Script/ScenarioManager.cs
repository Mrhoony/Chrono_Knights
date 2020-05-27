using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public CanvasManager canvasManager;
    public bool[] eventFlag;
    public int storyProgress;
    RepeatEventDialog _TempRepeatEventList;

    Dictionary<string, int> eventList = new Dictionary<string, int>();
    Dictionary<int, List<EventDialog>> eventContent = new Dictionary<int, List<EventDialog>>();
    Dictionary<int, List<RepeatEventDialog>> repeatEventList = new Dictionary<int, List<RepeatEventDialog>>();
    Dictionary<int, List<EventTalkBox>> eventTalkBox = new Dictionary<int, List<EventTalkBox>>();

    NPC_Control npc;

    public bool isDialogOn;
    public bool isRepeatDialogOn;
    public bool isOneByOneTextOn;
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

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isDialogOn)
            {
                if (isOneByOneTextOn)
                {
                    isOneByOneTextOn = false;
                    canvasManager.OneByOneTextSkip();
                }
                else
                {
                    SetDialogText();
                }
            }
            else if (isRepeatDialogOn)
            {
                if (isOneByOneTextOn)
                {
                    isOneByOneTextOn = false;
                    canvasManager.OneByOneTextSkip();
                }
                else
                {
                    SetRepeatDialogText();
                }
            }
        }
    }

    public bool ScenarioCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            Debug.Log("has key");
            if (!eventFlag[eventList[_CheckCurrentProgress]])
            {
                eventFlag[eventList[_CheckCurrentProgress]] = true;
                storyProgress = eventList[_CheckCurrentProgress];
                SetDialogText();
                Debug.Log("scenario check " + storyProgress);
                return true;
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }
    public bool ScenarioRepeatCheck(NPC_Control _NPC)
    {
        if (repeatEventList.ContainsKey(_NPC.objectNumber))
        {
            npc = _NPC;
            for (int i = 0; i < repeatEventList[npc.objectNumber].Count; ++i)
            {
                if (repeatEventList[npc.objectNumber][i].eventNumber <= storyProgress)
                {
                    _TempRepeatEventList = repeatEventList[npc.objectNumber][i];
                }
                else
                {
                    break;
                }
            }
            SetRepeatDialogText();
            return true;
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }

    public void SetDialogText()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        if (index >= eventContent[storyProgress].Count)
        {
            index = 0;
            isOneByOneTextOn = false;
            canvasManager.CloseDialogBox();
            isDialogOn = false;
            return;
        }
        string NPCName = "";
        string content = "";
        NPCName = eventContent[storyProgress][index].NPCName;
        content = eventContent[storyProgress][index].content;
        ++index;

        canvasManager.SetDialogText(NPCName, content, this);
        Debug.Log("SetDialogText");
    }

    public void SetRepeatDialogText()
    {
        isRepeatDialogOn = true;
        isOneByOneTextOn = true;

        if (index >= _TempRepeatEventList.eventDialog.Count)
        {
            index = 0;
            canvasManager.CloseDialogBox();
            isOneByOneTextOn = false;
            isRepeatDialogOn = false;
            npc.OpenNPCUI();
            return;
        }
        string NPCName = "";
        string content = "";

        NPCName = _TempRepeatEventList.eventDialog[index].NPCName;
        content = _TempRepeatEventList.eventDialog[index].content;

        ++index;
        canvasManager.SetDialogText(NPCName, content, this);

        Debug.Log("SetDialogRepeatText");
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
