using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;
    GameObject menu;
    bool inPlayer;

    private void Awake()
    {
        menu = MainUI.instance.UI_Menu;
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

    // 임시로 클리어시 마을로 복귀
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                dm.SelectedKey(0, 0, false);
            }
        }
    }
}
