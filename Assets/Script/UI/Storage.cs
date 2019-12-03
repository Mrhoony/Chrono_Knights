using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public bool inPlayer;
    public bool onStorage;
    public MainUI_InGameMenu menu;

    private void Start()
    {
        onStorage = false;
    }

    private void Update()
    {
        if (!inPlayer) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            menu = GameObject.Find("UI/Menus").GetComponent<MainUI_InGameMenu>();
            if (menu.InventoryOn || menu.cancelOn) return;

            if (!onStorage)
            {
                onStorage = true;
                menu.OpenStorage();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onStorage)
            {
                onStorage = false;
            }
        }
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
