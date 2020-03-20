using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu_InGameMenu : MonoBehaviour
{
    public CanvasManager canvasManager;
    public GameObject slots;
    public GameObject[] slot;
    public Slot slotInstance;
    public GameObject itemInformation;

    public Item[] itemList;
    public bool[] isFull;               // 슬롯이 비었는지 아닌지

    public int slotCount;
    public int availableSlot;
    public bool isUIOn;
    public int focused = 0;
    public bool isItemSelect;

    public abstract void Init();
    public abstract void FocusedSlot(int AdjustValue);
}
