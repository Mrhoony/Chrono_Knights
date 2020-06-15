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

    public GameObject cursor;
    public float cursorSpeed;

    public GameObject choises;
    public Text[] choise;

    public int focused;
    public int maxFocused;
    public int[] selectedChoiced;
    public int[] activedEvent;

    public bool cameraEvent;
    public bool isChoiceOn;
    public string eventName;
    
    private void OnEnable()
    {
        if(currentEventCount == 0)
        {
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

        isChoiceOn = false;
        cameraEvent = false;

        SetDialogText();
        playableDirector.Pause();
    }

    private void OnDisable()
    {
        isDialogOn = false;
    }

    public new void Update()
    {
        if (!isDialogOn) return;
        if (cameraEvent) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (isChoiceOn)
            {
                isChoiceOn = false;
                cursor.SetActive(false);
                for(int i = 0; i < maxFocused; ++i)
                {
                    choise[i].gameObject.SetActive(false);
                }
                choises.SetActive(false);
                currentEventCount = selectedChoiced[focused];

                SetTalkBox();
            }

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
        if (isChoiceOn)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) FocusedSlot(1);
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) FocusedSlot(-1);
            FocusMove(choise[focused].gameObject);
        }
    }
    private void LateUpdate()
    {
        if (chaseGameObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 1f), 8f * Time.deltaTime);
        }
    }

    public void SetTalkBox()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        TempCoroutine = OneByOneTextSetting(eventDialog[currentEventCount].content);
        ++currentEventCount;
        StartCoroutine(TempCoroutine);
    }
    public new void SetDialogText()
    {
        if (currentEventCount >= eventDialog.Count)
        {
            isOneByOneTextOn = false;
            
            playableDirector.Stop();

            CameraManager.instance.CameraFocusOff(0.1f);
            CanvasManager.instance.MainScenarioEnd();

            playableDirector.gameObject.SetActive(false);

            return;
        }
        
        switch (eventDialog[currentEventCount].eventType)
        {
            case EventType.CameraFocus:
                {
                    Debug.Log("camera focus");

                    ChaseObjectSet();
                    if (chaseGameObject != null)
                    {
                        CameraManager.instance.CameraFocus(chaseGameObject);
                        transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 1f);
                        chaseGameObject.GetComponent<SpriteRenderer>().material = spriteOutLine;

                        SetTalkBox();
                        playableDirector.Pause();
                    }
                    else
                        Debug.Log("dialog xml 오브젝트 이름 입력 실수");
                    break;
                }
            case EventType.None:
                {
                    Debug.Log("none");

                    ChaseObjectSet();
                    SetTalkBox();
                    playableDirector.Pause();
                    break;
                }
            case EventType.Select:
                {
                    Debug.Log("select");

                    ChaseObjectSet();
                    string[] result = eventDialog[currentEventCount].content.Split('@');

                    focused = 0;
                    maxFocused = result.Length;

                    selectedChoiced = new int[maxFocused];
                    activedEvent = new int[maxFocused];

                    for (int i = 0; i < maxFocused; ++i)
                    {
                        string[] choice = result[i].Split(':');
                        choise[i].text = choice[0];
                        selectedChoiced[i] = int.Parse(choice[1]);
                        choise[i].gameObject.SetActive(true);
                    }
                    choises.SetActive(true);
                    cursor.SetActive(true);

                    isDialogOn = true;
                    isChoiceOn = true;
                    playableDirector.Pause();
                    break;
                }
            case EventType.Answer:
                {
                    Debug.Log("answer");
                    isDialogOn = true;
                    isOneByOneTextOn = true;
                    ++currentEventCount;
                    break;
                }
            default:
                {
                    Debug.Log("Time line event Errrrrrrrrrr");
                    cameraEvent = true;
                    ++currentEventCount;

                    playableDirector.Resume();
                    break;
                }
        }
    }
    public void ChaseObjectSet()
    {
        if (eventDialog[currentEventCount].NPCName != "")
        {
            chaseGameObject = GameObject.Find(eventDialog[currentEventCount].NPCName);

            if (chaseGameObject == null)
            {
                Debug.Log("dialog xml 오브젝트 이름 입력 실수");
                return;
            }
        }
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
    }

    public void FocusMove(GameObject _FocusedObject)
    {
        if (focused < 0 || focused > maxFocused) return;
        cursor.transform.position = Vector2.Lerp(
            cursor.transform.position,
            new Vector2(cursor.transform.position.x, _FocusedObject.transform.position.y),
            Time.deltaTime * cursorSpeed);
    }
    public void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue > maxFocused - 1) focused = 0;
        else if (focused + AdjustValue < 0) focused = maxFocused - 1 ;
        else focused += AdjustValue;

        cursor.SetActive(true);
    }
}
