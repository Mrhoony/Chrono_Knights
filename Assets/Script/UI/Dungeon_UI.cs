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

    public GameObject bossClearTrialWindow;
    public GameObject[] trialCard;
    public GameObject trialCursor;
    public float cursorSpd;
    public int focused;

    private void Update()
    {
        if (!isTrialCardSelectOn) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {

        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        FocusMove();
    }

    public void OpenTrialCardSelectMenu()
    {
        focused = -1;
        bossClearTrialWindow.SetActive(true);
        isTrialCardSelectOn = true;
    }
    public void CloseTrialCardSelectMenu()
    {
        isTrialCardSelectOn = false;
        bossClearTrialWindow.SetActive(false);
        trialCursor.SetActive(false);
    }
    public void SetTrialOption()
    {
        int statusRandom = 0;
        int valueRandom = 0;
        for(int i = 0; i < 3; ++i)
        {
            statusRandom = Random.Range(0, 3);
            valueRandom = Random.Range(0, 3);
            if(statusRandom == 2) valueRandom *= 10;
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
