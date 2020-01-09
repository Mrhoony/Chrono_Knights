using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Blacksmith : NPC_Control
{
    public GameObject enchantUI;
    public GameObject upgradeUI;
    public GameObject selectUI;
    public GameObject[] button;

    private bool openSelectUI;
    private bool openEnchantUI;
    private bool openUpgradeUI;

    public int focus;

    // Update is called once per frame
    void Update()
    {
        if (!inPlayer) return;
        if (menu.isCancelOn || menu.isInventoryOn) return;
        if (openEnchantUI || openUpgradeUI) return;
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!openSelectUI)
            {
                if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                OpenSelectMenu();
            }
            else
            {
                switch (focus)
                {
                    case 0:
                        OpenEnchantMenu();
                        break;
                    case 1:
                        OpenUpgradeMenu();
                        break;
                    case 2:
                        CloseSelectMenu();
                        break;
                }
            }
        }

        if (openSelectUI)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) { focus = FocusedSlot(button, 1, focus); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { focus = FocusedSlot(button, -1, focus); }

            if (Input.GetKeyDown(KeyCode.X))
            {
                CloseSelectMenu();
            }
        }

    }

    public int FocusedSlot(GameObject[] slots, int AdjustValue, int focused)
    {
        slots[focused].transform.GetChild(0).gameObject.SetActive(false);

        focused += AdjustValue;

        if (focused < 0)
            focused = 2;
        if (focused > 2)
            focused = 0;

        slots[focused].transform.GetChild(0).gameObject.SetActive(true);

        return focused;
    }

    public void OpenEnchantMenu()
    {
        openEnchantUI = true;
        enchantUI.SetActive(true);
        enchantUI.GetComponent<Menu_Enchant>().OpenEnchantMenu();
    }
    public void CloseEnchantMenu()
    {
        enchantUI.SetActive(false);
        openEnchantUI = false;
    }

    public void OpenUpgradeMenu()
    {
        openUpgradeUI = true;
        upgradeUI.SetActive(true);
        upgradeUI.GetComponent<Menu_Upgrade>().OpenUpgradeMenu();
    }
    public void CloseUpgradeMenu()
    {
        upgradeUI.SetActive(false);
        openUpgradeUI = false;
    }

    public void OpenSelectMenu()
    {
        player.GetComponent<PlayerControl>().enabled = false;
        focus = 0;
        openSelectUI = true;
        selectUI.SetActive(true);
        button[focus].transform.GetChild(0).gameObject.SetActive(true);

    }
    public void CloseSelectMenu()
    {
        button[focus].transform.GetChild(0).gameObject.SetActive(false);
        openSelectUI = false;
        selectUI.SetActive(false);
        player.GetComponent<PlayerControl>().enabled = true;
    }
}
