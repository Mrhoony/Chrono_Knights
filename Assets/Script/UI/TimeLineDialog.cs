using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineDialog : TalkBox
{
    public PlayableDirector playableDirector;
    Material spriteOutLine;
    Material spriteDiffuse;

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
        spriteOutLine = Resources.Load<Material>("Material/SpriteOutLine");
        spriteDiffuse = Resources.Load<Material>("Material/SpriteDiffuse");

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
                // 조건 변경 - 대화가 아닌 카메라 이동, 액션 등일 경우 resume
                if (currentEventCount >= eventDialog.Count)
                {
                    playableDirector.Stop();

                    CameraManager.instance.CameraFocusOff(0.1f);
                    CanvasManager.instance.isMainScenarioOn = false;

                    playableDirector.gameObject.SetActive(false);
                }
                chaseGameObject.GetComponent<SpriteRenderer>().material = spriteDiffuse;
                SetDialogText();
                //playableDirector.Resume();
            }
        }
    }

    private void LateUpdate()
    {
        if (chaseGameObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 0.5f), 8f * Time.deltaTime);
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
        }
        if (eventDialog[currentEventCount].NPCName != "")
        {
            chaseGameObject = GameObject.Find(eventDialog[currentEventCount].NPCName);

            if (chaseGameObject != null)
            {
                CameraManager.instance.CameraFocus(chaseGameObject);
                chaseGameObject.GetComponent<SpriteRenderer>().material = spriteOutLine;
            }
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
