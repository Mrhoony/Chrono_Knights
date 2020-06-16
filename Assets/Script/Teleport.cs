using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    public PlayerControl playerControl;
    public bool sceneMove;
    public int destinationSceneNumber;
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
                if (usableScenarioProgress > DungeonManager.instance.scenarioManager.storyProgress)
                {
                    DungeonManager.instance.ActiveInteractiveObject(objectNumber);
                    return;
                }

                if (gameObject.GetComponent<EventTrigger>().eventName.Length != 0)
                {
                    if (eventCheckType == EventCheckType.InputKey)
                    {
                        if (gameObject.GetComponent<EventTrigger>().eventEndTrigger != null)
                            gameObject.GetComponent<EventTrigger>().eventEndTrigger.eventEndDelegate = SceneMoveTeleport;

                        gameObject.GetComponent<EventTrigger>().EventStart();

                        return;
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
            
            if (gameObject.GetComponent<EventTrigger>().eventName.Length == 0) return;
            
            if (eventCheckType == EventCheckType.TriggerEnter)
                gameObject.GetComponent<EventTrigger>().EventStart();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;

        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;

            if (gameObject.GetComponent<EventTrigger>().eventName.Length == 0) return;
            
            if (eventCheckType == EventCheckType.TriggerEnter)
                gameObject.GetComponent<EventTrigger>().EventStart();
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