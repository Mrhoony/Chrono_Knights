using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
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

        if (player.GetComponent<PlayerControl>().PlayerIdleCheck())
        {
            if (player.GetComponent<PlayerControl>().playerInputKey.activeInHierarchy)
                player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
        }
        else
        {
            if (!player.GetComponent<PlayerControl>().playerInputKey.activeInHierarchy)
                player.GetComponent<PlayerControl>().playerInputKey.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                if (usableScenarioProgress > DungeonManager.instance.scenarioManager.storyProgress) return;

                if (gameObject.GetComponent<EventTrigger>().eventName.Length != 0)
                {
                    gameObject.GetComponent<EventTrigger>().EventStart();
                }
                else
                {
                    player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);

                    if (sceneMove)
                    {
                        DungeonManager.instance.ActiveInteractiveTeleport(destinationSceneNumber);
                    }
                    else
                    {
                        DungeonManager.instance.ActiveInteractiveTeleport(objectNumber, sameSceneDestination);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;
            
            if (gameObject.GetComponent<EventTrigger>().eventName.Length == 0) return;
            Debug.Log("TriggerEnterEvent");
            if (eventCheckType == EventCheckType.TriggerEnter)
                gameObject.GetComponent<EventTrigger>().EventStart();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;
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