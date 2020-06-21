using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    public PlayerControl playerControl;
    public EventTrigger eventTrigger;

    public GameObject currentMap;
    public bool sceneMove;
    public int destinationSceneNumber;
    public Teleport sameSceneDestination;

    public int usableScenarioProgress = -1;

    private void Start()
    {
        objectType = InteractiveObjectType.Teleport;
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        eventTrigger = GetComponent<EventTrigger>();
    }
    private void Update()
    {
        if (DungeonManager.instance.mainQuest) return;  // 메인 이벤트가 실행중이면
        if (!inPlayer) return;  // 플레이어 캐릭터가 주변에 없으면
        if (CanvasManager.instance.GameMenuOnCheck()) return;   // UI가 켜져있으면
        if (!GameManager.instance.GetGameStart() || DungeonManager.instance.isSceneLoading)
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
            return;
        }   // 게임이 시작중이 아니거나 화면 전환중이면
        if (playerControl == null) return;  // 플레이어컨트롤이 등록되어있지 않으면

        if (!playerControl.PlayerIdleCheck())
        {
            if (playerControl.playerInputKey.activeInHierarchy)
                playerControl.playerInputKey.SetActive(false);
        }
        else
        {
            if (!playerControl.playerInputKey.activeInHierarchy)
                playerControl.playerInputKey.SetActive(true);
            
            if (Input.GetButtonDown("Fire1"))
            {
                if (eventCheckType == EventCheckType.InputKey)
                {
                    // 요구 진행도가 현재 진행도보다 높으면 대사출력후 행동 취소
                    if (usableScenarioProgress > DungeonManager.instance.scenarioManager.storyProgress)
                    {
                        DungeonManager.instance.ActiveInteractiveObject(objectNumber);
                        return;
                    }

                    // 이벤트 트리거가 비어있지 않고
                    if (eventTrigger.eventEndTrigger != null)
                    {
                        // 이벤트 트리거에 이벤트가 지정되어 있을 경우
                        if (eventTrigger.eventName.Length != 0)
                        {
                            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);

                            // 이벤트 트리거에 있는 이벤트를 던전매니저에 보냄 ( 키 선택후 실행 )
                            DungeonManager.instance.startWaitingEvent = EventStart;
                        }
                    }
                }

                player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);

                if (sceneMove)
                {
                    SceneMoveTeleport();
                }
                else
                {
                    SameSceneMoveTeleport();
                }
            }
        }
    }

    public void EventStart()
    {
        // 이벤트 있을 때
        if (eventTrigger.EventStart())
        {
            eventTrigger.eventEndTrigger.eventEndDelegate = DungeonManager.instance.TeleportNextFloor;
        }
    }

    public void SceneMoveTeleport()
    {
        DungeonManager.instance.ActiveInteractiveTeleport(destinationSceneNumber, objectNumber);
    }
    public void SameSceneMoveTeleport()
    {
        DungeonManager.instance.ActiveInteractiveTeleport(objectNumber, sameSceneDestination);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;

        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;

            if (eventCheckType != EventCheckType.TriggerEnter) return;
            Debug.Log("Event Trigger");
            // 이벤트 트리거가 비어있지 않고
            if (eventTrigger.eventEndTrigger == null) return;
            Debug.Log("Event Trigger not null");
            if (eventTrigger.eventName.Length == 0) return;
            Debug.Log("Event Trigger start");

            eventTrigger.EventStart();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;

        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;

            if (eventCheckType != EventCheckType.TriggerEnter) return;
            // 이벤트 트리거가 비어있지 않고
            if (eventTrigger.eventEndTrigger == null) return;
            if (eventTrigger.eventName.Length == 0) return;
            Debug.Log("Event Trigger start");

            eventTrigger.EventStart();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            player = collision.gameObject;
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
        }
    }
}