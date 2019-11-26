using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Blacksmith : NPC_Control
{
    public GameObject UpgradeUI;
    public GameObject EnchantUI;

    public bool openEnchantUI;
    public bool openUpgradeUI;

    // Update is called once per frame
    void Update()
    {
        if (!menu_inGame.CancelOn && !menu_inGame.InventoryOn)
        {
            if (inPlayer && !EnchantUI.GetComponent<Menu_Enchant>().upgradeSet && !UpgradeUI.GetComponent<Menu_Upgrade>().upgradeSet)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    //OpenUpgradeMenu();
                    OpenEnchantMenu();
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (openUpgradeUI)
                    {
                        //CloseUpgradeMenu();
                        //openUpgradeUI = false;
                        CloseEnchantMenu();
                        openUpgradeUI = false;
                    }
                }
            }
        }
    }

    public void OpenUpgradeMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        openUpgradeUI = true;
        UpgradeUI.SetActive(true);
        UpgradeUI.GetComponent<Menu_Upgrade>().OpenUpgradeMenu();
    }
    public void CloseUpgradeMenu()
    {
        UpgradeUI.SetActive(false);
        openUpgradeUI = false;
        Time.timeScale = 1;
        UpgradeUI.GetComponent<Menu_Upgrade>().upgradeSet = false;
        player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OpenEnchantMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        openEnchantUI = true;
        EnchantUI.SetActive(true);
        EnchantUI.GetComponent<Menu_Enchant>().OpenEnchantMenu();
    }
    public void CloseEnchantMenu()
    {
        EnchantUI.SetActive(false);
        openEnchantUI = false;
        Time.timeScale = 1;
        EnchantUI.GetComponent<Menu_Enchant>().upgradeSet = false;
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
