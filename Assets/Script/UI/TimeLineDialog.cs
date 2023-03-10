using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimeLineDialog : TalkBox
{
    public PlayableDirector playableDirector;
    public GameObject TalkBoxImage;
    Material spriteOutLine;
    Material spriteDiffuse;

    public GameObject cursor;
    public float cursorSpeed;

    public float clampedX;
    public float clampedY;

    public GameObject choises;
    public Text[] choise;

    public int focused;
    public int maxFocused;
    public int[] selectedChoiced;
    public int[] activedEvent;
    public string[] butterflyEffectName;
    public int[] butterflyEffectBool;

    public bool cameraEvent;
    public bool isChoiceOn;
    public bool cameraScrollOn;
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
        
        spriteOutLine = Resources.Load<Material>("Material/SpriteOutLine");
        spriteDiffuse = Resources.Load<Material>("Material/SpriteDiffuse");

        isChoiceOn = false;
        cameraEvent = false;

        SetDialogText();
    }

    public new void Update()
    {
        if (!isDialogOn) return;
        if (cameraEvent) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            // 선택하는 경우
            if (isChoiceOn)
            {
                isChoiceOn = false;
                cursor.SetActive(false);
                for(int i = 0; i < maxFocused; ++i)
                {
                    choise[i].gameObject.SetActive(false);
                }
                choises.SetActive(false);
                currentEventCount = selectedChoiced[focused]-1;
                DungeonManager.instance.scenarioManager.ButterflyEffectSelect(butterflyEffectName[focused], butterflyEffectBool[focused]);

                SetTalkBox();

                return;
            }

            // 텍스트 한글자씩 출력
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
        // 선택지 포커스 이동
        if (isChoiceOn)
        {
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) FocusedSlot(1);
            if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) FocusedSlot(-1);
            FocusMove(choise[focused].gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (chaseGameObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(chaseGameObject.transform.position + Vector3.up * 1f), 8f * Time.deltaTime);
            if (!cameraScrollOn)
            {
                clampedX = Mathf.Clamp(transform.position.x,
                    Camera.main.WorldToScreenPoint(new Vector2(-Camera.main.orthographicSize + Camera.main.transform.position.x - 0.9f, 0)).x,
                    Camera.main.WorldToScreenPoint(new Vector2(Camera.main.orthographicSize + Camera.main.transform.position.x + 0.9f, 0)).x);
                clampedY = Mathf.Clamp(transform.position.y,
                    Camera.main.WorldToScreenPoint(new Vector2(0, -Camera.main.orthographicSize * 1280 / 720 + Camera.main.transform.position.y + 1.2f)).y,
                    Camera.main.WorldToScreenPoint(new Vector2(0, Camera.main.orthographicSize * 1280 / 720 + Camera.main.transform.position.y - 1.8f)).y);
                transform.position = new Vector3(clampedX, clampedY, -10f);
            }
        }
    }

    public void SetTalkBox()
    {
        isDialogOn = true;
        isOneByOneTextOn = true;

        if(eventDialog[currentEventCount].content == "none")
        {
            ++currentEventCount;
            playableDirector.Resume();
            return;
        }

        TempCoroutine = OneByOneTextSetting(eventDialog[currentEventCount].content);
        ++currentEventCount;
        StartCoroutine(TempCoroutine);
        playableDirector.Pause();
    }

    public new void SetDialogText()
    {
        // 다음 내용이 없을때 초기화하고 종료
        if (currentEventCount >= eventDialog.Count)
        {
            isDialogOn = false;
            isChoiceOn = false;
            cameraEvent = false;
            currentEventCount = 0;

            //playableDirector.Stop();

            CameraManager.instance.CameraFocusOff(0.1f);
            CanvasManager.instance.MainScenarioEnd();

            playableDirector.Resume();
            //playableDirector.gameObject.SetActive(false);

            return;
        }

        cameraScrollOn = false;

        switch (eventDialog[currentEventCount].eventType)
        {
            case EventType.CameraFocus:
                {
                    Debug.Log("Timeline camera focus");

                    ChaseObjectSet();
                    if (chaseGameObject != null)
                    {
                        CameraManager.instance.CameraFocus(chaseGameObject);
                        // chaseGameObject.GetComponent<SpriteRenderer>().material = spriteOutLine;

                        SetTalkBox();
                    }
                    else
                        Debug.Log("dialog xml 오브젝트 이름 입력 실수");
                    break;
                }
            case EventType.CameraScroll:
                {
                    Debug.Log("Timeline camera scroll");

                    if (eventDialog[currentEventCount].NPCName != "")
                    {
                        string[] chaseObjects = eventDialog[currentEventCount].NPCName.Split(':');

                        if (GameObject.Find(chaseObjects[0]) == null || GameObject.Find(chaseObjects[1]) == null)
                        {
                            Debug.Log(currentEventCount + " dialog xml 오브젝트 이름 입력 실수");
                            return;
                        }
                        isDialogOn = true;
                        cameraScrollOn = true;
                        CameraManager.instance.CameraScroll(GameObject.Find(chaseObjects[0]), GameObject.Find(chaseObjects[1]), float.Parse(chaseObjects[2]));
                        ++currentEventCount;
                        playableDirector.Pause();
                    }
                    break;
                }
            case EventType.None:
                {
                    Debug.Log("Timeline none");

                    ChaseObjectSet();
                    if (chaseGameObject != null)
                    {
                        SetTalkBox();
                    }
                    else
                    {
                        Debug.Log("dialog xml 오브젝트 이름 입력 실수");
                    }
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
                    butterflyEffectName = new string[maxFocused];
                    butterflyEffectBool = new int[maxFocused];

                    for (int i = 0; i < maxFocused; ++i)
                    {
                        string[] choice = result[i].Split(':');
                        choise[i].text = choice[0];
                        selectedChoiced[i] = int.Parse(choice[1]);
                        butterflyEffectName[i] = choice[2];
                        butterflyEffectBool[i] = int.Parse(choice[3]);

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
                    Debug.Log("Timeline answer");
                    ++currentEventCount;
                    break;
                }
            default:
                {
                    Debug.Log("Timeline camera");
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
                TalkBoxImage.SetActive(false);
                return;
            }
            else
                TalkBoxImage.SetActive(true);
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
