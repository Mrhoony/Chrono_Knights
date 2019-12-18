using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool entrance;   // 입구면 true, 출구면 false
    public int useSystem;   // 이동 할 씬 번호
    public bool inPlayer;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            collision.transform.GetChild(1).gameObject.SetActive(true);
            DungeonManager.instance.useTeleportSystem = useSystem;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            collision.transform.GetChild(1).gameObject.SetActive(false);
            DungeonManager.instance.useTeleportSystem = 10;
        }
    }
}
