using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Inventory : MonoBehaviour
{
    GameObject _Player;
    public GameObject slots;
    public Transform[] transforms;
    public int slotCount;
    public GameObject[] slot;
    public bool[] isFull;
    public Key[] inventoryKeylist;
    Sprite[] keyItemBorderSprite;

    bool upgrade;
    int Focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length;
        slot = new GameObject[slotCount - 1];
        upgrade = false;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");

        for (int i=1;i<slotCount; ++i)
        {
            slot[i-1] = transforms[i].gameObject;
        }
        slot[Focused].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!upgrade)
        {
            if (Input.GetKeyDown(KeyCode.I)) { CloseInventory(); }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

            if (Input.GetKeyDown(KeyCode.Z))
                ItemSelect(Focused);
        }
    }

    public void GetKeyItem(Key _key)
    {
        int slotCount = slot.Length;
        for (int i = 0; i < slotCount; i++)
        {
            if (!isFull[i])
            {
                isFull[i] = true;
                slot[i].GetComponent<SpriteRenderer>().sprite = _key.sprite;
                Debug.Log(_key.keyRarity);
                slot[i].GetComponent<Image>().sprite = keyItemBorderSprite[11 - _key.keyRarity];
                break;
            }
        }
    }
    public void OpenInventory()
    {
        upgrade = true;
    }

    public Key ItemSelect(int focus)
    {
        return inventoryKeylist[focus];
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
