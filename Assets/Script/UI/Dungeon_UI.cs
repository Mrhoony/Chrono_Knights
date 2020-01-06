using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon_UI : MonoBehaviour
{
    public GameObject dungeonFloor;
    public Text dungeonFloorText;
    public GameObject dungeonFloorEft;
    public Text dungeonFloorEftText;
    
    public void SetDungeonFloor(int stage)
    {
        dungeonFloorText.text = stage.ToString() + "F";
        dungeonFloorEftText.text = stage.ToString() + "F";
        StartCoroutine(DungeonFloorEft());
    }

    IEnumerator DungeonFloorEft()
    {
        dungeonFloorEft.SetActive(true);
        yield return new WaitForSeconds(2f);
        dungeonFloorEft.SetActive(false);
    }
}
