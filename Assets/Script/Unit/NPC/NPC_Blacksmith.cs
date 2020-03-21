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
        if (OpenUICheck()) return;

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
                        townUI.OpenEnchantMenu();
                        break;
                    case 1:
                        townUI.OpenUpgradeMenu();
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
        StartCoroutine(canvasManager.PlayerMoveEnable());
    }
}
