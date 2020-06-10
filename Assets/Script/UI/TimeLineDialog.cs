using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineDialog : TalkBox
{
    public PlayableDirector playableDirector;
    
    public string eventName;

    public bool hasToPause;
    
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

        if(hasToPause)
            playableDirector.Pause();

        SetDialogText();
        isDialogOn = true;
    }

    private void OnDisable()
    {
        isDialogOn = false;
    }

    public new void Update()
    {
        if (!isDialogOn) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (isOneByOneTextOn)
            {
                isOneByOneTextOn = false;
                OneByOneTextSkip();
            }
            else
            {
                if (currentEventCount >= eventDialog.Count)
                {
                    playableDirector.Stop();

                    CameraManager.instance.CameraFocusOff(0.1f);
                    CanvasManager.instance.isMainScenarioOn = false;

                    playableDirector.gameObject.SetActive(false);
                }
                playableDirector.Resume();
            }
        }
    }

    private void LateUpdate()
    {
        if (chaseGameObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 0.5f);
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

            CameraManager.instance.CameraFocusOff(0.1f);
            CanvasManager.instance.isMainScenarioOn = false;

            playableDirector.gameObject.SetActive(false);

            return;

            //GameObject event1 = GameObject.Find("Event1");
            //CameraManager.instance.CameraFocus(event1);
            //event1.GetComponent<PlayableDirector>().Play();
        }
        if (eventDialog[currentEventCount].NPCName != "")
        {
            chaseGameObject = GameObject.Find(eventDialog[currentEventCount].NPCName);

            if (chaseGameObject != null)
                CameraManager.instance.CameraFocus(chaseGameObject);
            else
                Debug.Log("dialog xml 오브젝트 이름 입력 실수");
        }

        TempCoroutine = OneByOneTextSetting(eventDialog[currentEventCount].content);
        ++currentEventCount;
        StartCoroutine(TempCoroutine);
    }

    public new IEnumerator OneByOneTextSetting(string _Content)
    {
        completeText = _Content;
        string _TempDialogText = "";
        int textCount = _Content.Length;

        for (int i = 0; i < textCount; ++i)
        {
            _TempDialogText += _Content[i];
            dialogText.text = _TempDialogText;
            yield return new WaitForSeconds(0.1f);
        }
        isOneByOneTextOn = false;
        playableDirector.Pause();
    }
}
