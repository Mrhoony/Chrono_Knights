using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;
    CircleCollider2D cc;
    GameObject menu;

    private void Awake()
    {
        dm = DungeonManager.instance;
        cc = GetComponent<CircleCollider2D>();
        menu = CanvasManager.instance.UI_Menu;
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
