using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public int useSystem;   // 이동 할 씬 번호
    public bool inPlayer;

    private void Update()
    {
        if (!inPlayer) return; // 플레이어가 근처에 없으면 리턴
        if (player.GetComponent<PlayerControl>().inputDirection != 0)
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
        }       // 플레이어가 대기상태가 아니면
        else
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            DungeonManager.instance.useTeleportSystem = useSystem;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            DungeonManager.instance.useTeleportSystem = 10;
            player = collision.gameObject;
        }
    }
}
