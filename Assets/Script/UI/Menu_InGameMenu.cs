using UnityEngine;

public abstract class Menu_InGameMenu : FocusUI
{
    public CanvasManager canvasManager;
    public GameObject slots;
    public GameObject[] slot;
    public Slot slotInstance;
    public GameObject itemInformation;

    public Item[] itemList;

    public int slotCount;
    public int availableSlot;
    public bool isItemSelect;

    public abstract void Init();
}
