using UnityEngine;

public abstract class Menu_InGameMenu : MonoBehaviour
{
    public CanvasManager canvasManager;
    public GameObject slots;
    public GameObject[] slot;
    public Slot slotInstance;
    public GameObject itemInformation;

    public Item[] itemList;

    public int slotCount;
    public int availableSlot;
    public bool isUIOn;
    public int focused = 0;
    public bool isItemSelect;

    public abstract void Init();
    public abstract void FocusedSlot(int AdjustValue);
}
