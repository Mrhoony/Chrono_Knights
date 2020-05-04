using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractiveObject
{
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
}