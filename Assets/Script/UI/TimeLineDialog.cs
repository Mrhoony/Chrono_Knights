using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineDialog : TalkBox
{
    public PlayableDirector playableDirector;
    
    public string eventName;


    private void OnEnable()
    {
        if(currentEventCount == 0)
        {
            Debug.Log("start");
            completeText = "";
            eventDialog = DungeonManager.instance.scenarioManager.GetEventTextlist(eventName);

            if (eventDialog == null)
            {
                return;
            }
        }

        Debug.Log("get event");

        playableDirector.Pause();

        SetDialogText();
        isDialogOn = true;
    }

    private void OnDisable()
    {
        isDialogOn = false;
    }

    public void PlayerRestart()
    {
        playableDirector.Pause();
    }

    public new void Update()
    {
        if (!isDialogOn) return;

        if (chaseGameObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 0.5f);
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
    
    public new void SetDialogText()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        if (currentEventCount >= eventDialog.Count)
        {
            isOneByOneTextOn = false;
            
            playableDirector.Stop();

            CanvasManager.instance.isMainScenarioOn = false;
            CameraManager.instance.CameraFocusOff(0.1f);

            playableDirector.gameObject.SetActive(false);

            return;

            //GameObject event1 = GameObject.Find("Event1");
            //CameraManager.instance.CameraFocus(event1);
            //event1.GetComponent<PlayableDirector>().Play();
        }

        TempCoroutine = OneByOneTextSetting(eventDialog[currentEventCount].content);
        ++currentEventCount;
        StartCoroutine(TempCoroutine);
    }
}
