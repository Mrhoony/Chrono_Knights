using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trainer : NPC_Control
{
    public GameObject traningUI;
    public bool openTraningUI;

    // Update is called once per frame
    void Update()
    {
        if (!menu_inGame.CancelOn && !menu_inGame.InventoryOn)
        {
            if (inPlayer)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    OpenTraningMenu();
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    if (openTraningUI)
                    {
                        CloseTraningMenu(true);
                        openTraningUI = false;
                    }
                }
            }
        }
    }

    public void OpenTraningMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        openTraningUI = true;
        traningUI.SetActive(true);
    }

    public void CloseTraningMenu(bool inGame)
    {
        Time.timeScale = 1;
        openTraningUI = false;
        traningUI.SetActive(false);
        if (inGame)
            player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
