using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_UI : FocusUI
{
    public GameObject dungeonFloor;
    public GameObject dungeonFloorEft;
    public Text dungeonFloorText;
    public Text dungeonFloorStatText;
    public Text dungeonFloorEftText;
    public GameObject dungeonPoolManager;

    public GameObject bossClearTrialMenu;
    public GameObject[] trialCard;

    private void Update()
    {
        if (!isUIOn) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (focused < 0) return;
            trialCard[focused].GetComponent<DungeonTrialSlot>().SelectThisCard();
            CloseTrialCardSelectMenu();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }

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
        dungeonFloorEft.SetActive(true);
        yield return new WaitForSeconds(2f);
        dungeonFloorEft.SetActive(false);
    }
}
