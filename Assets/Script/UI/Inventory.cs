using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] Slot;
    public bool[] isFull;
    
    GameObject _Player;
    int Focused = 0;

    private void Awake()
    {
        Slot[Focused].transform.GetChild(0).gameObject.SetActive(true);
        _Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) { CloseInventory(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }
    }

    void CloseInventory()
    {
        // _Player.GetComponent<PlayerController>().enabled = true;
        gameObject.SetActive(false);
    }

    void FocusedSlot(int AdjustValue)
    {
        if (Focused + AdjustValue < 0 || Focused + AdjustValue > 23) { return; }

        Slot[Focused].transform.GetChild(0).gameObject.SetActive(false);
        Focused += AdjustValue;
        Slot[Focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
