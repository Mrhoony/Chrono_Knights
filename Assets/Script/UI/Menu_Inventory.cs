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

    public Sprite[] keyItemBorderSprite;

    public NPC_Blacksmith npc_Blacksmith;

    bool upgrade;
    int focused = 0;

    private void Awake()
    {
        transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length;
        slot = new GameObject[slotCount - 1];
        upgrade = false;
        keyItemBorderSprite = Resources.LoadAll<Sprite>("UI/Inventory_Set");

        inventoryKeylist = new Key[slotCount-1];
        for (int i=1;i<slotCount; ++i)
        {
            slot[i-1] = transforms[i].gameObject;
            slot[i-1].transform.GetChild(1).gameObject.SetActive(true);
        }
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(6); }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-6); }

        if (!upgrade)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                gameObject.transform.parent.GetComponent<Menu_InGame>().CloseInventory();
            }
        }

        if(upgrade)
        {
            if (Input.GetKeyDown(KeyCode.Z))    // 인챈트할때 Z
            {
                if (isFull[focused])
                {
                    upgrade = false;
                    EnchantKeyItemSet(focused);
                    gameObject.transform.parent.GetComponent<Menu_InGame>().CloseInventory();
                }
            }
            if (Input.GetKeyDown(KeyCode.X))    // 인챈트할때 Z
            {
                if (isFull[focused])
                {
                    upgrade = false;
                    gameObject.transform.parent.GetComponent<Menu_InGame>().CloseInventory();
                }
            }
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

                slot[i].transform.GetChild(1).GetComponent<Image>().sprite = _key.sprite;
                slot[i].GetComponent<Image>().sprite = keyItemBorderSprite[11 - _key.keyRarity];
                inventoryKeylist[i] = _key;
                break;
            }
        }
    }
    public void OpenInventory()
    {
        upgrade = true;
        npc_Blacksmith = GameObject.Find("Blacksmith").GetComponent<NPC_Blacksmith>();
    }

    public void EnchantKeyItemSet(int focus)
    {
        npc_Blacksmith.EnchantUI.GetComponent<Menu_Enchant>().SetEnchantKey(inventoryKeylist[focus], focus);
    }

    public void EnchantedKey(int _focus)
    {
        inventoryKeylist[_focus] = null;
        isFull[_focus] = false;
        SlotDefault(_focus);
    }

    void SlotDefault(int _focus)
    {
        slot[_focus].transform.GetChild(1).GetComponent<Image>().sprite = null;
        slot[_focus].GetComponent<Image>().sprite = keyItemBorderSprite[6];
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focused + AdjustValue < 0 || focused + AdjustValue > 23) { return; }

        slot[focused].transform.GetChild(0).gameObject.SetActive(false);
        focused += AdjustValue;
        slot[focused].transform.GetChild(0).gameObject.SetActive(true);
    }
}
