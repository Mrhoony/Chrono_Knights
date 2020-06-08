using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineDialog : MonoBehaviour
{
    public GameObject chaseGameObject;
    public PlayableDirector playableDirector;

    public Text dialogText;
    public string eventName;

    public bool isDialogOn;
    public bool isOneByOneTextOn;
    public int currentEventCount = 0;
    public string completeText;

    List<EventDialog> eventDialog;

    public IEnumerator TempCoroutine;

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

    public void Update()
    {
        if (!isDialogOn) return;

        if (chaseGameObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 0.5f);
        }

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            Debug.Log("test");

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
    
    public void SetDialogText()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        if (currentEventCount >= eventDialog.Count)
        {
            isOneByOneTextOn = false;
            
            playableDirector.Stop();
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
            yield return new WaitForSeconds(1f);
        }

        isOneByOneTextOn = false;
    }
}
