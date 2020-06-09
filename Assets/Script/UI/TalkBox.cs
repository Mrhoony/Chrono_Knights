using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBox : MonoBehaviour
{
    public GameObject chaseGameObject;

    public Text dialogName;
    public Text dialogText;

    private NPC_Control NPC;

    public List<EventDialog> eventDialog;

    public bool isDialogOn;
    public bool isOneByOneTextOn;

    public int currentEventCount = 0;

    public string completeText;
    ScenarioManager scenarioManager;

    public IEnumerator TempCoroutine;

    public void Update()
    {
        if (!isDialogOn) return;

        if (chaseGameObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 1.5f);
        }

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (isOneByOneTextOn)
            {
                isOneByOneTextOn = false;
                OneByOneTextSkip();
            }
            else
            {
                SetDialogText();
            }
        }
    }

    public void SetDialogText(string _Text)
    {
        TempCoroutine = OneByOneTextSetting(_Text);
        StartCoroutine(TempCoroutine);
    }

    public void SetDialogText()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        if (currentEventCount >= eventDialog.Count)
        {
            isDialogOn = false;
            isOneByOneTextOn = false;
            CanvasManager.instance.OpenNPCUI();
            NPC.OpenNPCUI();
            return;
        }

        TempCoroutine = OneByOneTextSetting(eventDialog[currentEventCount].content);
        ++currentEventCount;
        StartCoroutine(TempCoroutine);
    }

    public void SetEventList(List<EventDialog> _TempRepeatEventList, NPC_Control _NPC)
    {
        currentEventCount = 0;
        eventDialog = _TempRepeatEventList;
        NPC = _NPC;
        chaseGameObject = NPC.gameObject;

        SetDialogText();
    }

    public void CloseTalkBox()
    {
        dialogText.text = "";
        currentEventCount = 0;
    }
    
    public void OneByOneTextSkip()
    {
        StopCoroutine(TempCoroutine);
        dialogText.text = completeText;
    }

    public IEnumerator OneByOneTextSetting(string _Content)
    {
        completeText = _Content;
        string _TempDialogText = "";
        int textCount = _Content.Length;

        for (int i = 0; i < textCount; ++i)
        {
            _TempDialogText += _Content[i];
            dialogText.text = _TempDialogText;
            yield return new WaitForSeconds(0.2f);
        }
        isOneByOneTextOn = false;
    }
}
