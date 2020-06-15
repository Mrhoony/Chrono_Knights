using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : FocusUI
{
    public GameObject dungeonFloorCenterEffect;
    public Text dungeonFloorText;
    public Text dungeonFloorStatText;
    public Text dungeonFloorEftText;
    public GameObject dungeonPoolManager;

    public GameObject bossClearTrialMenu;
    public GameObject[] trialCard;

    private void Update()
    {
        if (!isUIOn) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if (focused < 0) return;
            trialCard[focused].GetComponent<DungeonTrialSlot>().SelectThisCard();
            CloseTrialCardSelectMenu();
        }

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Right"])) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Left"])) { FocusedSlot(-1); }

        if (focused < 0) return;
        FocusMove(trialCard[focused]);
    }

    public void OpenTrialCardSelectMenu()
    {
        focused = -1;
        bossClearTrialMenu.SetActive(true);
        SetTrialOption();
        isUIOn = true;
    }
    public void CloseTrialCardSelectMenu()
    {
        isUIOn = false;
        bossClearTrialMenu.SetActive(false);
        cursor.SetActive(false);
        CanvasManager.instance.CloseTrialCardSelectMenu();
    }
    public void SetTrialOption()
    {
        for(int i = 0; i < 3; ++i)
        {
            trialCard[i].GetComponent<DungeonTrialSlot>().SetTrialCard();
        }
    }
    
    public void SetDungeonFloor(int _stage, string _floorStat)
    {
        dungeonFloorText.text = _stage.ToString() + "F";
        dungeonFloorStatText.text = _floorStat;
        dungeonFloorEftText.text = _stage.ToString() + "F";
        StartCoroutine(DungeonFloorEft());
    }

    IEnumerator DungeonFloorEft()
    {
        dungeonFloorCenterEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        dungeonFloorCenterEffect.SetActive(false);
    }
}
