using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    public PlayerControl playerControl;
    public bool sceneMove;
    public int destinationSceneNumber;
    public EventTrigger eventTrigger;
    public GameObject currentMap;
    public Teleport sameSceneDestination;
    public int usableScenarioProgress = -1;

    private void Start()
    {
        objectType = InteractiveObjectType.Teleport;
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if (DungeonManager.instance.mainQuest) return;
        if (!inPlayer) return;
        if (CanvasManager.instance.GameMenuOnCheck()) return;
        if (!GameManager.instance.GetGameStart() || DungeonManager.instance.isSceneLoading)
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
            return;
        }
        if (playerControl == null) return;

        if (playerControl.PlayerIdleCheck())
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
                            if (eventTrigger.EventStart())
                            {
                                if (sceneMove)
                                {
                                    eventTrigger.eventEndTrigger.eventEndDelegate = SceneMoveTeleport;
                                }
                                else
                                {
                                    eventTrigger.eventEndTrigger.eventEndDelegate = SameSceneMoveTeleport;
                                }
                                return;
                            }
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
            playerControl = player.GetComponent<PlayerControl>();

            if (eventCheckType != EventCheckType.TriggerEnter) return;
            // 이벤트 트리거가 비어있지 않고
            if (eventTrigger.eventEndTrigger == null) return;
            if (eventTrigger.eventName.Length == 0) return;

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