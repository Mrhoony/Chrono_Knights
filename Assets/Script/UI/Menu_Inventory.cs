using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Inventory : MonoBehaviour
{
    public GameObject slots;
    public Transform[] transforms;
    public int slotCount;
    public GameObject[] slot;
    public bool[] isFull;
    
    GameObject _Player;
    int Focused = 0;

    private void Awake()
    {
        _Player = GameObject.Find("Player");

        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length;
        slot = new GameObject[slotCount - 1];

        for(int i=1;i<slotCount; ++i)
        {
            slot[i-1] = transforms[i].gameObject;
        }
        slot[Focused].transform.GetChild(0).gameObject.SetActive(true);
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

        slot[Focused].transform.GetChild(0).gameObject.SetActive(false);
        Focused += AdjustValue;
        slot[Focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
