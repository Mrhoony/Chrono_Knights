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
    public GameObject gameOverWindow;
    
    public void SetDungeonFloor(int _stage, string _floorStat)
    {
        dungeonFloorText.text = _stage.ToString() + "F";
        dungeonFloorStatText.text = _floorStat;
        dungeonFloorEftText.text = _stage.ToString() + "F";
        StartCoroutine(DungeonFloorEft());
    }

    IEnumerator DungeonFloorEft()
    {
        dungeonFloor.SetActive(true);
        dungeonFloorEft.SetActive(true);
        yield return new WaitForSeconds(2f);
        dungeonFloorEft.SetActive(false);
    }

    public void OnGameOverWindow(bool _OnOff)
    {
        dungeonFloor.SetActive(false);
        gameOverWindow.SetActive(_OnOff);
    }
}
