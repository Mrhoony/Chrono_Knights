using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScenarioManager : MonoBehaviour
{
    public CanvasManager canvasManager;
    public Dictionary<string, bool> eventFlag;
    public Dictionary<string, int> butterflyEffect;
    public int storyProgress;

    public PlayableDirectorScript playableDirector;

    public Dictionary<string, int> eventList;
    Dictionary<string, List<EventDialog>> eventContent;

    Dictionary<int, List<RepeatEventDialog>> repeatEventList = new Dictionary<int, List<RepeatEventDialog>>();
    Dictionary<int, List<EventTalkBox>> eventTalkBox = new Dictionary<int, List<EventTalkBox>>();

    NPC_Control npc;

    public bool isDialogOn;
    public bool isRepeatDialogOn;
    public int index;
    
    public void EventReset()
    {
        storyProgress = 0;
        eventFlag = new Dictionary<string, bool>();
    }
    public void SetEventList(Dictionary<int, List<RepeatEventDialog>> _RepeatEventList)
    {
        repeatEventList = _RepeatEventList;
    }
    public void SetEventList(Dictionary<string, int> _EventList, Dictionary<string, List<EventDialog>> _EventContent)
    {
        eventList = _EventList;
        eventContent = _EventContent;

        //선택지 이름, 트리거활성화 
        butterflyEffect = new Dictionary<string, int>();
        butterflyEffect.Add("TutorialSelect", 0);
        //butterflyEffect.Add("TutorialSelect", false);
        //butterflyEffect.Add("TutorialSelect", false);
    }
    public void SetEventTalkBoxList(Dictionary<int, List<EventTalkBox>> _EventTalkBox)
    {
        eventTalkBox = _EventTalkBox;
    }
    public void ButterflyEffectSelect(string _SelectedButterflyEffect, int _EventNumber)
    {
        if (butterflyEffect.ContainsKey(_SelectedButterflyEffect))
        {
            Debug.Log(_EventNumber);
            butterflyEffect[_SelectedButterflyEffect] = _EventNumber;
            Debug.Log(butterflyEffect[_SelectedButterflyEffect]);
        }
        else
        {
            Debug.Log("Butterfly effect name error");
        }
    }
    
    // timeline event
    public bool ScenarioCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            if (!eventFlag[_CheckCurrentProgress])
            {
                if (storyProgress < eventList[_CheckCurrentProgress]) return false;

                eventFlag[_CheckCurrentProgress] = true;

                if(storyProgress == eventList[_CheckCurrentProgress])
                {
                    storyProgress = eventList[_CheckCurrentProgress] + 1;
                }

                canvasManager.talkBox.gameObject.SetActive(false);
                canvasManager.MainScenarioStart();
                CameraManager.instance.MainScenarioStart();

                GameObject.Find("EventList").transform.Find(_CheckCurrentProgress).gameObject.SetActive(true);
                return true;
            }
        }
        else
        {
            Debug.Log("Not Found");
        }
        return false;
    }
    public void ScenarioFlagCheck(string _CheckCurrentProgress)
    {
        if (eventList.ContainsKey(_CheckCurrentProgress))
        {
            eventFlag[_CheckCurrentProgress] = true;
        }
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
    public bool ScenarioRepeatObjectCheck(int _ObjectNumber)
    {
        List<RepeatDialog> _TempRepeatEventList = null;

        if (repeatEventList.ContainsKey(_ObjectNumber))
        {
            for (int i = 0; i < repeatEventList[_ObjectNumber].Count; ++i)
            {
                if (repeatEventList[_ObjectNumber][i].eventNumber <= storyProgress)
                {
                    _TempRepeatEventList = repeatEventList[_ObjectNumber][i].eventDialog;
                }
                else
                {
                    break;
                }
            }

            if (_TempRepeatEventList != null)
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

    public int GetSelectedEventOn(string _ButterflyEffectEvent)
    {
        if (butterflyEffect.ContainsKey(_ButterflyEffectEvent))
        {
            Debug.Log(butterflyEffect[_ButterflyEffectEvent]);
            return butterflyEffect[_ButterflyEffectEvent];
        }
        return 0;
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
    public void TalkBoxInActive()
    {
        canvasManager.CloseTalkBox();
    }

    public List<EventDialog> GetEventTextlist(string _EventName)
    {
        List<EventDialog> returnEventList;

        if (eventList.ContainsKey(_EventName))
        {
            returnEventList = eventContent[_EventName];
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
    public Dictionary<string, bool> GetEventFlag()
    {
        return eventFlag;
    }
    public void LoadGamePlayData(int _StoryProgress, Dictionary<string, bool> _eventFlag)
    {
        storyProgress = _StoryProgress;
        eventFlag = _eventFlag;
    }
}
