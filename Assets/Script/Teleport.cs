using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
    private void Update()
    {
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

                DungeonManager.instance.ActiveInteractiveObject(isNPC, objectNumber);
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