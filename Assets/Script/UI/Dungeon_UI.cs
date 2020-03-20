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
    
    public void SetDungeonFloor(int _stage, string _floorStat)
    {
        dungeonFloor.SetActive(true);
        dungeonPoolManager.SetActive(true);
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
