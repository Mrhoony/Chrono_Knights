using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;
    GameObject menu;

    private void Awake()
    {
        menu = CanvasManager.instance.UI_Menu;
        dm = DungeonManager.instance;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!menu.GetComponent<Menu_InGame>().InventoryOn && !menu.GetComponent<Menu_InGame>().CancelOn)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (dm.GetDungeonClear())
                    {
                        dm.Teleport();
                    }
                }
            }
        }
    }
}
