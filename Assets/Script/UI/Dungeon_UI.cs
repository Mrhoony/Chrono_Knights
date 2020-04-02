using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_UI : MonoBehaviour
{
    public GameObject dungeonFloor;
    public GameObject dungeonFloorEft;
    public Text dungeonFloorText;
    public Text dungeonFloorStatText;
    public Text dungeonFloorEftText;
    public GameObject dungeonPoolManager;

    public bool isTrialCardSelectOn;

    public GameObject bossClearTrialMenu;
    public GameObject[] trialCard;
    public GameObject trialCursor;
    public float cursorSpd;
    public int focused;

    private void Update()
    {
        if (!isTrialCardSelectOn) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            trialCard[focused].GetComponent<DungeonTrialSlot>().SelectThisCard();
            CloseTrialCardSelectMenu();
        }

        if (focused < 0 || focused > 2) return;
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        FocusMove();
    }

    public void OpenTrialCardSelectMenu()
    {
        focused = -1;
        bossClearTrialMenu.SetActive(true);
        SetTrialOption();
        isTrialCardSelectOn = true;
    }
    public void CloseTrialCardSelectMenu()
    {
        isTrialCardSelectOn = false;
        bossClearTrialMenu.SetActive(false);
        trialCursor.SetActive(false);
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

    public void FocusMove()
    {
        trialCursor.transform.position = Vector2.Lerp(trialCursor.transform.position, trialCard[focused].transform.position, Time.deltaTime * cursorSpd);
    }
    public void FocusedSlot(int AdjustValue)
    {
        trialCursor.SetActive(true);
        if (focused + AdjustValue > 2) focused = 0;
        else if (focused + AdjustValue < 0) focused = 2;
        else focused += AdjustValue;
    }
}
