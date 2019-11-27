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
        if (!MainUI_Menu.CancelOn && !MainUI_Menu.InventoryOn)
        {
            if (inPlayer && !EnchantUI.GetComponent<Menu_Enchant>().upgradeSet && !UpgradeUI.GetComponent<Menu_Upgrade>().upgradeSet)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    //OpenUpgradeMenu();
                    OpenEnchantMenu();
                }
            }
        }
    }

    public void OpenEnchantMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        openEnchantUI = true;
        EnchantUI.SetActive(true);
        EnchantUI.GetComponent<Menu_Enchant>().OpenEnchantMenu();
    }
    public void CloseEnchantMenu()
    {
        EnchantUI.SetActive(false);
        openEnchantUI = false;
        EnchantUI.GetComponent<Menu_Enchant>().upgradeSet = false;
        player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OpenUpgradeMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        openUpgradeUI = true;
        UpgradeUI.SetActive(true);
        UpgradeUI.GetComponent<Menu_Upgrade>().OpenUpgradeMenu();
    }
    public void CloseUpgradeMenu()
    {
        UpgradeUI.SetActive(false);
        openUpgradeUI = false;
        UpgradeUI.GetComponent<Menu_Upgrade>().upgradeSet = false;
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
