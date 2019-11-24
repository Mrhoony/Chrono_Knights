using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Blacksmith : NPC_Control
{
    public GameObject traningUI;
    public bool openTraningUI;

    // Update is called once per frame
    void Update()
    {
        if (!menu_inGame.CancelOn)
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
        traningUI.GetComponent<Menu_Traning>().OpenTraningMenu();
    }

    public void CloseTraningMenu(bool inGame)
    {
        traningUI.GetComponent<Menu_Traning>().CloseTraningMenu();
        traningUI.SetActive(false);
        openTraningUI = false;
        Time.timeScale = 1;
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
