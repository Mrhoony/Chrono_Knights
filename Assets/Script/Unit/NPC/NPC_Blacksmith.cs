using UnityEngine;

public class NPC_Blacksmith : NPC_Control
{
    public GameObject selectUI;
    public GameObject[] button;

    private bool openSelectUI;
    public int focus;

    // Update is called once per frame
    void Update()
    {
        if (OpenUICheck() || canvasManager.DialogBoxOn()) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (!openSelectUI)
            {
                if (PlayerControl.instance.GetActionState() != ActionState.Idle) return;
                DungeonManager.instance.ActiveInteractiveNPC(this);
            }
            else
            {
                switch (focus)
                {
                    case 0:
                        canvasManager.OpenEnchantMenu();
                        break;
                    case 1:
                        canvasManager.OpenUpgradeMenu();
                        break;
                    case 2:
                        CloseSelectMenu();
                        break;
                }
            }
        }

        if (!openSelectUI) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { focus = FocusedSlot(button, 1, focus); }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { focus = FocusedSlot(button, -1, focus); }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            CloseSelectMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSelectMenu();
        }
    }

    public override void OpenNPCUI()
    {
        OpenSelectMenu();
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

    public void OpenSelectMenu()
    {
        canvasManager.PlayerMoveStop();
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
        StartCoroutine(canvasManager.PlayerMoveEnable());
    }
}
