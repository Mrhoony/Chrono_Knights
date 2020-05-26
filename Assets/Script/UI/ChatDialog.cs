using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialog : MonoBehaviour
{
    public CanvasManager cm;
    public Text dialogName;
    public Text dialogText;
    public bool isDialogOn;
    public bool isRepeatDialogOn;
    public bool isOneByOneTextOn;
    public int index;
    public int eventDialogListCount;
    public int repeatEventDialogListCount;
    public string completeText;
    public List<EventDialog> eventDialog;
    public List<RepeatEventDialog> repeatEventDialog;

    public IEnumerator TempCoroutine;

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isDialogOn)
            {
                if (isOneByOneTextOn)
                {
                    StopCoroutine(TempCoroutine);
                    dialogText.text = completeText;
                    isOneByOneTextOn = false;
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
                    StopCoroutine(TempCoroutine);
                    dialogText.text = completeText;
                    isOneByOneTextOn = false;
                }
                else
                {
                    SetRepeatDialogText();
                }
            }
        }
    }

    public void SetDialogList(List<RepeatEventDialog> _EventDialog, CanvasManager _CM)
    {
        cm = _CM;
        repeatEventDialog = _EventDialog;
        repeatEventDialogListCount = repeatEventDialog.Count;
        index = 0;
        isRepeatDialogOn = true;

        SetRepeatDialogText();
    }
    public void SetDialogList(List<EventDialog> _EventDialog, CanvasManager _CM)
    {
        cm = _CM;
        eventDialog = _EventDialog;
        eventDialogListCount = eventDialog.Count;
        index = 0;
        isDialogOn = true;

        SetDialogText();
    }

    public void SetRepeatDialogText()
    {
        if (index >= eventDialogListCount)
        {
            repeatEventDialog = null;
            index = 0;
            isRepeatDialogOn = false;
            cm.CloseDialogBox();
            return;
        }
        string NPCName = "";
        string content = "";

        NPCName = repeatEventDialog[index].NPCName;
        content = repeatEventDialog[index].content;
        completeText = content;

        TempCoroutine = OneByOneTextSetting(NPCName, content);
        StartCoroutine(TempCoroutine);
        ++index;
    }
    public void SetDialogText()
    {
        if (index >= eventDialogListCount)
        {
            eventDialog = null;
            index = 0;
            isDialogOn = false;
            cm.CloseDialogBox();
            return;
        }
        string NPCName = "";
        string content = "";

        NPCName = eventDialog[index].NPCName;
        content = eventDialog[index].content;
        completeText = content;

        TempCoroutine = OneByOneTextSetting(NPCName, content);
        StartCoroutine(TempCoroutine);
        ++index;
    }
        
    public IEnumerator OneByOneTextSetting(string _NPCName, string _Content)
    {
        isOneByOneTextOn = true;
        string _TempDialogText = "";
        int textCount = _Content.Length;
        dialogName.text = _NPCName;
        for(int i = 0; i < textCount; ++i)
        {
            _TempDialogText += _Content[i];
            dialogText.text = _TempDialogText;
            yield return new WaitForSeconds(0.1f);
        }
        isOneByOneTextOn = false;
    }
}