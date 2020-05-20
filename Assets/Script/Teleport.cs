using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    public bool sceneMove;
    public GameObject currentMap;
    public Teleport sameSceneDestination;

    private void Start()
    {
        objectType = InteractiveObjectType.Teleport;
    }
    private void Update()
    {
        if (CanvasManager.instance.GameMenuOnCheck()) return;

        if (inPlayer)
        {
            if (player.GetComponent<PlayerControl>().inputDirection != 0)
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
                    if (gameObject.GetComponent<EventTrigger>() != null)
                    {
                        gameObject.GetComponent<EventTrigger>().EventStart();
                    }
                    else
                    {
                        DungeonManager.instance.ActiveInteractiveTeleport(sceneMove, objectNumber, sameSceneDestination);
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