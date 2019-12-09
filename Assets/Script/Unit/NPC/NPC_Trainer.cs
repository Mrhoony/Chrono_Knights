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
        if (!menu.isCancelOn && !menu.isInventoryOn)
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
                        CloseTraningMenu();
                        openTraningUI = false;
                    }
                }
            }
        }
    }

    public void OpenTraningMenu()
    {
        Time.timeScale = 0;
        player.GetComponent<PlayerControl>().enabled = false;
        openTraningUI = true;
        traningUI.SetActive(true);
    }

    public void CloseTraningMenu()
    {
        openTraningUI = false;
        traningUI.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = true;
        Time.timeScale = 1;
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
