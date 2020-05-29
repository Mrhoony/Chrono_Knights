﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    public bool sceneMove;
    public int destinationSceneNumber;
    public GameObject currentMap;
    public Teleport sameSceneDestination;

    private void Start()
    {
        objectType = InteractiveObjectType.Teleport;
    }

    private void Update()
    {
        if (!GameManager.instance.GetGameStart()) return;
        if (CanvasManager.instance.GameMenuOnCheck()) return;
        if (DungeonManager.instance.isSceneLoading) return;

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