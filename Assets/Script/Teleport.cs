using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;
    public bool entrance;   // 입구면 true, 출구면 false
    public bool inPlayer;

    private void Start()
    {
        dm = DungeonManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
